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
            StatusGcdCount = 4,
        };
    }
    #endregion

    #region 2
    static partial void ModifyHuntersStingPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.DreadFangsPvE];
        setting.StatusProvide = [StatusID.HuntersInstinct];
    }

    static partial void ModifySwiftskinsStingPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.DreadFangsPvE];
        setting.StatusProvide = [StatusID.Swiftscaled];
    }

    #endregion

    #region 3
    static partial void ModifyFlankstingStrikePvE(ref ActionSetting setting)
    {
        setting.NeedsHighlight = true;
    }

    static partial void ModifyFlanksbaneFangPvE(ref ActionSetting setting)
    {
        setting.NeedsHighlight = true;
    }

    static partial void ModifyHindstingStrikePvE(ref ActionSetting setting)
    {
        setting.NeedsHighlight = true;
    }

    static partial void ModifyHindsbaneFangPvE(ref ActionSetting setting)
    {
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
            StatusGcdCount = 4,
            AoeCount = 2,
        };
    }
    #endregion

    #region 2
    static partial void ModifyHuntersBitePvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.DreadMawPvE];
        setting.StatusProvide = [StatusID.HuntersInstinct];
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifySwiftskinsBitePvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.DreadMawPvE];
        setting.StatusProvide = [StatusID.Swiftscaled];
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }
    #endregion

    static partial void ModifyPitOfDreadPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
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


    static partial void ModifyJaggedMawPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.GrimskinsVenom];
    }

    static partial void ModifyBloodiedMawPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.GrimhuntersVenom];
    }

    static partial void ModifyHuntersCoilPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.HuntersVenom];
    }

    static partial void ModifySwiftskinsCoilPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.SwiftskinsVenom];
    }

    static partial void ModifyHuntersDenPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.FellhuntersVenom];
    }

    static partial void ModifySwiftskinsDenPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.FellskinsVenom];
    }

    static partial void ModifyTwinfangBitePvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.SwiftskinsVenom];
    }

    static partial void ModifyTwinbloodBitePvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.HuntersVenom];
    }

    static partial void ModifyTwinfangThreshPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.FellskinsVenom];
    }

    static partial void ModifyTwinbloodThreshPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.FellhuntersVenom];
    }

    static partial void ModifyUncoiledFuryPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.PoisedForTwinfang];
    }

    static partial void ModifySerpentsIrePvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.ReadyToReawaken];
    }

    static partial void ModifyReawakenPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => SerpentOffering >= 50 || Player.HasStatus(true, StatusID.ReadyToReawaken);
    }

    static partial void ModifyFirstGenerationPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Reawakened];
    }

    static partial void ModifySecondGenerationPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Reawakened];
    }

    static partial void ModifyThirdGenerationPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Reawakened];
    }

    static partial void ModifyFourthGenerationPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Reawakened];
    }

    static partial void ModifyUncoiledTwinfangPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.PoisedForTwinfang];
        setting.StatusProvide = [StatusID.PoisedForTwinblood];
    }

    static partial void ModifyUncoiledTwinbloodPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Reawakened];
    }

    static partial void ModifyOuroborosPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Reawakened];
    }

    static partial void ModifyFirstLegacyPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Reawakened];
    }

    static partial void ModifySecondLegacyPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Reawakened];
    }

    static partial void ModifyThirdLegacyPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Reawakened];
    }

    static partial void ModifyFourthLegacyPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Reawakened];
    }
}
