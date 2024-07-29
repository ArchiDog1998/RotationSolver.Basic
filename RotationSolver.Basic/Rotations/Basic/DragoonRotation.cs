using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class DragoonRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Strength;

    static partial void ModifyHighJumpPvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    //static partial void ModifyVorpalThrustPvE(ref ActionSetting setting)
    //{
    //    setting.ComboIds = [ActionID.RaidenThrustPvE];
    //}

    //static partial void ModifyDisembowelPvE(ref ActionSetting setting)
    //{
    //    setting.ComboIds = [ActionID.RaidenThrustPvE];
    //}

    static partial void ModifyChaoticSpringPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Rear;
    }

    static partial void ModifyChaosThrustPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Rear;
    }

    static partial void ModifyFangAndClawPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Flank;
        setting.StatusNeed = [StatusID.FangAndClawBared];
    }

    static partial void ModifyWheelingThrustPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Rear;
        setting.StatusNeed = [StatusID.WheelInMotion];
    }

    static partial void ModifyPiercingTalonPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MeleeRange;
    }

    static partial void ModifyJumpPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.DiveReady];
    }

    static partial void ModifyHighJumpPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.DiveReady];
    }

    static partial void ModifyMirageDivePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.DiveReady];
    }

    //static partial void ModifySonicThrustPvE(ref ActionSetting setting)
    //{
    //    setting.ComboIds = [ActionID.DraconianFuryPvE];
    //}

    static partial void ModifyNastrondPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => JobGauge.IsLOTDActive;
    }

    static partial void ModifyStardiverPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => JobGauge.IsLOTDActive;
    }

    static partial void ModifyWyrmwindThrustPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => FirstmindsFocusCount == 2;
    }

    static partial void ModifyLifeSurgePvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.LifeSurge];
        setting.ActionCheck = () => !IsLastAbility(ActionID.LifeSurgePvE);
    }

    static partial void ModifyLanceChargePvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    //static partial void ModifyDragonSightPvE(ref ActionSetting setting)
    //{
    //    setting.TargetType = TargetType.Melee;
    //    setting.CanTarget = b => b != Player;
    //    setting.CreateConfig = () => new()
    //    {
    //        TimeToKill = 10,
    //    };
    //}

    static partial void ModifyBattleLitanyPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    /// <inheritdoc/>
    protected override bool DefenseAreaAbility(out IAction? act)
    {
        if (FeintPvE.CanUse(out act)) return true;
        return false;
    }

    /// <inheritdoc/>
    protected override bool MoveBackAbility(out IAction? act)
    {
        if (ElusiveJumpPvE.CanUse(out act, skipClippingCheck: true)) return true;
        return base.MoveBackAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (SkyHighPvP.CanUse(out act)) return true;
        if (SkyShatterPvP.CanUse(out act)) return true;
        if (SkyShatterPvP_29499.CanUse(out act)) return true;
        return base.LimitBreakPvPGCD(out act);
    }
}