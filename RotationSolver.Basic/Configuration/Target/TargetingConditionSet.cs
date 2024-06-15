using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Condition Set")]
internal class TargetingConditionSet : TargetingConditionBase
{
    [UI("Conditions")]
    public List<TargetingConditionBase> Conditions { get; set; } = [];

    [UI("Logical Type")]
    public LogicalType Type { get; set; } = LogicalType.All;

    public override bool IsTrue(BattleChara chara)
    {
        if (Conditions.Count == 0) return false;

        return Type switch
        {
            LogicalType.All => Conditions.All(c => c.IsTrue(chara)),
            LogicalType.Any => Conditions.Any(c => c.IsTrue(chara)),
            LogicalType.NotAll => !Conditions.All(c => c.IsTrue(chara)),
            LogicalType.NotAny => !Conditions.Any(c => c.IsTrue(chara)),
            _ => false,
        };
    }
}
