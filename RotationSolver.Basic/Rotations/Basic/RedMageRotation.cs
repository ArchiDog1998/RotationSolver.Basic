using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class RedMageRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Intelligence;

    /// <inheritdoc/>
    public override bool CanHealSingleSpell => DataCenter.PartyMembers.Length == 1 && base.CanHealSingleSpell;

    #region Job Gauge
    /// <summary>
    /// Is <see cref="WhiteMana"/> larger than <see cref="BlackMana"/>
    /// </summary>
    public static bool IsWhiteManaLargerThanBlackMana => WhiteMana > BlackMana;
    #endregion

    /// <summary>
    /// Has Swift cast
    /// </summary>
    protected static bool HasSwift => !Player.WillStatusEndGCD(0, 0, true, SwiftcastStatus);

    private protected sealed override IBaseAction Raise => VerraisePvE;

    private static StatusID[] SwiftcastStatus { get; } = [.. StatusHelper.SwiftcastStatus, StatusID.Acceleration];

    static partial void ModifyCorpsacorpsPvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyCorpsacorpsPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    #region Ver things
    static void VerThing(ref ActionSetting setting)
    {
        setting.ActionCheck = () => !RedMagicMasteryIiiTrait.EnoughLevel || EnchantedBladeMasteryTrait.EnoughLevel;
    }

    static partial void ModifyVerfirePvE(ref ActionSetting setting)
    {
        VerThing(ref setting);
    }

    static partial void ModifyVerstonePvE(ref ActionSetting setting)
    {
        VerThing(ref setting);
    }
    #endregion

    #region Mana Getter
    static partial void ModifyScatterPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyImpactPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static void WhiteManaUsage(ref ActionSetting setting, byte mana)
    {
        setting.ActionCheck = () =>
        {
            if (WhiteMana + mana == BlackMana) return false;
            if (WhiteMana <= BlackMana) return true;
            if (BlackMana + mana == WhiteMana) return true;
            return false;
        };
    }

    static partial void ModifyVeraeroPvE(ref ActionSetting setting)
    {
        WhiteManaUsage(ref setting, 6);
    }

    static partial void ModifyVeraeroIiPvE(ref ActionSetting setting)
    {
        WhiteManaUsage(ref setting, 7);
    }

    static partial void ModifyVeraeroIiiPvE(ref ActionSetting setting)
    {
        WhiteManaUsage(ref setting, 6);
    }

    /// <summary>
    /// Short casting aoe.
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    protected bool AoeShort(out IAction? act)
    {
        if (VeraeroIiPvE.CanUse(out act)) return true;
        if (VerthunderIiPvE.CanUse(out act)) return true;
        return false;
    }


    /// <summary>
    /// Long casting aoe
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    protected bool AoeLong(out IAction? act)
    {
        if (ImpactPvE.CanUse(out act)) return true;
        if (ScatterPvE.CanUse(out act)) return true;
        return false;
    }

    /// <summary>
    /// Long casting single
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    protected bool SingleLong(out IAction? act)
    {
        if (VeraeroIiiPvE.CanUse(out act, skipStatusProvideCheck: true)) return true;
        if (VeraeroPvE.CanUse(out act, skipStatusProvideCheck: true)) return true;
        if (VerthunderIiiPvE.CanUse(out act, skipStatusProvideCheck: true)) return true;
        if (VerthunderPvE.CanUse(out act, skipStatusProvideCheck: true)) return true;
        return false;
    }

    /// <summary>
    /// Short casting single
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    protected bool SingleShort(out IAction? act)
    {
        if (VerfirePvE.CanUse(out act)) return true;
        if (VerstonePvE.CanUse(out act)) return true;
        if (JoltIiiPvE.CanUse(out act)) return true;
        if (JoltIiPvE.CanUse(out act)) return true;
        if (JoltPvE.CanUse(out act)) return true;
        return false;
    }
    #endregion

    #region Mana Usage
    static void ManaAction(ref ActionSetting setting, byte value)
    {
        setting.ActionCheck = () => Player.HasStatus(true, StatusID.MagickedSwordplay) 
        || BlackMana >= value && WhiteMana >= value;
    }

    static partial void ModifyEnchantedRipostePvE(ref ActionSetting setting)
    {
        ManaAction(ref setting, 20);
    }

    static partial void ModifyZwerchhauPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.EnchantedRipostePvE];
    }

    static partial void ModifyEnchantedZwerchhauPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.EnchantedRipostePvE];
        ManaAction(ref setting, 15);
    }

    static partial void ModifyRedoublementPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.EnchantedZwerchhauPvE];
    }

    static partial void ModifyEnchantedRedoublementPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.EnchantedZwerchhauPvE];
        ManaAction(ref setting, 15);
    }

    static partial void ModifyEnchantedMoulinetPvE(ref ActionSetting setting)
    {
        ManaAction(ref setting, 20);
    }

    static partial void ModifyEnchantedMoulinetDeuxPvE(ref ActionSetting setting)
    {
        ManaAction(ref setting, 15);
    }

    static partial void ModifyEnchantedMoulinetTroisPvE(ref ActionSetting setting)
    {
        ManaAction(ref setting, 15);
    }

    static partial void ModifyVerflarePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => ManaStacks >= 3;
    }

    static partial void ModifyVerholyPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => ManaStacks >= 3 && !IsWhiteManaLargerThanBlackMana;
    }

    static partial void ModifyScorchPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.VerflarePvE, ActionID.VerholyPvE];
    }

    static partial void ModifyResolutionPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.ScorchPvE];
    }

    /// <summary>
    /// Finish the mana use burst actions.
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    protected bool FinishManaUsage(out IAction? act)
    {
        if (ResolutionPvE.CanUse(out act, skipAoeCheck: true, skipStatusProvideCheck: true)) return true;
        if (ScorchPvE.CanUse(out act, skipAoeCheck: true, skipStatusProvideCheck: true)) return true;
        if (VerholyPvE.CanUse(out act, skipAoeCheck: true, skipStatusProvideCheck: true)) return true;
        if (VerflarePvE.CanUse(out act, skipAoeCheck: true, skipStatusProvideCheck: true)) return true;

        if (EnchantedMoulinetDeuxPvE.CanUse(out act, skipAoeCheck: true)) return true;
        if (EnchantedMoulinetTroisPvE.CanUse(out act, skipAoeCheck: true)) return true;
        if (EnchantedRedoublementPvE.CanUse(out act)) return true;
        if (EnchantedZwerchhauPvE.CanUse(out act)) return true;

        return false;
    }
    #endregion

    static partial void ModifyAccelerationPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Acceleration];
    }

    static partial void ModifyEmboldenPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyManaficationPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat && ManaStacks == 0
            && !IsLastGCD(ActionID.VerflarePvE, ActionID.VerholyPvE, ActionID.ScorchPvE);

        //Don't break the combo!
        setting.ComboIdsNot =
        [
            ActionID.RipostePvE,
            ActionID.EnchantedRipostePvE,
            ActionID.ZwerchhauPvE,
            ActionID.EnchantedZwerchhauPvE,

            ActionID.MoulinetPvE,
            ActionID.EnchantedMoulinetPvE,
            ActionID.EnchantedMoulinetDeuxPvE,

            ActionID.VerflarePvE,
            ActionID.VerholyPvE,
            ActionID.ScorchPvE,
        ];

        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    /// <inheritdoc/>
    protected override bool HealSingleGCD(out IAction? act)
    {
        if (VercurePvE.CanUse(out act, skipStatusProvideCheck: true)) return true;
        return base.HealSingleGCD(out act);
    }

    /// <inheritdoc/>
    protected override bool MoveForwardAbility(out IAction? act)
    {
        if (CorpsacorpsPvE.CanUse(out act)) return true;
        return base.MoveForwardAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool DefenseAreaAbility(out IAction? act)
    {
        if (AddlePvE.CanUse(out act)) return true;
        if (MagickBarrierPvE.CanUse(out act, skipAoeCheck: true)) return true;
        return base.DefenseAreaAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (SouthernCrossPvP.CanUse(out act, skipAoeCheck: true)) return true;
        if (SouthernCrossPvP_29705.CanUse(out act, skipAoeCheck: true)) return true;
        return false;
    }
}