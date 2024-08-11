using ECommons.GameHelpers;

namespace RotationSolver.Basic.Rotations;
partial class CustomRotation
{
    /// <summary>
    /// Can heal area ability
    /// </summary>
    [Description("Can heal area ability")]
    public virtual bool CanHealAreaAbility => true;

    /// <summary>
    /// Can heal area spell
    /// </summary>
    [Description("Can heal area spell")]
    public virtual bool CanHealAreaSpell => true;

    /// <summary>
    /// Can heal single ability
    /// </summary>
    [Description("Can heal single ability")]
    public virtual bool CanHealSingleAbility => true;

    /// <summary>
    /// Can heal single area
    /// </summary>
    [Description("Can heal single area")]
    public virtual bool CanHealSingleSpell => true;

    /// <summary/>
    public double AverageCountOfLastUsing { get; internal set; } = 0;

    /// <summary/>
    public int MaxCountOfLastUsing { get; internal set; } = 0;

    /// <summary/>
    public double AverageCountOfCombatTimeUsing { get; internal set; } = 0;

    /// <summary/>
    public int MaxCountOfCombatTimeUsing { get; internal set; } = 0;

    internal long CountOfTracking { get; set; } = 0;

    private protected static void GreatDefense(ref ActionSetting setting)
    {
        setting.StatusProvide = StatusHelper.RampartStatus;
        setting.ActionCheck = CombatData.Player.IsTargetOnSelf;
    }
}
