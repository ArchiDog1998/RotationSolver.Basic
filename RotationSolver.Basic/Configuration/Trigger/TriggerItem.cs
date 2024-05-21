using RotationSolver.Basic.Configuration.TerritoryAction;

namespace RotationSolver.Basic.Configuration.Trigger;

[Description("Drawing Trigger")]
internal class DrawingTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => DrawingAction;
    public DrawingAction DrawingAction { get; set; } = new();
}

[Description("Action Trigger")]
internal class ActionTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => ActionAction;
    public ActionAction ActionAction { get; set; } = new();
}

[Description("State Trigger")]
internal class StateTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => StateAction;
    public StateAction StateAction { get; set; } = new();
}

[Description("Path find Trigger")]
internal class PathfindTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => PathfindAction;
    public PathfindAction PathfindAction { get; set; } = new();
}

[Description("Move Trigger")]
internal class MoveTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => MoveAction;
    public MoveAction MoveAction { get; set; } = new();
}

[Description("Macro Trigger")]
internal class MacroTriggerItem : BaseTriggerItem
{
    internal override ITerritoryAction TerritoryAction => MacroAction;
    public MacroAction MacroAction { get; set; } = new();
}