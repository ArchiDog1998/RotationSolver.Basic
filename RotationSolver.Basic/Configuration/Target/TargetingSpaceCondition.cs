using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Space Condition")]
internal class TargetingSpaceCondition : TargetingConditionBase
{
    internal enum ValueType : byte
    {
        Distance,

        [Description("Hitbox Radius")]
        Hitbox,
    }

    [UI("Value Type")]
    public ValueType Type { get; set; } = ValueType.Distance;

    [UI("Comparison")]
    public Comparison Compare { get; set; } = Comparison.Bigger;

    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("Value")]
    public float Value { get; set; } = 1;

    protected override bool IsTrueInside(IGameObject obj)
    {
        var value = Type switch
        {
            ValueType.Hitbox => obj.HitboxRadius,
            _ => obj.DistanceToPlayer(),
        };

        return Compare.Compare(value, Value);
    }
}