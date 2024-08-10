namespace RotationSolver.Basic.Actions;

internal enum SpecialActionType : byte
{
    None,
    MeleeRange,
    MovingForward,
}

/// <summary>
/// Setting from the developer.
/// </summary>
public class ActionSetting()
{
    /// <summary>
    /// The override of the <see cref="ActionBasicInfo.MPNeed"/>.
    /// </summary>
    public Func<uint?>? MPOverride { get; internal set; } = null;

    /// <summary>
    /// Is this action in the melee range.
    /// </summary>
    internal SpecialActionType SpecialType { get; set; }

    /// <summary>
    /// Is this status is added by the plyer.
    /// </summary>
    public bool StatusFromSelf { get; internal set; } = true;
    
    /// <summary>
    /// You can't use this action when the target has this status.
    /// </summary>
    public StatusID[]? TargetStatusPenalty { get; internal set; } = null;

    /// <summary>
    /// The status that it needs on the target.
    /// </summary>
    public StatusID[]? TargetStatusNeed { get; internal set; } = null;

    /// <summary>
    /// Can the target be targeted.
    /// </summary>
    public Func<IBattleChara, bool> CanTarget { get; internal set; } = t => true;

    /// <summary>
    /// Skip the combo check
    /// </summary>
    public bool SkipComboCheck { get; internal set; } = false;
    /// <summary>
    /// The additional combo ids.
    /// </summary>
    public ActionID[]? ComboIds { get; internal set; }

    /// <summary>
    /// The additional not combo ids.
    /// </summary>
    public ActionID[]? ComboIdsNot { get; internal set; }

    /// <summary>
    /// You can't use this action when the player has this status.
    /// </summary>
    public StatusID[]? StatusPenalty { get; internal set; } = null;

    /// <summary>
    /// Status that this action needs.
    /// </summary>
    public StatusID[]? StatusNeed { get; internal set; } = null;

    internal Func<bool>? ActionCheck { get; set; } = null;

    internal Func<ActionConfig>? CreateConfig { get; set; } = null;

    /// <summary>
    /// Is this action friendly.
    /// </summary>
    public bool IsFriendly { get; internal set; }

    private TargetType _type = TargetType.None;

    /// <summary>
    /// The strategy to target the target.
    /// </summary>
    public TargetType TargetType 
    {
        get
        {
            var type = IBaseAction.TargetOverride ?? _type;
            if (IsFriendly)
            {

            }
            else
            {
                switch (type)
                {
                    case TargetType.BeAttacked:
                        return _type;
                }
            }

            return type;
        }
        internal set => _type = value; 
    }

    /// <summary>
    /// The enemy positional for this action.
    /// </summary>
    public EnemyPositional EnemyPositional { get; internal set; } = EnemyPositional.None;

    /// <summary>
    /// Should end the special.
    /// </summary>
    public bool EndSpecial { get; internal set; }

    /// <summary>
    /// This action needs highlight, maybe is a combo.
    /// </summary>
    public bool NeedsHighlight { get; internal set; }

    internal StatusID[]? StatusProvide { get; set; } = null;

    internal StatusID[]? TargetStatusProvide { get; set; } = null;
}
