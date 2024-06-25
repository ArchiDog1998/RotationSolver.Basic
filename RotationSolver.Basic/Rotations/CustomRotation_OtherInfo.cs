namespace RotationSolver.Basic.Rotations;
partial class CustomRotation
{
    /// <summary>
    /// 
    /// </summary>
    [Description("Can heal area ability")]
    public virtual bool CanHealAreaAbility => true;

    /// <summary>
    /// 
    /// </summary>
    [Description("Can heal area spell")]
    public virtual bool CanHealAreaSpell => true;

    /// <summary>
    /// 
    /// </summary>
    [Description("Can heal single ability")]
    public virtual bool CanHealSingleAbility => true;

    /// <summary>
    /// 
    /// </summary>
    [Description("Can heal single area")]
    public virtual bool CanHealSingleSpell => true;

    /// <summary>
    /// 
    /// </summary>
    public double AverageCountOfLastUsing { get; internal set; } = 0;

    /// <summary>
    /// 
    /// </summary>
    public int MaxCountOfLastUsing { get; internal set; } = 0;

    /// <summary>
    /// 
    /// </summary>
    public double AverageCountOfCombatTimeUsing { get; internal set; } = 0;

    /// <summary>
    /// 
    /// </summary>
    public int MaxCountOfCombatTimeUsing { get; internal set; } = 0;
    internal long CountOfTracking { get; set; } = 0;
}
