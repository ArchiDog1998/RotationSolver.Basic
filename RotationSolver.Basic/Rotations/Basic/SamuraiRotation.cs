﻿using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class SamuraiRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Strength;

    /// <summary>
    /// Has <see cref="StatusID.Fugetsu"/>
    /// </summary>
    public static bool HasMoon => Player.HasStatus(true, StatusID.Fugetsu);

    /// <summary>
    /// Has <see cref="StatusID.Fuka"/> 
    /// </summary>
    public static bool HasFlower => Player.HasStatus(true, StatusID.Fuka);

    /// <summary>
    /// 
    /// </summary>
    public static bool IsMoonTimeLessThanFlower
        => Player.StatusTime(true, StatusID.Fugetsu) < Player.StatusTime(true, StatusID.Fuka);

    #region JobGauge
    /// <summary>
    /// 
    /// </summary>
    public static byte SenCount => (byte)((HasGetsu ? 1 : 0) + (HasSetsu ? 1 : 0) + (HasKa ? 1 : 0));
    #endregion

    static partial void ModifyGekkoPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Rear;
    }

    static partial void ModifyKashaPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Flank;
    }

    static partial void ModifyHissatsuSotenPvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyShohaPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => MeditationStacks == 3;
    }

    //static partial void ModifyShohaIiPvE(ref ActionSetting setting)
    //{
    //    setting.ActionCheck = () => MeditationStacks == 3;
    //}

    //static partial void ModifyMangetsuPvE(ref ActionSetting setting)
    //{
    //    setting.ComboIds = [ActionID.FugaPvE, ActionID.FukoPvE];
    //}

    //static partial void ModifyOkaPvE(ref ActionSetting setting)
    //{
    //    setting.ComboIds = [ActionID.FugaPvE, ActionID.FukoPvE];
    //}

    static partial void ModifyOgiNamikiriPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.OgiNamikiriReady];
        setting.ActionCheck = () => !IsMoving;
    }

    static partial void ModifyKaeshiNamikiriPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Kaeshi == Kaeshi.NAMIKIRI;
    }

    static partial void ModifyHiganbanaPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => SenCount == 1;
        setting.TargetStatusProvide = [StatusID.Higanbana];
    }

    static partial void ModifyTenkaGokenPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => SenCount == 2;
        setting.IsFriendly = false;
    }
    static partial void ModifyMidareSetsugekkaPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => SenCount == 3;
    }

    static partial void ModifyKaeshiGokenPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Kaeshi == Kaeshi.GOKEN;
        setting.IsFriendly = false;
    }

    static partial void ModifyKaeshiSetsugekkaPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Kaeshi == Kaeshi.SETSUGEKKA;
    }

    static partial void ModifyEnpiPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MeleeRange;
    }

    static partial void ModifyMeikyoShisuiPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.MeikyoShisui];
        setting.CreateConfig = () => new()
        {
            TimeToKill = 8,
        };
    }

    static partial void ModifyHagakurePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => SenCount > 0;
    }

    static partial void ModifyIkishotenPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.OgiNamikiriReady];
        setting.ActionCheck = () => InCombat;
    }

    static partial void ModifyHissatsuShintenPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Kenki >= 25;
    }

    static partial void ModifyHissatsuGyotenPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
        setting.ActionCheck = () => Kenki >= 10;
    }

    static partial void ModifyHissatsuYatenPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Kenki >= 10;
    }

    static partial void ModifyHissatsuKyutenPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Kenki >= 25;
    }

    static partial void ModifyHissatsuGurenPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Kenki >= 25;
    }

    static partial void ModifyHissatsuSeneiPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Kenki >= 25;
    }

    /// <inheritdoc/>
    protected override bool MoveForwardAbility(out IAction? act)
    {
        if (HissatsuGyotenPvE.CanUse(out act)) return true;
        return base.MoveForwardAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool DefenseAreaAbility(out IAction? act)
    {
        if (FeintPvE.CanUse(out act)) return true;
        return base.DefenseAreaAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool DefenseSingleAbility(out IAction? act)
    {
        if (ThirdEyePvEReplace.CanUse(out act)) return true;
        return base.DefenseSingleAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (ZantetsukenPvP.CanUse(out act)) return true;
        return base.LimitBreakPvPGCD(out act);
    }
}