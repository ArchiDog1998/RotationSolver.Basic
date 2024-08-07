namespace RotationSolver.Basic.Data;

/// <summary>
/// The Type of description.
/// </summary>
public enum DescType : byte
{
    /// <summary/>
    None,

    /// <summary/>
    [Description("Burst Actions")]
    BurstActions,

    /// <summary/>
    [Description("Heal Area GCD")]
    HealAreaGCD,

    /// <summary/>
    [Description("Heal Area Ability")]
    HealAreaAbility,

    /// <summary/>
    [Description("Heal Single GCD")]
    HealSingleGCD,

    /// <summary/>
    [Description("Heal Single Ability")]
    HealSingleAbility,

    /// <summary/>
    [Description("Defense Area GCD")]
    DefenseAreaGCD,

    /// <summary/>
    [Description("Defense Area Ability")]
    DefenseAreaAbility,

    /// <summary/>
    [Description("Defense Single GCD")]
    DefenseSingleGCD,

    /// <summary/>
    [Description("Defense Single Ability")]
    DefenseSingleAbility,

    /// <summary/>
    [Description("Move Forward GCD")]
    MoveForwardGCD,

    /// <summary/>
    [Description("Move Forward Ability")]
    MoveForwardAbility,

    /// <summary/>
    [Description("Move Back Ability")]
    MoveBackAbility,

    /// <summary/>
    [Description("Speed Ability")]
    SpeedAbility,
}
