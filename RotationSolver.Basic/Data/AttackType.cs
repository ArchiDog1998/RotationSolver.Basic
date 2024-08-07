namespace RotationSolver.Basic.Data;

/// <summary>
/// TODO: Check the BLU actions.
/// </summary>
public enum AttackType : byte
{
    /// <summary/>
    Unknown = 0,

    /// <summary/>
    Slashing = 1,

    /// <summary/>
    Piercing = 2,

    /// <summary/>
    Blunt = 3,

    /// <summary/>
    Magic = 5,

    /// <summary/>
    Darkness = 6,

    /// <summary/>
    Physical = 7,

    /// <summary/>
    LimitBreak = 8,
}
