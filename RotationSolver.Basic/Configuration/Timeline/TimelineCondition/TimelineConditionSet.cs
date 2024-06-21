using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Timeline.TimelineCondition;

[Description("Condition Set")]
internal class TimelineConditionSet : TimelineConditionBase
{
    [JsonIgnore]
    public override TimelineItem? TimelineItem 
    { 
        get => base.TimelineItem; 
        set
        {
            base.TimelineItem = value;
            foreach (var item in Conditions)
            {
                if (item == null) continue;
                item.TimelineItem = value;
            }
        }
    }

    [UI("Conditions")]
    public List<TimelineConditionBase?> Conditions { get; set; } = [];

    [UI("Type")]
    public LogicalType Type { get; set; } = LogicalType.All;

    public override bool IsTrue(TimelineItem item)
    {
        return Type.IsTrue(Conditions, c => c.IsTrue(item));
    }
}
