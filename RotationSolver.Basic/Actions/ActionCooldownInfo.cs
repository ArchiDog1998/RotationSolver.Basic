using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace RotationSolver.Basic.Actions;

/// <summary>
/// The action cooldown information.
/// </summary>
public readonly struct ActionCooldownInfo : ICooldown
{
    private readonly IBaseAction _action;

    /// <summary>
    /// The cd group.
    /// </summary>
    public CdInfo[] CoolDownGroups { get; }

    /// <summary>
    /// Recast time.
    /// </summary>
    public unsafe float RecastTime => CoolDownGroups[0].RecastTime;

    /// <summary/>
    public float RecastTimeElapsed => RecastTimeElapsedRaw - DataCenter.WeaponElapsed;

    /// <summary/>
    internal unsafe float RecastTimeElapsedRaw => CoolDownGroups[0].RecastTimeElapsed;

    float ICooldown.RecastTimeElapsedRaw => RecastTimeElapsedRaw;

    /// <summary/>
    public unsafe bool IsCoolingDown => CoolDownGroups.Any(i => i.IsCoolingDown);

    private float RecastTimeRemainRaw => CoolDownGroups[0].RecastTimeRemain;

    /// <summary/>
    public bool HasOneCharge => CurrentCharges > 0;

    /// <summary/>
    public unsafe ushort CurrentCharges => (ushort)ActionManager.Instance()->GetCurrentCharges(_action.Info.ID);

    /// <summary/>
    public unsafe ushort MaxCharges => Math.Max(ActionManager.GetMaxCharges(_action.Info.ID, (uint)Player.Level), (ushort)1);

    internal float RecastTimeOneChargeRaw => ActionManager.GetAdjustedRecastTime(ActionType.Action, _action.Info.ID) / 1000f;

    float ICooldown.RecastTimeOneChargeRaw => RecastTimeOneChargeRaw;

    /// <summary/>
    public float RecastTimeRemainOneCharge => RecastTimeRemainOneChargeRaw - DataCenter.WeaponRemain;

    float RecastTimeRemainOneChargeRaw
    {
        get
        {
            var result = RecastTimeRemainRaw % RecastTimeOneChargeRaw;
            if (CoolDownGroups.Length > 1)
            {
                result = MathF.Max(result,  CoolDownGroups[1].RecastTimeRemain);
            }
            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float RecastTimeElapsedOneCharge => RecastTimeElapsedOneChargeRaw - DataCenter.WeaponElapsed;

    float RecastTimeElapsedOneChargeRaw => RecastTimeElapsedRaw % RecastTimeOneChargeRaw;

    /// <summary>
    /// The default constructor.
    /// </summary>
    /// <param name="action">the action.</param>
    public ActionCooldownInfo(IBaseAction action)
    {
        _action = action;
        CoolDownGroups = _action.Action.GetCoolDownGroup();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gcdCount"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public bool ElapsedOneChargeAfterGCD(uint gcdCount = 0, float offset = 0)
        => ElapsedOneChargeAfter(DataCenter.GCDTime(gcdCount, offset));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool ElapsedOneChargeAfter(float time)
        => IsCoolingDown && time <= RecastTimeElapsedOneCharge;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gcdCount"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public bool ElapsedAfterGCD(uint gcdCount = 0, float offset = 0)
        => ElapsedAfter(DataCenter.GCDTime(gcdCount, offset));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool ElapsedAfter(float time)
        => IsCoolingDown && time <= RecastTimeElapsed;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gcdCount"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public bool WillHaveOneChargeGCD(uint gcdCount = 0, float offset = 0)
        => WillHaveOneCharge(DataCenter.GCDTime(gcdCount, offset));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="remain"></param>
    /// <returns></returns>
    public bool WillHaveOneCharge(float remain)
        => HasOneCharge || RecastTimeRemainOneCharge <= remain;

    /// <summary>
    /// Is this action used after several time.
    /// If the action is ready, this will return <see langword="true"/>.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool JustUsedAfter(float time)
    {
        if (!IsCoolingDown) return true;
        var elapsed = RecastTimeElapsedRaw % RecastTimeOneChargeRaw;
        return elapsed + DataCenter.WeaponRemain < time;
    }

    internal bool CooldownCheck(bool useUp, bool onLastAbility, bool ignoreClippingCheck, byte gcdCountForAbility, out WhyActionCantUse whyCant)
    {
        if (!_action.Info.IsGeneralGCD)
        {
            if (IsCoolingDown)
            {
                if (_action.Info.IsRealGCD)
                {
                    if (!WillHaveOneChargeGCD(0, 0))
                    {
                        whyCant = WhyActionCantUse.NoChargesGCD;
                        return false;
                    }
                }
                else
                {
                    if (!HasOneCharge && RecastTimeRemainOneChargeRaw > DataCenter.AnimationLocktime)
                    {
                        whyCant = WhyActionCantUse.NoCharges0GCD;
                        return false;
                    }
                }
            }

            if (!useUp)
            {
                if (RecastTimeRemainRaw > DataCenter.WeaponRemain + DataCenter.WeaponTotal * gcdCountForAbility)
                {
                    whyCant = WhyActionCantUse.NotEmpty;
                    return false;
                }
            }
        }

        if (!_action.Info.IsRealGCD) //0GCD
        {
            if (onLastAbility)
            {
                if (DataCenter.NextAbilityToNextGCD > _action.Info.AnimationLockTime + DataCenter.Ping + DataCenter.MinAnimationLock)
                {
                    whyCant = WhyActionCantUse.OnLast;
                    return false;
                }
            }
            if (!ignoreClippingCheck)
            {
                if (DataCenter.NextAbilityToNextGCD < _action.Info.AnimationLockTime + DataCenter.Ping)
                {
                    whyCant = WhyActionCantUse.Clipping;
                    return false;
                }
            }
        }

        whyCant = WhyActionCantUse.None;
        return true;
    }
}
