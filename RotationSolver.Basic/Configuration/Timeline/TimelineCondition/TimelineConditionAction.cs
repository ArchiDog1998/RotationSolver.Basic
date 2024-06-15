using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Timeline.TimelineCondition;

[Description("Action Condition")]
internal class TimelineConditionAction : TimelineConditionBase
{
    [UI("Action ID")]
    public ActionID ActionID { get; set; }

    public override bool IsTrue(TimelineItem item)
    {
        return (uint)ActionID == item.LastActionID;
    }
}
