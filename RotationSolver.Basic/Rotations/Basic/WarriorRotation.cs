using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class WarriorRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Strength;

    private sealed protected override IBaseActionSet TankStance => DefiancePvEReplace;

    static partial void ModifyPrimalRendPvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyPrimalRuinationPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.PrimalRuinationReady];
    }

    static partial void ModifyOnslaughtPvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyStormsEyePvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            StatusGcdCount = 9,
        };
    }

    #region BeastGauge
    private static void BeastGaugeSingle(ref ActionSetting setting)
    {
        setting.ActionCheck = () => BeastGauge >= 50 || Player.HasStatus(true, StatusID.InnerRelease);
    }

    private static void BeastGaugeAoe(ref ActionSetting setting)
    {
        BeastGaugeSingle(ref setting);
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    private static void BeastReplaceStatus(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.NascentChaos];
    }

    static partial void ModifyInnerBeastPvE(ref ActionSetting setting)
        => BeastGaugeSingle(ref setting);

    static partial void ModifyFellCleavePvE(ref ActionSetting setting)
        => BeastGaugeSingle(ref setting);

    static partial void ModifyInnerChaosPvE(ref ActionSetting setting)
    {
        BeastGaugeSingle(ref setting);
        BeastReplaceStatus(ref setting);
    }

    static partial void ModifySteelCyclonePvE(ref ActionSetting setting)
        => BeastGaugeAoe(ref setting);

    static partial void ModifyDecimatePvE(ref ActionSetting setting)
        => BeastGaugeAoe(ref setting);

    static partial void ModifyChaoticCyclonePvE(ref ActionSetting setting)
    {
        BeastGaugeAoe(ref setting);
        BeastReplaceStatus(ref setting);
    }
    #endregion

    static partial void ModifyTomahawkPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MeleeRange;
        setting.TargetType = TargetType.ProvokeOrOthers;
    }

    static partial void ModifyPrimalRendPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.PrimalRendReady];
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyInfuriatePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasHostilesInRange && BeastGauge <= 50 && InCombat;
        setting.CreateConfig = () => new()
        {
            TimeToKill = 5,
        };
    }

    static partial void ModifyInnerReleasePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasHostilesInRange;
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyBerserkPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasHostilesInRange;
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyOverpowerPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyMythrilTempestPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyVengeancePvE(ref ActionSetting setting)
    {
        setting.StatusProvide = StatusHelper.RampartStatus;
        setting.ActionCheck = Player.IsTargetOnSelf;
    }

    static partial void ModifyRawIntuitionPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = Player.IsTargetOnSelf;
    }

    static partial void ModifyHolmgangPvE(ref ActionSetting setting)
    {
        setting.TargetType = TargetType.Self;
    }

    static partial void ModifyFellCleavePvP(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.InnerRelease_1303];
    }

    static partial void ModifyChaoticCyclonePvP(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.NascentChaos_1992];
    }

    static partial void ModifyOnslaughtPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyPrimalWrathPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Wrathful];
    }

    static partial void ModifyDamnationPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = StatusHelper.RampartStatus;
        setting.ActionCheck = Player.IsTargetOnSelf;
    }

    /// <inheritdoc/>
    protected override bool EmergencyAbility(IAction nextGCD, out IAction? act)
    {
        if (HolmgangPvE.CanUse(out act)
            && Player.GetHealthRatio() <= Service.Config.HealthForDyingTanks) return true;
        return base.EmergencyAbility(nextGCD, out act);
    }

    /// <inheritdoc/>
    protected override bool MoveForwardAbility(out IAction? act)
    {
        if (OnslaughtPvE.CanUse(out act)) return true;
        return base.MoveForwardAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool MoveForwardGCD(out IAction? act)
    {
        if (PrimalRendPvE.CanUse(out act, skipAoeCheck: true)) return true;
        return base.MoveForwardGCD(out act);
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (PrimalScreamPvP.CanUse(out act, skipAoeCheck: true)) return true;
        return false;
    }
}