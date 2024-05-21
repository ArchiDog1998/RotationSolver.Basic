using Newtonsoft.Json.Linq;
using RotationSolver.Basic.Configuration.Condition;

namespace RotationSolver.Basic.Configuration.Timeline;

internal class BaseTimelineItemConverter : JsonCreationConverter<BaseTimelineItem>
{
    protected override BaseTimelineItem? Create(JObject jObject)
    {
        if (FieldExists(nameof(ActionTimelineItem.ActionAction), jObject))
        {
            return new ActionTimelineItem();
        }
        else if (FieldExists(nameof(StateTimelineItem.StateAction), jObject))
        {
            return new StateTimelineItem();
        }
        else if (FieldExists(nameof(DrawingTimelineItem.DrawingAction), jObject))
        {
            return new DrawingTimelineItem();
        }
        else if (FieldExists(nameof(MacroTimelineItem.MacroAction), jObject))
        {
            return new MacroTimelineItem();
        }
        else if (FieldExists(nameof(MoveTimelineItem.MoveAction), jObject))
        {
            return new MoveTimelineItem();
        }
        else if (FieldExists(nameof(PathfindTimelineItem.PathfindAction), jObject))
        {
            return new PathfindTimelineItem();
        }
        return null;
    }
}
