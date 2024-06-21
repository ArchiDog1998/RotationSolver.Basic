using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Condition;

[Description("Condition Set")]
internal class ConditionSet : DelayConditionBase
{
    [UI("Conditions")]
    public List<DelayConditionBase?> Conditions { get; set; } = [];

    [UI("Type")]
    public LogicalType Type { get; set; } = LogicalType.All;

    protected override bool IsTrueInside(ICustomRotation rotation)
    {
        return Type.IsTrue(Conditions, c => c.IsTrue());
    }
}