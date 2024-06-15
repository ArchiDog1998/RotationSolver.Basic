using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Targeting Item")]
[ListUI(63927)]
internal class TargetingItem
{
    [UI("Target Condition")]
    public TargetingConditionSet ConditionSet { get; set; } = new();

    [UI("Targeting Type")]
    public TargetingType TargetingType { get; set; } = TargetingType.Big;

    public BattleChara? FindTarget(IEnumerable<BattleChara> chara)
    {
        return TargetingType.FindTarget(chara.Where(ConditionSet.IsTrue));
    }
}
