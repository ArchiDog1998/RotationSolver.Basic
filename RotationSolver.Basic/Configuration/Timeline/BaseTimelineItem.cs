using ECommons.DalamudServices;
using RotationSolver.Basic.Configuration.TerritoryAction;
using RotationSolver.Basic.Configuration.Timeline.TimelineCondition;
using XIVConfigUI;

namespace RotationSolver.Basic.Configuration.Timeline;

internal abstract class BaseTimelineItem
{
    public TimelineConditionSet Condition { get; set; } = new();

    internal abstract ITerritoryAction TerritoryAction { get;}

    public float Time { get; set; } = 3;
    public float Duration { get; set; } = 3;

    private bool _enable = false;
    internal bool Enable
    {
        get => _enable;
        set
        {
            if (_enable == value) return;
            _enable = value;

            if (_enable)
            {
                if (DownloadHelper.IsSupporter)
                {
                    TerritoryAction.Enable();
                }
                else
                {
                    Svc.Toasts.ShowError(UiString.CantUseTerritoryAction.Local());
                }
            }
            else
            {
                TerritoryAction.Disable();
            }
        }
    }

    public virtual bool InPeriod(TimelineItem item)
    {
        var time = item.Time - DataCenter.RaidTimeRaw;

        if (time > Time) return false;
        if (time < Time - Duration) return false;

        if (!Condition.IsTrue(item)) return false;

        return true;
    }
}
