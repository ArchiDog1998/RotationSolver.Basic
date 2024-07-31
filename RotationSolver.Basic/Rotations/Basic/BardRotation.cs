using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class BardRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Dexterity;

    #region Shot
    private static void HawksEye(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.HawksEye_3861, StatusID.Barrage];
    }

    #region Single
    static partial void ModifyStraightShotPvE(ref ActionSetting setting)
    {
        HawksEye(ref setting);
    }

    static partial void ModifyRefulgentArrowPvE(ref ActionSetting setting)
    {
        HawksEye(ref setting);
    }
    #endregion

    #region AoE
    static partial void ModifyQuickNockPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyLadonsbitePvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyWideVolleyPvE(ref ActionSetting setting)
    {
        HawksEye(ref setting);
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyShadowbitePvE(ref ActionSetting setting)
    {
        HawksEye(ref setting);
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyRainOfDeathPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyHeartbreakShotPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }
    #endregion
    #endregion

    #region Bite
    static partial void ModifyIronJawsPvE(ref ActionSetting setting)
    {
        setting.CanTarget = t =>
        {
            if (t.WillStatusEndGCD(0, 0, true, StatusID.VenomousBite, StatusID.CausticBite)) return false;
            if (t.WillStatusEndGCD(0, 0, true, StatusID.Windbite, StatusID.Stormbite)) return false;
            return true;
        };
    }

    #endregion

    #region Song
    static partial void ModifyPitchPerfectPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Song == Song.WANDERER && Repertoire > 0;
    }

    static partial void ModifyMagesBalladPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat;
    }

    static partial void ModifyArmysPaeonPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat;
    }

    static partial void ModifyTheWanderersMinuetPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat;
    }
    #endregion


    static partial void ModifyApexArrowPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => SoulVoice >= 20;
    }

    static partial void ModifyBlastArrowPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.BlastArrowReady];
    }

    static partial void ModifyResonantArrowPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.ResonantArrowReady];
    }

    static partial void ModifyRadiantEncorePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.RadiantEncoreReady];
    }

    static partial void ModifyRadiantFinalePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => JobGauge.Coda.Any(s => s != Song.NONE);
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyRagingStrikesPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyTroubadourPvE(ref ActionSetting setting)
    {
        setting.StatusFromSelf = false;
        setting.StatusProvide = StatusHelper.RangePhysicalDefense;
    }

    static partial void ModifyBattleVoicePvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }


    static partial void ModifyPitchPerfectPvP(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Repertoire];
    }

    static partial void ModifyBlastArrowPvP(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.BlastArrowReady_3142];
    }

    /// <inheritdoc/>
    protected override bool DispelGCD(out IAction? act)
    {
        if (TheWardensPaeanPvE.CanUse(out act)) return true;
        return base.DispelGCD(out act);
    }

    /// <inheritdoc/>
    protected override bool HealSingleAbility(out IAction? act)
    {
        if (NaturesMinnePvE.CanUse(out act)) return true;
        return base.HealSingleAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool DefenseAreaAbility(out IAction act)
    {
        if (TroubadourPvE.CanUse(out act)) return true;
        return false;
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (FinalFantasiaPvP.CanUse(out act, skipAoeCheck: true)) return true;
        return false;
    }
}
