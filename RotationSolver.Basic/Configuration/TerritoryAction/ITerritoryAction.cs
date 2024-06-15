using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.TerritoryAction;

[ListUI(66301)]
internal interface ITerritoryAction
{
    void Enable();

    void Disable();
}
