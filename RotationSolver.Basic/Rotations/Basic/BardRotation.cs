using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
partial class BardRotation
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Dexterity;

    static partial void ModifyHeavyShotPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.StraightShotReady];
    }

    static partial void ModifyStraightShotPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.StraightShotReady];
    }

    static partial void ModifyVenomousBitePvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.VenomousBite, StatusID.CausticBite];
    }

    static partial void ModifyWindbitePvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.Windbite, StatusID.Stormbite];
    }

    static partial void ModifyIronJawsPvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.VenomousBite, StatusID.CausticBite, StatusID.Windbite, StatusID.Stormbite];
        setting.CanTarget = t =>
        {
            if (t.WillStatusEndGCD(0, 0, true, StatusID.VenomousBite, StatusID.CausticBite)) return false;
            if (t.WillStatusEndGCD(0, 0, true, StatusID.Windbite, StatusID.Stormbite)) return false;
            return true;
        };
    }

    static partial void ModifyPitchPerfectPvP(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Repertoire];
    }

    static partial void ModifySilentNocturnePvP(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Repertoire];
    }

    static partial void ModifyTheWardensPaeanPvP(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Repertoire];
    }

    static partial void ModifyBlastArrowPvP(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.BlastArrowReady_3142];
    }

    static partial void ModifyPitchPerfectPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Song == Song.WANDERER && Repertoire > 0;
    }

    static partial void ModifyQuickNockPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.ShadowbiteReady];
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyShadowbitePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.ShadowbiteReady];
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyApexArrowPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.BlastArrowReady];
        setting.ActionCheck = () => SoulVoice >= 20;
    }

    static partial void ModifyBlastArrowPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.BlastArrowReady];
    }

    static partial void ModifyRainOfDeathPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
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
