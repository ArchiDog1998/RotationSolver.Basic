﻿using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class DarkKnightRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Strength;

    static partial void ModifyPlungePvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyBloodspillerPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Blood >= 50 || !Player.WillStatusEnd(0, true, StatusID.Delirium_1972);
    }

    static partial void ModifyUnmendPvE(ref ActionSetting setting)
    {
        setting.SpecialType =  SpecialActionType.MeleeRange;
        setting.TargetType = TargetType.ProvokeOrOthers;
    }

    static partial void ModifyLivingShadowPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Blood >= 50;
    }

    static partial void ModifyQuietusPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Blood >= 50 || !Player.WillStatusEnd(0, true, StatusID.Delirium_1972);
    }

    static partial void ModifyStalwartSoulPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.SaltedEarth];
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifySaltAndDarknessPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => ActionID.SaltedEarthPvE.AdjustId() == ActionID.SaltAndDarknessPvE;
    }

    static partial void ModifyShadowbringerPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => DarksideTimeRemaining > 0;
    }

    private protected sealed override IBaseActionSet TankStance => GritPvEReplace;

    static partial void ModifyShadowWallPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = StatusHelper.RampartStatus;
        setting.ActionCheck = Player.IsTargetOnSelf;
    }

    static partial void ModifyDarkMindPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = Player.IsTargetOnSelf;
    }

    static partial void ModifyOblationPvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.Oblation];
    }

    static partial void ModifyBloodWeaponPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new ()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyDeliriumPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyUnleashPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    //static partial void ModifyPlungePvE(ref ActionSetting setting)
    //{
    //    setting.SpecialType = SpecialActionType.MovingForward;
    //}

    /// <inheritdoc/>
    protected override bool EmergencyAbility(IAction nextGCD, out IAction? act)
    {
        if (LivingDeadPvE.CanUse(out act) 
            && Player.GetHealthRatio() <= Service.Config.HealthForDyingTanks) return true;
        return base.EmergencyAbility(nextGCD, out act);
    }

    ///// <inheritdoc/>
    //protected override bool MoveForwardAbility(out IAction? act)
    //{
    //    if (PlungePvE.CanUse(out act)) return true;
    //    return false;
    //}

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (EventidePvP.CanUse(out act, skipAoeCheck: true)) return true;
        return false;
    }
}
