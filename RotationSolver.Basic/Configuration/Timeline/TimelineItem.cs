using RotationSolver.Basic.Configuration.TerritoryAction;

namespace RotationSolver.Basic.Configuration.Timeline;

[Description("Drawing Timeline")]
internal class DrawingTimeline : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => DrawingAction;

    public DrawingAction DrawingAction { get; set; } = new();

    public DrawingTimeline()
    {
        Time = 6;
    }

    public override bool InPeriod(TimelineItem item)
    {
        var time = item.Time - DataCenter.RaidTimeRaw;

        if (time < Time - DrawingAction.Duration) return false;
        if (time > Time) return false;

        if (!Condition.IsTrue(item)) return false;

        return true;
    }
}


[Description("Action Timeline")]
internal class ActionTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => ActionAction;

    public ActionAction ActionAction { get; set; } = new();
}

[Description("State Timeline")]
internal class StateTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => StateAction;

    public StateAction StateAction { get; set; } = new();
}

[Description("Path find Time line")]
internal class PathfindTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => PathfindAction;

    public PathfindAction PathfindAction { get; set; } = new();
}

[Description("Move Time line")]
internal class MoveTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => MoveAction;
    public MoveAction MoveAction { get; set; } = new();
}

[Description("Macro Time line")]
internal class MacroTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => MacroAction;

    public MacroAction MacroAction { get; set; } = new();
}
