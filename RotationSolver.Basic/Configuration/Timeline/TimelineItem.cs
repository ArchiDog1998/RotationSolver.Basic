﻿using RotationSolver.Basic.Configuration.TerritoryAction;

namespace RotationSolver.Basic.Configuration.Timeline;

[Description("Drawing Timeline")]
internal class DrawingTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => DrawingAction;
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
    public ActionAction ActionAction { get; set; } = new();
}

[Description("State Timeline")]
internal class StateTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => StateAction;
    public StateAction StateAction { get; set; } = new();
}

[Description("Path find Timeline")]
internal class PathfindTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => PathfindAction;
    public PathfindAction PathfindAction { get; set; } = new();
}

[Description("Move Timeline")]
internal class MoveTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => MoveAction;
    public MoveAction MoveAction { get; set; } = new();
}

[Description("Macro Timeline")]
internal class MacroTimelineItem : BaseTimelineItem
{
    internal override ITerritoryAction TerritoryAction => MacroAction;
    public MacroAction MacroAction { get; set; } = new();
}