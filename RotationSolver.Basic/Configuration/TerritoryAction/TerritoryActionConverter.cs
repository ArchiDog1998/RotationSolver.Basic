using Newtonsoft.Json.Linq;
using RotationSolver.Basic.Configuration.Condition;

namespace RotationSolver.Basic.Configuration.TerritoryAction;
internal class TerritoryActionConverter : JsonCreationConverter<ITerritoryAction>
{
    protected override ITerritoryAction? Create(JObject jObject)
    {
        if (FieldExists(nameof(ActionAction.ID), jObject))
        {
            return new ActionAction();
        }
        else if (FieldExists(nameof(StateAction.State), jObject))
        {
            return new StateAction();
        }
        else if (FieldExists(nameof(DrawingAction.DrawingGetters), jObject))
        {
            return new DrawingAction();
        }
        else if (FieldExists(nameof(MacroAction.Macro), jObject))
        {
            return new MacroAction();
        }
        else if (FieldExists(nameof(MoveAction.Points), jObject))
        {
            return new MoveAction();
        }
        else if (FieldExists(nameof(PathfindAction.Destination), jObject))
        {
            return new PathfindAction();
        }
        return null;
    }
}
