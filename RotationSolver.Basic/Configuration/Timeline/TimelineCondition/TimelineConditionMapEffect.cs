using RotationSolver.Basic.Record;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Timeline.TimelineCondition;

[Description("Map Effect Condition")]
internal class TimelineConditionMapEffect : TimelineConditionBase
{
    [Range(0, 0, ConfigUnitType.Seconds)]
    [UI("Duration")]
    public Vector2 TimeDuration { get; set; } = new(0, 2);

    [UI("Position")]
    public int Position { get; set; }

    [UI("Param1")]
    public int Param1 { get; set; }

    [UI("Param2")]
    public int Param2 { get; set; }

    public override bool IsTrue(TimelineItem item)
    {
        return Recorder.GetData<MapEffectData>(TimeDuration).Any(effect =>
        {
            if (effect.Position != Position) return false;
            if (effect.Param1 != Param1) return false;
            if (effect.Param2 != Param2) return false;

            return true;
        });
    }
}
