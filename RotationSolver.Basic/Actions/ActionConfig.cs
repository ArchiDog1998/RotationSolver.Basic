using RotationSolver.Basic.Configuration.Target;

namespace RotationSolver.Basic.Actions;

/// <summary>
/// User config.
/// </summary>
internal class ActionConfig()
{
    public TargetingConditionSet PriorityTargeting { get; set; } = new();
    public TargetingConditionSet CantTargeting { get; set; } = new();

    private bool _isEnable = true;

    /// <summary>
    /// If this action is enabled.
    /// </summary>
    public bool IsEnabled 
    {
        get => IBaseAction.ForceEnable || _isEnable;
        set => _isEnable = value;
    }

    /// <summary>
    /// Should check the status for this action.
    /// </summary>
    public bool ShouldCheckStatus { get; set; } = true;

    /// <summary>
    /// The status count in gcd for adding the status.
    /// </summary>
    public byte StatusGcdCount { get; set; } = 2;

    /// <summary>
    /// The aoe count of this action.
    /// </summary>
    public byte AoeCount { get; set; } = 3;

    /// <summary>
    /// How many ttk should this action use.
    /// </summary>
    public float TimeToKill { get; set; } = 0;

    /// <summary>
    /// How many ttu should this action use.
    /// </summary>
    public float TimeToUntargetable { get; set; } = 0;

    /// <summary>
    /// The heal ratio for the auto heal.
    /// </summary>
    public float AutoHealRatio { get; set; } = 0.8f;

    /// <summary>
    /// Is this action in the cd window.
    /// </summary>
    public bool IsInCooldown { get; set; } = true;

    /// <summary>
    /// Is this action should be a mistake action.
    /// </summary>
    public bool IsInMistake { get; set; }

    public bool IsTopPriority(GameObject obj)
    {
        if (PriorityTargeting.IsTrue(obj)) return true;
        if (ObjectHelper.IsTopPriority(obj)) return true;
        return false;
    }

    public bool CantAttack(GameObject obj)
    {
        if (CantTargeting.IsTrue(obj)) return true;
        if (Service.Config.CantTargeting.IsTrue(obj)) return true;
        return false;
    }
}
