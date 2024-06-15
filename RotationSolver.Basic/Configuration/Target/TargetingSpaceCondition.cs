using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Space Condition")]
internal class TargetingSpaceCondition : TargetingConditionBase
{
    [UI("Value Type")]
    public ValueType Type { get; set; } = ValueType.Distance;

    [UI("Comparison")]
    public Comparison Compare { get; set; } = Comparison.Bigger;

    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("Value")]
    public float Value { get; set; } = 1;

    public override bool IsTrue(GameObject obj)
    {
        var value = Type switch
        {
            ValueType.Hitbox => obj.HitboxRadius,
            _ => obj.DistanceToPlayer(),
        };

        return Compare switch
        {
            Comparison.Bigger => value > Value,
            Comparison.Smaller => value < Value,
            Comparison.Equal => value == Value,
            Comparison.BiggerOrEqual => value >= Value,
            Comparison.SmallerOrEqual => value <= Value,
            _ => false,
        };
    }
}

internal enum ValueType : byte
{
    Distance,

    [Description("Hitbox Radius")]
    Hitbox,
}
