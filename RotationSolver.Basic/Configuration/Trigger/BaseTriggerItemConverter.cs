using Newtonsoft.Json.Linq;
using RotationSolver.Basic.Configuration.Condition;

namespace RotationSolver.Basic.Configuration.Trigger;

internal class BaseTriggerItemConverter : JsonCreationConverter<BaseTriggerItem>
{
    protected override BaseTriggerItem? Create(JObject jObject)
    {
        if (FieldExists(nameof(ActionTriggerItem.ActionAction), jObject))
        {
            return new ActionTriggerItem();
        }
        else if (FieldExists(nameof(StateTriggerItem.StateAction), jObject))
        {
            return new StateTriggerItem();
        }
        else if (FieldExists(nameof(DrawingTriggerItem.DrawingAction), jObject))
        {
            return new DrawingTriggerItem();
        }
        else if (FieldExists(nameof(MacroTriggerItem.MacroAction), jObject))
        {
            return new MacroTriggerItem();
        }
        else if (FieldExists(nameof(MoveTriggerItem.MoveAction), jObject))
        {
            return new MoveTriggerItem();
        }
        else if (FieldExists(nameof(PathfindTriggerItem.PathfindAction), jObject))
        {
            return new PathfindTriggerItem();
        }
        return null;
    }
}
