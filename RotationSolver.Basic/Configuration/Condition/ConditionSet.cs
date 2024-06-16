using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Condition;

[Description("Condition Set")]
internal class ConditionSet : DelayConditionBase
{
    [UI("Conditions")]
    public List<DelayConditionBase> Conditions { get; set; } = [];

    [UI("Type")]
    public LogicalType Type { get; set; } = LogicalType.All;

    protected override bool IsTrueInside(ICustomRotation rotation)
    {
        if (Conditions.Count == 0) return false;

        return Type switch
        {
            LogicalType.All => Conditions.All(c => c.IsTrue() ?? false),
            LogicalType.Any => Conditions.Any(c => c.IsTrue() ?? false),
            LogicalType.NotAll => !Conditions.All(c => c.IsTrue() ?? false),
            LogicalType.NotAny => !Conditions.Any(c => c.IsTrue() ?? false),
            _ => false,
        };
    }
}