using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration;
#pragma warning disable CS1591 // Missing XML comment for publicly visible
public class MacroInfo
{
    [Range(-1, 99, ConfigUnitType.None)]
    [UI("Macro No.")]
    public int MacroIndex { get; set; } = -1;

    [UI("Is Shared")]
    public bool IsShared { get; set; }

    public unsafe bool AddMacro(IGameObject? tar = null)
    {
        if (MacroIndex < 0 || MacroIndex > 99) return false;

        try
        {
            var macro = RaptureMacroModule.Instance()->GetMacro(IsShared ? 1u : 0u, (uint)MacroIndex);

            DataCenter.Macros.Enqueue(new MacroItem(tar, macro));
            return true;
        }
        catch (Exception ex)
        {
            Svc.Log.Warning(ex, "Failed to add macro.");
            return false;
        }
    }
}