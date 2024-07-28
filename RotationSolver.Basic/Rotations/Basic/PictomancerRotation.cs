using FFXIVClientStructs.FFXIV.Client.Game;
using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class PictomancerRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Intelligence;

    private static void Combo1(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Aetherhues];
    }

    private static void Combo2(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Aetherhues];
        setting.StatusProvide = [StatusID.AetherhuesIi];
    }

    private static void Combo3(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.AetherhuesIi];
    }

    #region Basic Combo
    static partial void ModifyFireInRedPvE(ref ActionSetting setting)
        => Combo1(ref setting);

    static partial void ModifyAeroInGreenPvE(ref ActionSetting setting)
        => Combo2(ref setting);

    static partial void ModifyWaterInBluePvE(ref ActionSetting setting)
        => Combo3(ref setting);

    static partial void ModifyFireIiInRedPvE(ref ActionSetting setting)
    {
        Combo1(ref setting);
        setting.CreateConfig = () => new()
        {
            AoeCount = 4,
        };
    }

    static partial void ModifyAeroIiInGreenPvE(ref ActionSetting setting)
    {
        Combo2(ref setting);
        setting.CreateConfig = () => new()
        {
            AoeCount = 4,
        };
    }

    static partial void ModifyWaterIiInBluePvE(ref ActionSetting setting)
    {
        Combo3(ref setting);
        setting.CreateConfig = () => new()
        {
            AoeCount = 4,
        };
    }
    #endregion

    private static void Palatte(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Player.HasStatus(true, StatusID.SubtractivePalette, StatusID.Hyperphantasia);
    }

    #region Palette
    static partial void ModifyBlizzardInCyanPvE(ref ActionSetting setting)
    {
        Palatte(ref setting);
        Combo1(ref setting);
    }

    static partial void ModifyStoneInYellowPvE(ref ActionSetting setting)
    {
        Palatte(ref setting);
        Combo2(ref setting);
    }

    static partial void ModifyThunderInMagentaPvE(ref ActionSetting setting)
    {
        Palatte(ref setting);
        Combo3(ref setting);
    }

    static partial void ModifyBlizzardIiInCyanPvE(ref ActionSetting setting)
    {
        Palatte(ref setting);
        Combo1(ref setting);
        setting.CreateConfig = () => new()
        {
            AoeCount = 4,
        };
    }

    static partial void ModifyStoneIiInYellowPvE(ref ActionSetting setting)
    {
        Palatte(ref setting);
        Combo2(ref setting);
        setting.CreateConfig = () => new()
        {
            AoeCount = 4,
        };
    }

    static partial void ModifyThunderIiInMagentaPvE(ref ActionSetting setting)
    {
        Palatte(ref setting);
        Combo3(ref setting);
        setting.CreateConfig = () => new()
        {
            AoeCount = 4,
        };
    }
    #endregion

    #region Creature
    #region Creature Motif
    static partial void ModifyCreatureMotifPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => false;
    }

    private static void CreatureMotif(ref ActionSetting setting, ActionID actionId)
    {
        setting.ActionCheck = () => !CreatureMotifDrawn && ActionID.CreatureMotifPvE.AdjustId() == actionId;
    }

    static partial void ModifyPomMotifPvE(ref ActionSetting setting)
    {
        CreatureMotif(ref setting, ActionID.PomMotifPvE);
    }

    static partial void ModifyWingMotifPvE(ref ActionSetting setting)
    {
        CreatureMotif(ref setting, ActionID.WingMotifPvE);
    }

    static partial void ModifyClawMotifPvE(ref ActionSetting setting)
    {
        CreatureMotif(ref setting, ActionID.ClawMotifPvE);
    }

    static partial void ModifyMawMotifPvE(ref ActionSetting setting)
    {
        CreatureMotif(ref setting, ActionID.MawMotifPvE);
    }
    #endregion

    #region Living Muse
    static partial void ModifyLivingMusePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => false;
    }

    private static void LivingMuse(ref ActionSetting setting, ActionID actionId)
    {
        setting.ActionCheck = () => CreatureMotifDrawn && ActionID.LivingMusePvE.AdjustId() == actionId;
    }

    static partial void ModifyPomMusePvE(ref ActionSetting setting)
    {
        LivingMuse(ref setting, ActionID.PomMusePvE);
    }

    static partial void ModifyWingedMusePvE(ref ActionSetting setting)
    {
        LivingMuse(ref setting, ActionID.WingedMusePvE);
    }

    static partial void ModifyClawedMusePvE(ref ActionSetting setting)
    {
        LivingMuse(ref setting, ActionID.ClawedMusePvE);
    }

    static partial void ModifyFangedMusePvE(ref ActionSetting setting)
    {
        LivingMuse(ref setting, ActionID.FangedMusePvE);
    }
    #endregion

    #region Portrait
    static partial void ModifyMogOfTheAgesPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => MooglePortraitReady;
    }

    static partial void ModifyRetributionOfTheMadeenPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => MadeenPortraitReady;
    }
    #endregion
    #endregion

    #region Weapon
    #region Weapon Motif
    static partial void ModifyWeaponMotifPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => false;
    }

    private static void WeaponMotif(ref ActionSetting setting, ActionID actionId)
    {
        setting.ActionCheck = () => !WeaponMotifDrawn && ActionID.WeaponMotifPvE.AdjustId() == actionId;
    }

    static partial void ModifyHammerMotifPvE(ref ActionSetting setting)
    {
        WeaponMotif(ref setting, ActionID.HammerMotifPvE);
    }
    #endregion

    #region Steel Muse
    static partial void ModifySteelMusePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => false;
    }

    static void SteelMuse(ref ActionSetting setting, ActionID actionId)
    {
        setting.ActionCheck = () => WeaponMotifDrawn && ActionID.SteelMusePvE.AdjustId() == actionId;
    }

    static partial void ModifyStrikingMusePvE(ref ActionSetting setting)
    {
        SteelMuse(ref setting, ActionID.StrikingMusePvE);
        setting.StatusProvide = [StatusID.HammerTime];
    }
    #endregion

    #region Hammer
    static partial void ModifyHammerStampPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.HammerTime];
    }

    static partial void ModifyHammerBrushPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.HammerTime];
        setting.ComboIds = [ActionID.HammerStampPvE];
    }

    static partial void ModifyPolishingHammerPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.HammerTime];
        setting.ComboIds = [ActionID.PolishingHammerPvE];
    }
    #endregion
    #endregion

    #region Landscape
    #region Landscape Motif
    static partial void ModifyLandscapeMotifPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => false;
    }

    private static void LandscapeMotif(ref ActionSetting setting, ActionID actionId)
    {
        setting.ActionCheck = () => !LandscapeMotifDrawn && ActionID.LandscapeMotifPvE.AdjustId() == actionId;
    }

    static partial void ModifyStarrySkyMotifPvE(ref ActionSetting setting)
    {
        LandscapeMotif(ref setting, ActionID.StarrySkyMotifPvE);
    }
    #endregion

    #region Scenic Muse
    static partial void ModifyScenicMusePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => false;
    }

    private static void ScenicMuse(ref ActionSetting setting, ActionID actionId)
    {
        setting.ActionCheck = () => LandscapeMotifDrawn && ActionID.ScenicMusePvE.AdjustId() == actionId;
    }

    static partial void ModifyStarryMusePvE(ref ActionSetting setting)
    {
        ScenicMuse(ref setting, ActionID.StarryMusePvE);
    }
    #endregion
    #endregion


    static partial void ModifyHolyInWhitePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Paint > 0;
    }

    static partial void ModifyCometInBlackPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.MonochromeTones];
    }

    static partial void ModifyStarPrismPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Starstruck];
    }

    static partial void ModifySubtractivePalettePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => PalleteGauge >= 50 || Player.HasStatus(true, StatusID.SubtractiveSpectrum);
        setting.StatusProvide = [StatusID.SubtractivePalette, StatusID.MonochromeTones];
    }

    static partial void ModifyTemperaGrassaPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.TemperaCoat];
    }

    #region Basic Actions
    /// <inheritdoc/>
    protected override bool DefenseAreaAbility(out IAction? act)
    {
        if (TemperaGrassaPvE.CanUse(out act)) return true;
        return base.DefenseAreaAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool DefenseSingleAbility(out IAction? act)
    {
        if (TemperaCoatPvE.CanUse(out act)) return true;
        return base.DefenseSingleAbility(out act);
    }
    #endregion

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (AdventOfChocobastionPvP.CanUse(out act, skipAoeCheck: true)) return true;
        return base.LimitBreakPvPGCD(out act);
    }
}
