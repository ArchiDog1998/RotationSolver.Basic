using RotationSolver.Basic.Configuration.TerritoryAction;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Trigger;

[Description("Drawing Trigger")]
internal class DrawingTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => DrawingAction;

    [UI("Drawing")]
    public DrawingAction DrawingAction { get; set; } = new();
}

[Description("Action Trigger")]
internal class ActionTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => ActionAction;

    [UI("Action")]
    public ActionAction ActionAction { get; set; } = new();
}

[Description("State Trigger")]
internal class StateTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => StateAction;

    [UI("State")]
    public StateAction StateAction { get; set; } = new();
}

[Description("Path find Trigger")]
internal class PathfindTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => PathfindAction;

    [UI("Pathfind")]
    public PathfindAction PathfindAction { get; set; } = new();
}

[Description("Move Trigger")]
internal class MoveTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => MoveAction;

    [UI("Move")]
    public MoveAction MoveAction { get; set; } = new();
}

[Description("Macro Trigger")]
internal class MacroTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => MacroAction;

    [UI("Macro")]
    public MacroAction MacroAction { get; set; } = new();
}


[Description("Notice Trigger")]
internal class NoticeTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => NoticeAction;

    [UI("Notice")]
    public NoticeAction NoticeAction { get; set; } = new();
}