using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class BlackMageRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Intelligence;

    #region Job Gauge
    /// <summary>
    /// 
    /// </summary>
    public static bool IsPolyglotStacksMaxed => PolyglotStacks >= MaxPolyglotStacks;

    /// <summary>
    /// 
    /// </summary>
    public static byte MaxPolyglotStacks
    {
        get
        {
            if (EnhancedPolyglotIiTrait.EnoughLevel) return 3;
            else if (EnhancedPolyglotTrait.EnoughLevel) return 2;
            else return 1;
        }
    }
    #endregion

    #region Thunder
    private static void Thunder(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Thunderhead];
    }

    static partial void ModifyThunderPvE(ref ActionSetting setting)
    {
        Thunder(ref setting);
    }

    static partial void ModifyThunderIiPvE(ref ActionSetting setting)
    {
        Thunder(ref setting);
    }

    static partial void ModifyThunderIiiPvE(ref ActionSetting setting)
    {
        Thunder(ref setting);
    }

    static partial void ModifyThunderIvPvE(ref ActionSetting setting)
    {
        Thunder(ref setting);
    }

    static partial void ModifyHighThunderPvE(ref ActionSetting setting)
    {
        Thunder(ref setting);
    }

    static partial void ModifyHighThunderIiPvE(ref ActionSetting setting)
    {
        Thunder(ref setting);
    }

    /// <summary>
    /// Use Thunder actions.
    /// </summary>
    /// <param name="act"></param>
    /// <param name="skipStatusProvideCheck"></param>
    /// <returns></returns>
    public bool UseThunder(out IAction? act, bool skipStatusProvideCheck = false)
    {
        if (ThunderIiPvEReplace.CanUse(out act, skipStatusProvideCheck: skipStatusProvideCheck)) return true;
        if (ThunderPvEReplace.CanUse(out act, skipStatusProvideCheck : skipStatusProvideCheck)) return true;
        return false;
    }
    #endregion

    #region Fire
    private static void FireCheck(ref ActionSetting setting, ActionID action)
    {
        setting.ActionCheck = () => InAstralFire && ElementTimeRemaining > action.GetCastTime() - 0.1f;
    }

    static partial void ModifyFireIiiPvE(ref ActionSetting setting)
    {
        setting.MPOverride = () => Player.WillStatusEnd(0, true, StatusID.Firestarter) ? null : 0;
    }

    static partial void ModifyFireIvPvE(ref ActionSetting setting)
    {
        FireCheck(ref setting, ActionID.FireIvPvE);
    }

    static partial void ModifyDespairPvE(ref ActionSetting setting)
    {
        FireCheck(ref setting, ActionID.DespairPvE);
    }

    static partial void ModifyFlarePvE(ref ActionSetting setting)
    {
        FireCheck(ref setting, ActionID.FlarePvE);
    }

    static partial void ModifyFlareStarPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => AstralSoulStacks == 6
            && InAstralFire && ElementTimeRemaining > ActionID.FlareStarPvE.GetCastTime() - 0.1f;
    }
    #endregion

    #region Ice
    private static void IceCheck(ref ActionSetting setting, ActionID action)
    {
        setting.ActionCheck = () => InUmbralIce && ElementTimeRemaining > action.GetCastTime() - 0.1f;
    }

    static partial void ModifyBlizzardIvPvE(ref ActionSetting setting)
    {
        IceCheck(ref setting, ActionID.BlizzardIvPvE);
    }

    static partial void ModifyFreezePvE(ref ActionSetting setting)
    {
        IceCheck(ref setting, ActionID.FreezePvE);
    }

    /// <summary>
    /// To get the <see cref="UmbralHearts"/>
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    public bool GetUmbralHearts(out IAction? act)
    {
        if (FreezePvE.CanUse(out act)) return true;
        if (BlizzardIvPvE.CanUse(out act)) return true;
        return false;
    }
    #endregion

    #region None Elements
    static partial void ModifyXenoglossyPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => PolyglotStacks > 0;
    }

    static partial void ModifyParadoxPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => IsParadoxActive;
    }

    static partial void ModifyFoulPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => PolyglotStacks > 0;
    }
    #endregion

    #region Leylines
    static partial void ModifyLeyLinesPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 15,
        };
    }

    static partial void ModifyBetweenTheLinesPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.LeyLines];
    }

    static partial void ModifyRetracePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.LeyLines];
    }
    #endregion

    static partial void ModifyAetherialManipulationPvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyAmplifierPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => EnochianTimer > 5 && !IsPolyglotStacksMaxed;
    }

    static partial void ModifyManafontPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InAstralFire;
    }

    static partial void ModifyTriplecastPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = StatusHelper.SwiftcastStatus;
    }

    static partial void ModifyTransposePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => DataCenter.AnimationLocktime <= ElementTimeRemaining + DataCenter.WeaponRemain;
    }

    static partial void ModifyUmbralSoulPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InUmbralIce && DataCenter.AnimationLocktime <= ElementTimeRemaining + DataCenter.WeaponRemain;
    }

    /// <inheritdoc/>
    protected override bool DefenseSingleGCD(out IAction? act)
    {
        if (ManawardPvE.CanUse(out act)) return true;
        return base.DefenseSingleGCD(out act);
    }

    /// <inheritdoc/>
    protected override bool DefenseAreaAbility(out IAction act)
    {
        if (AddlePvE.CanUse(out act)) return true;
        return false;
    }

    /// <inheritdoc/>
    protected override bool MoveForwardGCD(out IAction? act)
    {
        if (AetherialManipulationPvE.CanUse(out act)) return true;
        return base.MoveForwardGCD(out act);
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (SoulResonancePvP.CanUse(out act, skipAoeCheck: true)) return true;
        return false;
    }
}
