using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Value Condition")]
internal class TargetingValueCondition : TargetingConditionBase
{
    internal enum PercentValueType : byte
    {
        MP,
        HP,
    }

    [UI("Type")]
    public PercentValueType PercentType { get; set; } = PercentValueType.MP;

    [UI("Comparison")]
    public Comparison Compare { get; set; } = Comparison.Bigger;

    [Range(0, 1, ConfigUnitType.Percent)]
    [UI("Percent")]
    public float Precent { get; set; }

    protected override bool IsTrueInside(GameObject obj)
    {
        if (obj is not BattleChara battle) return false;

        var value = PercentType switch
        {
             PercentValueType.MP => (float)battle.CurrentMp / battle.MaxMp,
             _ => battle.GetHealthRatio(),
        };

        return Compare switch
        {
            Comparison.Bigger => value > Precent,
            Comparison.Smaller => value < Precent,
            Comparison.Equal => value == Precent,
            Comparison.BiggerOrEqual => value >= Precent,
            Comparison.SmallerOrEqual => value <= Precent,
            _ => false,
        };
    }
}
