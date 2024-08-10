using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class MachinistRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Dexterity;

    #region Basic Combo
    static partial void ModifySlugShotPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.HeatedSplitShotPvE];
    }

    static partial void ModifyHeatedSlugShotPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.HeatedSplitShotPvE];
    }

    static partial void ModifyCleanShotPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.HeatedSlugShotPvE];
    }

    static partial void ModifyHeatedCleanShotPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.HeatedSlugShotPvE];
    }
    #endregion

    #region Basic Aoe
    static partial void ModifySpreadShotPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyScattergunPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }
    #endregion

    #region Overheated Usage
    static void Overheated(ref ActionSetting setting)
    {
        setting.ActionCheck = () => IsOverheated && OverheatTimeRemaining > 0;
    }

    static partial void ModifyHeatBlastPvE(ref ActionSetting setting)
    {
        Overheated(ref setting);
    }

    static partial void ModifyAutoCrossbowPvE(ref ActionSetting setting)
    {
        Overheated(ref setting);
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyBlazingShotPvE(ref ActionSetting setting)
    {
        ModifyAutoCrossbowPvE(ref setting);
    }
    #endregion

    #region Battery Usage
    static partial void ModifyRookAutoturretPvE(ref ActionSetting setting)
    {
        ModifyAutomatonQueenPvE(ref setting);
        setting.ReplacingTrait = PromotionTrait;
    }

    static partial void ModifyAutomatonQueenPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Battery >= 50 && !JobGauge.IsRobotActive;
        setting.CreateConfig = () => new()
        {
            TimeToKill = 8,
        };
    }

    static partial void ModifyRookOverdrivePvE(ref ActionSetting setting)
    {
        ModifyQueenOverdrivePvE(ref setting);
        setting.ReplacingTrait = PromotionTrait;

    }

    static partial void ModifyQueenOverdrivePvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 8,
        };
    }
    #endregion

    static partial void ModifyReassemblePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasHostilesInRange;
    }

    static partial void ModifyHyperchargePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => !IsOverheated && (Heat >= 50 || Player.HasStatus(true, StatusID.Hypercharged));
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

    static partial void ModifyBarrelStabilizerPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat;
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
