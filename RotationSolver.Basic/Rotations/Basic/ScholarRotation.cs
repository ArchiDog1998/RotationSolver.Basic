﻿using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class ScholarRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Mind;

    #region Job Gauge
    /// <summary>
    /// 
    /// </summary>
    public static bool HasAetherflow => Aetherflow > 0;
    #endregion
    private sealed protected override IBaseAction Raise => ResurrectionPvE;

    static partial void ModifyAdloquiumPvE(ref ActionSetting setting)
    {
        setting.StatusFromSelf = false;
        setting.StatusProvide =
        [
            StatusID.EukrasianDiagnosis,
            StatusID.EukrasianPrognosis,
            StatusID.Galvanize
        ];
    }

    static partial void ModifySuccorPvE(ref ActionSetting setting)
    {
        setting.StatusFromSelf = false;
        setting.StatusProvide = [StatusID.Galvanize];
    }

    static partial void ModifyLustratePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasAetherflow;
    }

    static partial void ModifySacredSoilPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasAetherflow && !IsMoving;
    }

    static partial void ModifyIndomitabilityPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasAetherflow;
    }

    static partial void ModifyExcogitationPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasAetherflow;
    }

    static partial void ModifyConsolationPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => SeraphTimer > 0;
    }

    static partial void ModifyBioPvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.Bio, StatusID.BioIi, StatusID.Biolysis];
    }

    static partial void ModifyEnergyDrainPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasAetherflow;
    }

    static partial void ModifySummonSeraphPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => CombatData.HasPet;
    }

    static partial void ModifySummonEosPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => !CombatData.HasPet && (!Player.HasStatus(true, StatusID.Dissipation)// || Dissipation.WillHaveOneCharge(30) && Dissipation.EnoughLevel
        );
    }

    static partial void ModifyWhisperingDawnPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => CombatData.HasPet;
    }

    static partial void ModifyFeyIlluminationPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasPet;
    }

    static partial void ModifyDissipationPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Dissipation];
        setting.ActionCheck = () => !HasAetherflow && SeraphTimer <= 0 && InCombat && HasPet;
    }

    static partial void ModifyAetherpactPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => FairyGauge >= 10 && HasPet && SeraphTimer <= 0;
    }

    static partial void ModifyFeyBlessingPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => SeraphTimer <= 0 && HasPet;
    }

    static partial void ModifyAetherflowPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat && !HasAetherflow;
    }

    static partial void ModifyChainStratagemPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat;
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyArtOfWarPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyDeploymentTacticsPvE(ref ActionSetting setting)
    {
        setting.TargetStatusNeed = [StatusID.Galvanize];
    }

    /// <inheritdoc/>
    protected override bool SpeedAbility(out IAction? act)
    {
        if (InCombat && ExpedientPvE.CanUse(out act, usedUp: true)) return true;
        return base.SpeedAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (SummonSeraphPvP.CanUse(out act, skipAoeCheck: true)) return true;
        if (SeraphFlightPvP.CanUse(out act, skipAoeCheck: true)) return true;
        return base.LimitBreakPvPGCD(out act);
    }
}