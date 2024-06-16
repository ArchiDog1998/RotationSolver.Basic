using ECommons.Schedulers;
using RotationSolver.Basic.Configuration.TerritoryAction;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Trigger;

internal class TriggerAttribute : ListUIAttribute
{
    public TriggerAttribute() : base(12)
    {
        Description = "Click to show the action.";
    }

    public override void OnClick(object obj)
    {
        if (obj is not BaseTriggerItem item) return;
        Task.Run(async () =>
        {
            item.TerritoryAction.Enable();
            await Task.Delay(3000);
            item.TerritoryAction.Disable();
        });
    }
}

[Trigger, Description("Trigger Item")]
internal abstract class BaseTriggerItem
{
    [Range(0, 0, ConfigUnitType.Seconds)]
    [UI("Time")]

    public float StartTime { get; set; }

    [Range(0, 0, ConfigUnitType.Seconds)]
    [UI("Duration")]
    public float Duration { get; set; } = 6;
    internal abstract ITerritoryAction TerritoryAction { get; }

    public void Invoke()
    {
        _ = new TickScheduler(TerritoryAction.Enable, (long)(StartTime * 1000));
        _ = new TickScheduler(TerritoryAction.Disable, (long)((StartTime + Duration) * 1000));
    }
}
