using FFXIVClientStructs.FFXIV.Client.Game;

namespace RotationSolver.Basic.Actions;

/// <summary>
/// 
/// </summary>
/// <param name="CdGrp"></param>
public unsafe readonly record struct CdInfo(byte CdGrp)
{
    private RecastDetail* CoolDownDetail => ActionIdHelper.GetCoolDownDetail(CdGrp);

    /// <summary/>
    public float RecastTime => CoolDownDetail == null ? 0 : CoolDownDetail->Total;

    /// <summary/>
    public float RecastTimeElapsed => CoolDownDetail == null ? 0 : CoolDownDetail->Elapsed;

    /// <summary/>
    public float RecastTimeRemain => RecastTime - RecastTimeElapsed;


    /// <summary/>
    public bool IsCoolingDown => CoolDownDetail != null && CoolDownDetail->IsActive != 0;
}
