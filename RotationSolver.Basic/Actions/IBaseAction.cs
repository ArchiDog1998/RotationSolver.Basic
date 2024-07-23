using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.Basic.Actions;

/// <summary>
/// The interface of the base action.
/// </summary>
public interface IBaseAction : ICanUse, IAction
{
    internal static TargetType? TargetOverride { get; set; } = null;
    internal static bool ForceEnable { get; set; } = false;
    internal static bool AutoHealCheck { get; set; } = false;
    internal static bool ActionPreview { get; set; } = false;
    internal static bool IgnoreClipping { get; set; } = false;
    internal static bool AllEmpty { get; set; } = false;
    internal static bool ShouldEndSpecial { get; set; } = false;

    /// <summary>
    /// The action itself.
    /// </summary>
    Action Action { get; }

    /// <summary>
    /// The information about the target.
    /// </summary>
    ActionTargetInfo TargetInfo { get; }

    /// <summary>
    /// The target to use on.
    /// </summary>
    TargetResult Target { get; set; }

    /// <summary>
    /// The target for preview.
    /// </summary>
    TargetResult? PreviewTarget { get; }

    /// <summary>
    /// The basic information of this action.
    /// </summary>
    ActionBasicInfo Info { get; }
    WhyActionCantUse WhyCant { get; }

    /// <summary>
    /// The cd information.
    /// </summary>
    new ActionCooldownInfo CD { get; }

    /// <summary>
    /// The setting to use this action.
    /// </summary>
    ActionSetting Setting { get; set; }
    internal ActionConfig Config { get; }
}
