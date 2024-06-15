using XIVConfigUI.Attributes;
using XIVConfigUI.ConditionConfigs;

namespace RotationSolver.Basic.Configuration.Timeline.TimelineCondition;

[Description("Timeline Condition")]
[ListUI(0)]
internal abstract class TimelineConditionBase : ICondition
{
    [JsonIgnore]
    public virtual TimelineItem? TimelineItem { get; set; }

    [JsonIgnore]
    public bool? State
    {
        get
        {
            if (TimelineItem == null) return null;
            return IsTrue(TimelineItem);
        }
    }
    public abstract bool IsTrue(TimelineItem item);
}
