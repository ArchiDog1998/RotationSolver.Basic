namespace RotationSolver.Basic.Data;

/// <summary>
/// The status of auto.
/// </summary>
[Flags]
public enum AutoStatus : uint
{
    /// <summary>
    /// Nothing.
    /// </summary>
    None = 0,

    /// <summary>
    /// Shall we interrupt.
    /// </summary>
    [Icon(808)]
    [Description("Interrupt")]
    Interrupt = 1 << 0,

    /// <summary>
    /// Shall we use tank stance.
    /// </summary>
    [Icon(72227)]
    [Description("Tank Stance")]
    TankStance = 1 << 1,

    /// <summary>
    /// Shall we provoke some enemy.
    /// </summary>
    [Icon(803)]
    [Description("Provoke")]
    Provoke = 1 << 2,

    /// <summary>
    /// Shall we defense single.
    /// </summary>
    [Icon(114007)]
    [Description("Defense Single")]
    DefenseSingle = 1 << 3,

    /// <summary>
    /// Shall we defense are.
    /// </summary>
    [Icon(114057)]
    [Description("Defense Area")]
    DefenseArea = 1 << 4,

    /// <summary>
    /// Shall we heal single by ability.
    /// </summary>
    [Icon(64868)]
    [Description("Heal Single Ability")]
    HealSingleAbility = 1 << 5,

    /// <summary>
    /// Shall we heal single by spell.
    /// </summary>
    [Icon(114008)]
    [Description("Heal Single Spell")]
    HealSingleSpell = 1 << 6,

    /// <summary>
    /// Shall we heal area by ability.
    /// </summary>
    [Icon(64859)]
    [Description("Heal Area Ability")]
    HealAreaAbility = 1 << 7,

    /// <summary>
    /// Shall we heal area by spell.
    /// </summary>
    [Icon(114058)]
    [Description("Heal Area Spell")]
    HealAreaSpell = 1 << 8,

    /// <summary>
    /// Shall we raise.
    /// </summary>
    [Icon(72273)]
    [Description("Raise")]
    Raise = 1 << 9,

    /// <summary>
    /// Shall we Dispel.
    /// </summary>
    [Icon(463)]
    [Description("Dispel")]
    Dispel = 1 << 10,

    /// <summary>
    /// 
    /// </summary>
    [Icon(16720)]
    [Description("Positional")]
    Positional = 1 << 11,

    /// <summary>
    /// 
    /// </summary>
    [Icon(791)]
    [Description("Shirk")]
    Shirk = 1 << 12,

    /// <summary>
    /// 
    /// </summary>
    [Icon(113)]
    [Description("Move Forward")]
    MoveForward = 1 << 13,

    /// <summary>
    /// 
    /// </summary>
    [Icon(114)]
    [Description("Move Back")]
    MoveBack = 1 << 14,

    /// <summary>
    /// 
    /// </summary>
    [Icon(2574)]
    [Description("Anti-Knockback")]
    AntiKnockback = 1 << 15,

    /// <summary>
    /// 
    /// </summary>
    [Icon(2598)]
    [Description("Burst")]
    Burst = 1 << 16,

    /// <summary>
    /// 
    /// </summary>
    [Icon(104)]
    [Description("Speed")]
    Speed = 1 << 17,

    /// <summary>
    /// 
    /// </summary>
    [Icon(103)]
    [Description("Limit Break")]
    LimitBreak = 1 << 18,
}
