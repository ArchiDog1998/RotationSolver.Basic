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
    public List<TimelineConditionBase> Conditions { get; set; } = [];

    [UI("Type")]
    public LogicalType Type { get; set; } = LogicalType.All;

    public override bool IsTrue(TimelineItem item)
    {
        if (Conditions.Count == 0) return true;

        return Type switch
        {
            LogicalType.All => Conditions.All(c => c.IsTrue(item)),
            LogicalType.Any => Conditions.Any(c => c.IsTrue(item)),
            LogicalType.NotAll => !Conditions.All(c => c.IsTrue(item)),
            LogicalType.NotAny => !Conditions.Any(c => c.IsTrue(item)),
            _ => false,
        };
    }
}
