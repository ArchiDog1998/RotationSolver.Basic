using RotationSolver.Basic.Record;
using XIVConfigUI;

namespace RotationSolver.Basic.Rotations.Duties;

partial class DutyRotation : IDisposable
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    #region GCD
    public virtual bool EmergencyGCD(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool GeneralGCD(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool RaiseGCD(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool DispelGCD(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool MoveForwardGCD(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool HealSingleGCD(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool HealAreaGCD(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool DefenseSingleGCD(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool DefenseAreaGCD(out IAction? act)
    {
        act = null; return false;
    }
    #endregion

    #region Ability
    public virtual bool InterruptAbility(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool AntiKnockbackAbility(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool ProvokeAbility(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool EmergencyAbility(IAction nextGCD, out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool MoveForwardAbility(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool MoveBackAbility(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool HealSingleAbility(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool HealAreaAbility(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool DefenseSingleAbility(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool DefenseAreaAbility(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool SpeedAbility(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool GeneralAbility(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool AttackAbility(out IAction? act)
    {
        act = null; return false;
    }

    public virtual bool DispelAbility(out IAction? act)
    {
        act = null; return false;
    }


    #endregion
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    #region Duty
    /// <summary>
    /// The timeline Items.
    /// </summary>
    public static TimelineItem[] TimelineItems => DataCenter.TimelineItems;

    /// <summary>
    /// The Raid Time.
    /// </summary>
    public static float RaidTimeRaw => DataCenter.RaidTimeRaw;

    /// <summary>
    /// Get the record data
    /// </summary>
    /// <typeparam name="T">the record data type</typeparam>
    /// <param name="min">The min time from now</param>
    /// <param name="max">The max time from now</param>
    /// <returns></returns>
    public static T[] GetRecordData<T>(float min, float max) where T : struct, IRecordData => Recorder.GetData<T>(min, max);
    #endregion

    #region Drawing
    internal void UpdateInfo()
    {
        UpdateDrawing();
    }

    /// <summary>
    /// Update every frame for drawings
    /// </summary>
    public virtual void UpdateDrawing()
    {

    }

    /// <summary>
    /// When a new actor showned.
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnNewActor(in ObjectNewData data)
    {

    }

    /// <summary>
    /// When a new actor effect is created.
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnActorVfxNew(in VfxNewData data)
    {

    }

    /// <summary>
    /// When on the object effect
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnObjectEffect(in ObjectEffectData data)
    {

    }

    /// <summary>
    /// When on the map effect.
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnMapEffect(in MapEffectData data)
    {

    }

    /// <summary>
    /// Any actions from the enemy to us.
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnActionFromEnemy(in ActionEffectSetData data)
    {

    }

    /// <summary>
    /// To destroy all drawing.
    /// </summary>
    public virtual void DestroyAllDrawing()
    {

    }
    #endregion

    internal SearchableCollection Configs { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    protected DutyRotation()
    {
        Configs = new SearchableCollection(this, new RotationSearchableConfig());

        //Load from config.
        var savedConfigs = Service.Config.DutyRotationConfig;
        foreach (var item in Configs)
        {
            item._default = item._property.GetValue(this)!;
            if (savedConfigs.TryGetValue(item._property.Name, out var value))
            {
                item.OnCommand(value);
            }
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        DestroyAllDrawing();
        GC.SuppressFinalize(this);
    }

    internal IAction[] AllActions
    {
        get
        {
            var properties = this.GetType().GetRuntimeProperties()
                .Where(p => DataCenter.DutyActions.Contains(p.GetCustomAttribute<IDAttribute>()?.ID ?? uint.MaxValue));

            if (properties == null || !properties.Any()) return [];

            return [.. properties.Select(p => (IAction)p.GetValue(this)!)];
        }
    }
}
