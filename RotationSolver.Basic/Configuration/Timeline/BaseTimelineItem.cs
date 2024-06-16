using ECommons.DalamudServices;
using RotationSolver.Basic.Configuration.Drawing;
using RotationSolver.Basic.Configuration.TerritoryAction;
using RotationSolver.Basic.Configuration.Timeline.TimelineCondition;
using XIVConfigUI;
using XIVConfigUI.Attributes;
using XIVConfigUI.ConditionConfigs;

namespace RotationSolver.Basic.Configuration.Timeline;

internal class TimelineAttribute : ListUIAttribute
{
    public TimelineAttribute() : base(0)
    {
        Description = "The icon means if it is in period.\nClick to show the action.";
    }

    public override void OnClick(object obj)
    {
        if (obj is not BaseTimelineItem item) return;
        Task.Run(async () =>
        {
            item.TerritoryAction.Enable();
            await Task.Delay(3000);
            item.TerritoryAction.Disable();
        });
    }
}

[Timeline, Description("Timeline Item")]
internal abstract class BaseTimelineItem : ICondition
{
    [Range(0, 0, ConfigUnitType.Seconds)]
    [UI("Time")]
    public float Time { get; set; } = 3;

    [Range(0, 0, ConfigUnitType.Seconds)]
    [UI("Duration")]
    public float Duration { get; set; } = 3;

    [UI("Condition")]
    public TimelineConditionSet Condition { get; set; } = new();

    internal abstract ITerritoryAction TerritoryAction { get; }

    private bool _enable = false;

    [JsonIgnore]
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

    private TimelineItem? _timelineItem;
    [JsonIgnore]
    public TimelineItem? TimelineItem 
    {
        get => _timelineItem;
        set
        {
            Condition.TimelineItem = _timelineItem = value;

            if (TerritoryAction is DrawingAction drawingAction)
            {
                foreach (var item in drawingAction.DrawingGetters)
                {
                    if (item is not ActionDrawingGetter actionDrawer) continue;
                    actionDrawer.TimelineItem = value;
                }
            }
        }
    }

    [JsonIgnore]
    public bool? State
    {
        get
        {
            if (TimelineItem == null) return null;
            return InPeriod(TimelineItem);
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
