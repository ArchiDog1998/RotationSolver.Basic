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
    [Description("Interrupt")]
    Interrupt = 1 << 0,

    /// <summary>
    /// Shall we use tank stance.
    /// </summary>
    [Description("Tank Stance")]
    TankStance = 1 << 1,

    /// <summary>
    /// Shall we provoke some enemy.
    /// </summary>
    [Description("Provoke")]
    Provoke = 1 << 2,

    /// <summary>
    /// Shall we defense single.
    /// </summary>
    [Description("Defense Single")]
    DefenseSingle = 1 << 3,

    /// <summary>
    /// Shall we defense are.
    /// </summary>
    [Description("Defense Area")]
    DefenseArea = 1 << 4,

    /// <summary>
    /// Shall we heal single by ability.
    /// </summary>
    [Description("Heal Single Ability")]
    HealSingleAbility = 1 << 5,

    /// <summary>
    /// Shall we heal single by spell.
    /// </summary>
    [Description("Heal Single Spell")]
    HealSingleSpell = 1 << 6,

    /// <summary>
    /// Shall we heal area by ability.
    /// </summary>
    [Description("Heal Area Ability")]
    HealAreaAbility = 1 << 7,

    /// <summary>
    /// Shall we heal area by spell.
    /// </summary>
    [Description("Heal Area Spell")]
    HealAreaSpell = 1 << 8,

    /// <summary>
    /// Shall we raise.
    /// </summary>
    [Description("Raise")]
    Raise = 1 << 9,

    /// <summary>
    /// Shall we Dispel.
    /// </summary>
    [Description("Dispel")]
    Dispel = 1 << 10,

    /// <summary>
    /// 
    /// </summary>
    [Description("Positional")]
    Positional = 1 << 11,

    /// <summary>
    /// 
    /// </summary>
    [Description("Shirk")]
    Shirk = 1 << 12,

    /// <summary>
    /// 
    /// </summary>
    [Description("Move Forward")]
    MoveForward = 1 << 13,

    /// <summary>
    /// 
    /// </summary>
    [Description("Move Back")]
    MoveBack = 1 << 14,

    /// <summary>
    /// 
    /// </summary>
    [Description("Anti-Knockback")]
    AntiKnockback = 1 << 15,

    /// <summary>
    /// 
    /// </summary>
    [Description("Burst")]
    Burst = 1 << 16,

    /// <summary>
    /// 
    /// </summary>
    [Description("Speed")]
    Speed = 1 << 17,

    /// <summary>
    /// 
    /// </summary>
    [Description("Limit Break")]
    LimitBreak = 1 << 18,
}
