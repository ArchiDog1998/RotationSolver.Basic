﻿using RotationSolver.Basic.Configuration.Condition;

namespace RotationSolver.Basic.Configuration.Timeline.TimelineCondition;

[Description("Condition Set")]
internal class TimelineConditionSet : ITimelineCondition
{
    public List<ITimelineCondition> Conditions { get; set; } = [];

    public LogicalType Type;
    public bool IsTrue(TimelineItem item)
    {
        if (Conditions.Count == 0) return true;

        return Type switch
        {
            LogicalType.All => Conditions.All(c => c.IsTrue(item)),
            LogicalType.Any => Conditions.Any(c => c.IsTrue(item)),
            _ => false,
        };
    }
}
