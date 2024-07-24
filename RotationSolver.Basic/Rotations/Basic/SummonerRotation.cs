using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class SummonerRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Intelligence;

    /// <summary/>
    public override bool CanHealSingleSpell => false;

    /// <summary/>
    public static bool InBahamut => ActionID.AstralFlowPvE.AdjustId() == ActionID.DeathflarePvE;

    /// <summary/>
    public static bool InPhoenix => ActionID.AstralFlowPvE.AdjustId() == ActionID.RekindlePvE;

    /// <summary/>
    public static bool InSolarBahamut => ActionID.AstralFlowPvE.AdjustId() == ActionID.SunflarePvE;

    private protected sealed override IBaseAction Raise => ResurrectionPvE;

    #region JobGauge

    /// <summary>
    /// 
    /// </summary>
    private static bool HasSummon => DataCenter.HasPet && SummonTimerRemaining <= 0;
    #endregion

    static partial void ModifyCrimsonCyclonePvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifySummonRubyPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.IfritsFavor];
        setting.ActionCheck = () => HasSummon && IsIfritReady;
    }

    static partial void ModifySummonTopazPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasSummon && IsTitanReady;
    }

    static partial void ModifySummonEmeraldPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.GarudasFavor];
        setting.ActionCheck = () => HasSummon && IsGarudaReady;
    }

    static RandomDelay _carbuncleDelay = new (() => (2, 2));
    static partial void ModifySummonCarbunclePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => _carbuncleDelay.Delay(!DataCenter.HasPet && AttunmentTimerRemaining < 0 && AttunmentTimerRemaining < 0) && DataCenter.LastGCD is not ActionID.SummonCarbunclePvE;
    }

    static partial void ModifyGemshinePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Attunement > 0 && AttunmentTimerRemaining > ActionID.GemshinePvE.GetCastTime();
    }

    static partial void ModifyPreciousBrilliancePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Attunement > 0 && AttunmentTimerRemaining > ActionID.PreciousBrilliancePvE.GetCastTime();
    }

    static partial void ModifyAetherchargePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat && HasSummon;
    }

    static partial void ModifySummonBahamutPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat && HasSummon;
    }

    static partial void ModifyEnkindleBahamutPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InBahamut || InPhoenix;
    }

    static partial void ModifyDeathflarePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InBahamut;
    }

    static partial void ModifyRekindlePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InPhoenix;
    }

    static partial void ModifyCrimsonCyclonePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.IfritsFavor];
    }

    static partial void ModifyMountainBusterPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.TitansFavor];
    }

    static partial void ModifySlipstreamPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.GarudasFavor];
    }

    static partial void ModifyRuinIvPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.FurtherRuin_2701];
    }

    static partial void ModifySearingLightPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.SearingLight];
        setting.ActionCheck = () => InCombat;
        setting.CreateConfig = () => new()
        {
            TimeToKill = 15,
        };
    }

    static partial void ModifyRadiantAegisPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasSummon;
    }

    static partial void ModifyEnergyDrainPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.FurtherRuin];
        setting.ActionCheck = () => !HasAetherflowStacks;
    }

    static partial void ModifyFesterPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasAetherflowStacks;
    }

    static partial void ModifyEnergySiphonPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.FurtherRuin];
        setting.ActionCheck = () => !HasAetherflowStacks;
    }

    static partial void ModifyPainflarePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasAetherflowStacks;
    }

    static partial void ModifyEnkindleSolarBahamutPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InSolarBahamut;
    }

    static partial void ModifySunflarePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InSolarBahamut;
    }

    static partial void ModifySearingFlashPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.RefulgentLux];
    }

    static partial void ModifyLuxSolarisPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.RefulgentLux];
    }

    static partial void ModifyNecrotizePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasAetherflowStacks;
    }

    /// <inheritdoc/>
    protected override bool DefenseSingleAbility(out IAction? act)
    {
        if (RadiantAegisPvE.CanUse(out act)) return true;
        return base.DefenseSingleAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool HealSingleGCD(out IAction? act)
    {
        if (PhysickPvE.CanUse(out act)) return true;
        return base.HealSingleGCD(out act);
    }

    /// <inheritdoc/>
    protected override bool DefenseAreaAbility(out IAction? act)
    {
        if (AddlePvE.CanUse(out act)) return true;
        return base.DefenseAreaAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (SummonPhoenixPvP.CanUse(out act, skipAoeCheck: true)) return true;
        if (EverlastingFlightPvP.CanUse(out act, skipAoeCheck: true)) return true;
        return false;
    }
}