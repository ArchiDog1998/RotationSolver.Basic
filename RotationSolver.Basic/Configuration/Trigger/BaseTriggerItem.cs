using RotationSolver.Basic.Configuration.TerritoryAction;

namespace RotationSolver.Basic.Configuration.Trigger;

internal abstract class BaseTriggerItem
{
    public float StartTime { get; set; }
    public float Duration { get; set; } = 6;
    internal abstract ITerritoryAction TerritoryAction { get; }
}
