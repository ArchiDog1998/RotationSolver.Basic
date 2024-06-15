using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Target Condition")]
internal class TargetingCondition : ITargetingCondition
{
    [UI("Value Type")]
    public ValueType Type { get; set; } = ValueType.Distance;

    [UI("Comparison")]
    public Comparison Compare { get; set; } = Comparison.Bigger;

    [UI("Value")]
    public float Value { get; set; } = 1;

    bool ITargetingCondition.IsTrue(BattleChara chara)
    {
        var value = Type switch
        {
            ValueType.Hitbox => chara.HitboxRadius,
            _ => chara.DistanceToPlayer(),
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
    Hitbox,
}
