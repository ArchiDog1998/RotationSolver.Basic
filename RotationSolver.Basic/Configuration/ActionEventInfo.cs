using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration;

/// <summary>
/// To save the special actions.
/// </summary>
[ListUI(60552)]
public class ActionEventInfo : MacroInfo
{
    /// <summary>
    /// Action Name.
    /// </summary>
    [UI("Action Name", Description ="You can do it in regex.")]
    public string Name { get; set; } = string.Empty;
}
