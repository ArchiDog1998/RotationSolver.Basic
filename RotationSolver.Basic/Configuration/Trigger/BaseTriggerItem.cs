using ECommons.Schedulers;
using RotationSolver.Basic.Configuration.TerritoryAction;

namespace RotationSolver.Basic.Configuration.Trigger;

internal abstract class BaseTriggerItem
{
    public float StartTime { get; set; }
    public float Duration { get; set; } = 6;
    internal abstract ITerritoryAction TerritoryAction { get; }

    public void Invoke()
    {
        _ = new TickScheduler(TerritoryAction.Enable, (long)(StartTime * 1000));
        _ = new TickScheduler(TerritoryAction.Disable, (long)((StartTime + Duration) * 1000));
    }
}
