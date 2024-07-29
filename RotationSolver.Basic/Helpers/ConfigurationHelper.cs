using Dalamud.Game.ClientState.Keys;

namespace RotationSolver.Basic.Helpers;

internal static class ConfigurationHelper
{
    public static readonly uint[] BadStatus =
    [
        583, //No items.
        581, //Unable to use.
        579, //Between Area
        574, //Job
        573, //Not learned
    ];

    public static VirtualKey ToVirtual(this ConsoleModifiers modifiers)
    {
        return modifiers switch
        {
            ConsoleModifiers.Alt => VirtualKey.MENU,
            ConsoleModifiers.Shift => VirtualKey.SHIFT,
            _ => VirtualKey.CONTROL,
        };
    }
}