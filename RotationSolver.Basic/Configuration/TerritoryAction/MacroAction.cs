using ECommons.Automation;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.TerritoryAction;
internal class MacroAction : ITerritoryAction
{
    [UIType(UiType.Multiline)]
    [UI("Macro")]
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
