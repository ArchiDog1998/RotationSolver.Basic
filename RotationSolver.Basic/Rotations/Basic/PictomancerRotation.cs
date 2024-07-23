using FFXIVClientStructs.FFXIV.Client.Game;
using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
partial class PictomancerRotation
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
        => Combo1(ref setting);

    static partial void ModifyAeroIiInGreenPvE(ref ActionSetting setting)
        => Combo2(ref setting);

    static partial void ModifyWaterIiInBluePvE(ref ActionSetting setting)
        => Combo3(ref setting);
    #endregion

    private static void Palatte(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Player.HasStatus(true, StatusID.SubtractivePalette);
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
    }

    static partial void ModifyStoneIiInYellowPvE(ref ActionSetting setting)
    {
        Palatte(ref setting);
        Combo2(ref setting);
    }

    static partial void ModifyThunderIiInMagentaPvE(ref ActionSetting setting)
    {
        Palatte(ref setting);
        Combo3(ref setting);
    }
    #endregion

    #region Creature Motif
    static partial void ModifyCreatureMotifPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => false;
    }

    private static void CreatureMotif(ref ActionSetting setting, ActionID actionId)
    {
        setting.ActionCheck = () => !CreatureMotifDrawn && ActionID.CreatureMotifPvE.GetAdjustedActionId() == actionId;
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

    #region Living
    static partial void ModifyLivingMusePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => false;
    }

    private static void LivingMuse(ref ActionSetting setting, ActionID actionId)
    {
        setting.ActionCheck = () => CreatureMotifDrawn && ActionID.LivingMusePvE.GetAdjustedActionId() == actionId;
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

    static partial void ModifyStarPrismPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Starstruck];
    }

    static partial void ModifySubtractivePalettePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => PalleteGauge >= 50;
        setting.StatusProvide = [StatusID.SubtractivePalette];
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
}
