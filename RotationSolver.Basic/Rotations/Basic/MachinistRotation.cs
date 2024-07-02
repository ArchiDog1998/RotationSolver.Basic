using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
partial class MachinistRotation
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Dexterity;

    static partial void ModifySlugShotPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.HeatedSplitShotPvE];
    }

    static partial void ModifyCleanShotPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.HeatedSlugShotPvE];
    }

    static partial void ModifyHeatBlastPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => IsOverheated && OverheatTimeRemaining > 0;
    }

    static partial void ModifyAutoCrossbowPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => IsOverheated && OverheatTimeRemaining > 0;
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyRookAutoturretPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Battery >= 50 && !JobGauge.IsRobotActive;
        setting.CreateConfig = () => new()
        {
            TimeToKill = 8,
        };
    }

    static partial void ModifyReassemblePvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Reassembled];
        setting.ActionCheck = () => HasHostilesInRange;
    }

    static partial void ModifyHyperchargePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => !IsOverheated && Heat >= 50;
        setting.CreateConfig = () => new()
        {
            TimeToKill = 8,
        };
    }

    static partial void ModifyWildfirePvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 8,
        };
    }

    static partial void ModifyQueenOverdrivePvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 8,
        };
    }

    static partial void ModifySpreadShotPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyBarrelStabilizerPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Heat <= 50 && InCombat;
    }

    static partial void ModifyTacticianPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = StatusHelper.RangePhysicalDefense;
    }

    /// <inheritdoc/>
    protected override bool DefenseAreaAbility(out IAction act)
    {
        if (TacticianPvE.CanUse(out act, skipAoeCheck: true)) return true;
        if (DismantlePvE.CanUse(out act, skipAoeCheck: true)) return true;
        return false;
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (MarksmansSpitePvP.CanUse(out act, skipAoeCheck: true)) return true;
        return false;
    }
}
