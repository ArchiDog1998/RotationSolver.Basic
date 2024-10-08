﻿using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class NinjaRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Dexterity;

    static partial void ModifyShukuchiPvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }


    static partial void ModifyArmorCrushPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Flank;
        //setting.ActionCheck = () => HutonTimer is > 0 and < 25;
    }

    //static partial void ModifyHuraijinPvE(ref ActionSetting setting)
    //{
    //    setting.ActionCheck = () => HutonEndAfterGCD();
    //}

    static partial void ModifyPhantomKamaitachiPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.PhantomKamaitachiReady];
    }

    static partial void ModifyThrowingDaggerPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MeleeRange;
    }

    static partial void ModifyBhavacakraPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Ninki >= 50;
    }

    static partial void ModifyHellfrogMediumPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Ninki >= 50;
    }

    static partial void ModifyMeisuiPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Suiton];
        setting.ActionCheck = () => Ninki <= 50;
    }

    static partial void ModifyMugPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => JobGauge.Ninki <= 60 && IsLongerThan(10);
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyAeolianEdgePvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Rear;
    }

    static partial void ModifyTrickAttackPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Rear;
        setting.StatusNeed = [StatusID.Suiton, StatusID.Hidden];
        setting.CreateConfig = () => new ActionConfig()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyBunshinPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Ninki >= 50;
    }

    static partial void ModifyTenChiJinPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Kassatsu, StatusID.TenChiJin];
        //setting.ActionCheck = () => HutonTimer > 2;
    }

    static partial void ModifyKassatsuPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Kassatsu, StatusID.TenChiJin];
    }

    static partial void ModifyHutonPvE(ref ActionSetting setting)
    {
        //setting.ActionCheck = () => HutonTimer < 0;
    }

    static partial void ModifyDotonPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Doton];
    }

    static partial void ModifyShukuchiPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    /// <inheritdoc/>
    internal override void Init()
    {
        FumaShurikenPvE.Ninjutsu = [TenPvE];
        KatonPvE.Ninjutsu = [ChiPvE, TenPvE];
        RaitonPvE.Ninjutsu = [TenPvE, ChiPvE];
        HyotonPvE.Ninjutsu = [TenPvE, JinPvE];
        HutonPvE.Ninjutsu = [JinPvE, ChiPvE, TenPvE];
        DotonPvE.Ninjutsu = [JinPvE, TenPvE, ChiPvE];
        SuitonPvE.Ninjutsu = [TenPvE, ChiPvE, JinPvE];
        GokaMekkyakuPvE.Ninjutsu = [ChiPvE, TenPvE];
        HyoshoRanryuPvE.Ninjutsu = [TenPvE, JinPvE];
    }

    static partial void ModifyKatonPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    /// <inheritdoc/>
    protected override bool MoveForwardAbility(out IAction? act)
    {
        if (ShukuchiPvE.CanUse(out act)) return true;
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
        if (ShadeShiftPvE.CanUse(out act)) return true;
        return base.DefenseSingleAbility(out act);
    }

    static partial void ModifySuitonPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Suiton];
    }

    static partial void ModifyFleetingRaijuPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.RaijuReady];
    }

    static partial void ModifyForkedRaijuPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.RaijuReady];
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (SeitonTenchuPvP.CanUse(out act)) return true;
        if (SeitonTenchuPvP_29516.CanUse(out act)) return true;
        return base.LimitBreakPvPGCD(out act);
    }
}