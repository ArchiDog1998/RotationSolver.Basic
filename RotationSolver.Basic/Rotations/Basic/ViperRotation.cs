using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class ViperRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Strength;

    #region Single
    #region 1
    static partial void ModifyDreadFangsPvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.NoxiousGnash];
        setting.CreateConfig = () => new()
        {
            StatusGcdCount = 8,
        };
    }
    #endregion

    #region 2
    static partial void ModifyHuntersStingPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.DreadFangsPvE];
    }

    static partial void ModifySwiftskinsStingPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.DreadFangsPvE];
    }

    #endregion

    #region 3
    static partial void ModifyFlankstingStrikePvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Flank;
        setting.NeedsHighlight = true;
    }

    static partial void ModifyFlanksbaneFangPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Flank;
        setting.NeedsHighlight = true;
    }

    static partial void ModifyHindstingStrikePvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Rear;
        setting.NeedsHighlight = true;
    }

    static partial void ModifyHindsbaneFangPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Rear;
        setting.NeedsHighlight = true;
    }
    #endregion

    #region Coil
    static partial void ModifyHuntersCoilPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Flank;
        setting.SkipComboCheck = true;
        setting.NeedsHighlight = true;
    }

    static partial void ModifySwiftskinsCoilPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Rear;
        setting.SkipComboCheck = true;
        setting.NeedsHighlight = true;
    }
    #endregion
    #endregion

    #region AoE

    #region 1
    static partial void ModifySteelMawPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyDreadMawPvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.NoxiousGnash];
        setting.CreateConfig = () => new()
        {
            StatusGcdCount = 8,
            AoeCount = 2,
        };
    }
    #endregion

    #region 2
    static partial void ModifyHuntersBitePvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.DreadMawPvE];
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifySwiftskinsBitePvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.DreadMawPvE];
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }
    #endregion

    #region 3
    static partial void ModifyJaggedMawPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.SwiftskinsBitePvE];
        setting.StatusProvide = [StatusID.GrimskinsVenom];
    }

    static partial void ModifyBloodiedMawPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.SwiftskinsBitePvE];
        setting.StatusProvide = [StatusID.GrimhuntersVenom];
    }
    #endregion

    #region Coil
    static partial void ModifyPitOfDreadPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyHuntersDenPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
        setting.SkipComboCheck = true;
        setting.NeedsHighlight = true;
    }

    static partial void ModifySwiftskinsDenPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
        setting.SkipComboCheck = true;
        setting.NeedsHighlight = true;
    }
    #endregion
    #endregion

    #region Continue
    static partial void ModifySerpentsTailPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => false;
    }

    private static void SerpentsTail(ref ActionSetting setting, ActionID action)
    {
        setting.ActionCheck = () => ActionID.SerpentsTailPvE.AdjustId() == action;

    }

    static partial void ModifyDeathRattlePvE(ref ActionSetting setting)
    {
        SerpentsTail(ref setting, ActionID.DeathRattlePvE);
    }

    static partial void ModifyLastLashPvE(ref ActionSetting setting)
    {
        SerpentsTail(ref setting, ActionID.LastLashPvE);
    }

    static partial void ModifyFirstLegacyPvE(ref ActionSetting setting)
    {
        SerpentsTail(ref setting, ActionID.FirstLegacyPvE);
    }

    static partial void ModifySecondLegacyPvE(ref ActionSetting setting)
    {
        SerpentsTail(ref setting, ActionID.SecondLegacyPvE);
    }

    static partial void ModifyThirdLegacyPvE(ref ActionSetting setting)
    {
        SerpentsTail(ref setting, ActionID.ThirdLegacyPvE);
    }

    static partial void ModifyFourthLegacyPvE(ref ActionSetting setting)
    {
        SerpentsTail(ref setting, ActionID.FourthLegacyPvE);
    }

    static partial void ModifyTwinfangPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => false;
    }

    static partial void ModifyTwinfangBitePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.HuntersVenom];
    }

    static partial void ModifyTwinfangThreshPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.FellhuntersVenom];
    }

    static partial void ModifyUncoiledTwinfangPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.PoisedForTwinfang];
    }

    static partial void ModifyTwinbloodPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => false;
    }

    static partial void ModifyTwinbloodBitePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.SwiftskinsVenom];
    }

    static partial void ModifyTwinbloodThreshPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.FellskinsVenom];
    }

    static partial void ModifyUncoiledTwinbloodPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.PoisedForTwinblood];
    }
    #endregion

    #region Reawaken
    static partial void ModifyReawakenPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => SerpentOffering >= 50 || Player.HasStatus(true, StatusID.ReadyToReawaken);
    }

    private static void Generation(ref ActionSetting setting)
    {
        setting.ActionCheck = () => AnguineTribute > 0;
        setting.StatusNeed = [StatusID.Reawakened];
        setting.NeedsHighlight = true;
    }

    static partial void ModifyFirstGenerationPvE(ref ActionSetting setting)
    {
        Generation(ref setting);
    }

    static partial void ModifySecondGenerationPvE(ref ActionSetting setting)
    {
        Generation(ref setting);
    }

    static partial void ModifyThirdGenerationPvE(ref ActionSetting setting)
    {
        Generation(ref setting);
    }

    static partial void ModifyFourthGenerationPvE(ref ActionSetting setting)
    {
        Generation(ref setting);
    }

    static partial void ModifyOuroborosPvE(ref ActionSetting setting)
    {
        Generation(ref setting);
    }
    #endregion

    static partial void ModifyWrithingSnapPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MeleeRange;
    }

    static partial void ModifySlitherPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyUncoiledFuryPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => RattlingCoilStacks > 0;
    }

    static partial void ModifySerpentsIrePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat;
    }

    /// <inheritdoc/>
    protected override bool MoveForwardAbility(out IAction? act)
    {
        if (SlitherPvE.CanUse(out act, usedUp: true)) return true;
        return base.MoveForwardAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (WorldswallowerPvP.CanUse(out act, skipAoeCheck: true)) return true;
        return base.LimitBreakPvPGCD(out act);
    }

    /// <inheritdoc/>
    protected override bool DefenseAreaAbility(out IAction? act)
    {
        if (FeintPvE.CanUse(out act)) return true;
        return base.DefenseAreaAbility(out act);
    }
}