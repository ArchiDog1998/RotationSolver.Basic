using ECommons.DalamudServices;
using RotationSolver.Basic.Configuration.Target;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Timeline.TimelineCondition;

[Description("Target Count Condition")]
internal class TimelineConditionTargetCount : TimelineConditionBase
{
    [UI("Target Condition")]
    public TargetingConditionSet TargetConditions { get; set; } = new();

    [UI("Comparison")]
    public Comparison Comparison { get; set; } = Comparison.BiggerOrEqual;

    [UI("Count")]
    public int Count { get; set; }

    public override bool IsTrue(TimelineItem item)
    {
        var count = Svc.Objects.Count(TargetConditions.IsTrue);
        return Comparison switch
        {
            Comparison.Bigger => count > Count,
            Comparison.Smaller => count < Count,
            Comparison.Equal => count == Count,
            Comparison.SmallerOrEqual => count <= Count,
            _ => count >= Count,
        };
    }
}
