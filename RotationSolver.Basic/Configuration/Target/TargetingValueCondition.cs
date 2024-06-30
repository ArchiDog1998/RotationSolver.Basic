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
    public float Percent { get; set; }

    protected override bool IsTrueInside(IGameObject obj)
    {
        if (obj is not IBattleChara battle) return false;

        var value = PercentType switch
        {
             PercentValueType.MP => (float)battle.CurrentMp / battle.MaxMp,
             _ => battle.GetHealthRatio(),
        };

        return Compare.Compare(value, Percent);
    }
}
