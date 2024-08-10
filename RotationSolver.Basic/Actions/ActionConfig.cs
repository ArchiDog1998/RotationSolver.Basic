using RotationSolver.Basic.Configuration.Target;

namespace RotationSolver.Basic.Actions;

/// <summary>
/// User config.
/// </summary>
public class ActionConfig
{
    internal TargetingConditionSet PriorityTargeting { get; set; } = new();
    internal TargetingConditionSet CantTargeting { get; set; } = new();

    private bool _isEnable = true;

    /// <summary>
    /// If this action is enabled.
    /// </summary>
    public bool IsEnabled 
    {
        get => IBaseAction.ForceEnable || _isEnable;
        set => _isEnable = value;
    }

    internal bool UseCustomTargetingData { get; set; } = false;

    internal string TargetingDataName { get; set; } = string.Empty;

    /// <summary>
    /// Should check the status for this action.
    /// </summary>
    public bool ShouldCheckStatus { get; internal set; } = true;

    /// <summary>
    /// The status count in gcd for adding the status.
    /// </summary>
    public byte StatusGcdCount { get; internal set; } = 2;

    /// <summary>
    /// The aoe count of this action.
    /// </summary>
    public byte AoeCount { get; internal set; } = 3;

    /// <summary>
    /// How many ttk should this action use.
    /// </summary>
    public float TimeToKill { get; internal set; } = 0;

    /// <summary>
    /// How many ttu should this action use.
    /// </summary>
    public float TimeToUntargetable { get; internal set; } = 0;

    /// <summary>
    /// The heal ratio for the auto heal.
    /// </summary>
    public float AutoHealRatio { get; internal set; } = 0.8f;

    /// <summary>
    /// Is this action in the cd window.
    /// </summary>
    public bool IsInCooldown { get; internal set; } = true;

    /// <summary>
    /// Is this action should be a mistake action.
    /// </summary>
    public bool IsInMistake { get; internal set; }

    internal ActionConfig()
    {
        
    }

    internal bool IsTopPriority(IGameObject obj)
    {
        if (PriorityTargeting.IsTrue(obj) ?? false) return true;
        if (obj.IsTopPriority()) return true;
        return false;
    }

    internal bool CantAttack(IGameObject obj)
    {
        if (CantTargeting.IsTrue(obj) ?? false) return true;
        if (obj.IsNoTarget()) return true;
        return false;
    }
}
