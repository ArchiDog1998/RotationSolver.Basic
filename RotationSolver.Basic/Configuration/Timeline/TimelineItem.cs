using RotationSolver.Basic.Configuration.TerritoryAction;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Timeline;

[Description("Drawing Timeline")]
internal class DrawingTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => DrawingAction;

    [UI("Drawing")]
    public DrawingAction DrawingAction { get; set; } = new();

    public DrawingTimelineItem()
    {
        Time = Duration = 6;
    }
}

[Description("Action Timeline")]
internal class ActionTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => ActionAction;

    [UI("Action")]
    public ActionAction ActionAction { get; set; } = new();
}

[Description("State Timeline")]
internal class StateTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => StateAction;

    [UI("State")]
    public StateAction StateAction { get; set; } = new();
}

[Description("Path find Timeline")]
internal class PathfindTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => PathfindAction;

    [UI("Pathfind")]
    public PathfindAction PathfindAction { get; set; } = new();
}

[Description("Move Timeline")]
internal class MoveTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => MoveAction;

    [UI("Move")]
    public MoveAction MoveAction { get; set; } = new();
}

[Description("Macro Timeline")]
internal class MacroTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => MacroAction;

    [UI("Macro")]
    public MacroAction MacroAction { get; set; } = new();
}

[Description("Notice Timeline")]
internal class NoticeTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => NoticeAction;

    [UI("Notice")]
    public NoticeAction NoticeAction { get; set; } = new();
}