using ECommons.DalamudServices;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.TerritoryAction;

internal class StateAction : ITerritoryAction
{
    [UI("State")]
    public SpecialCommandType State { get; set; } = SpecialCommandType.DefenseArea;

    public void Disable()
    {
    }

    public void Enable()
    {
        DataCenter.SpecialType = State;
#if DEBUG
        Svc.Log.Debug($"Added the state {State} to timeline.");
#endif 
    }
}
