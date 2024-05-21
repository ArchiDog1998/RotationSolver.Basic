using ECommons.Automation;

namespace RotationSolver.Basic.Configuration.TerritoryAction;
internal class MacroAction : ITerritoryAction
{
    public string Macro { get; set; } = "";

    public void Enable()
    {
        if (!string.IsNullOrEmpty(Macro))
        {
            Chat.Instance.SendMessage(Macro);
        }
    }

    public void Disable()
    {
    }
}
