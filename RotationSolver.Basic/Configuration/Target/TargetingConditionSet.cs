using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Condition Set")]
internal class TargetingConditionSet : TargetingConditionBase
{
    [UI("Conditions")]
    public List<TargetingConditionBase?> Conditions { get; set; } = [];

    [UI("Logical Type")]
    public LogicalType Type { get; set; } = LogicalType.All;

    protected override bool IsTrueInside(GameObject obj)
    {
        return Type.IsTrue(Conditions, c => c?.IsTrue(obj) ?? false);
    }
}
