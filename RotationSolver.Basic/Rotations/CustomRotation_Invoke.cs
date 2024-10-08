﻿namespace RotationSolver.Basic.Rotations;

partial class CustomRotation
{
    /// <inheritdoc/>
    public bool TryInvoke(out IAction? newAction, out IAction? gcdAction)
    {
        newAction = gcdAction = null;

        if (!IsEnabled)
        {
            return false;
        }

        try
        {
            UpdateInfo();
            DataCenter.RightNowDutyRotation?.UpdateInfo();

            IBaseAction.ActionPreview = true;
            UpdateActions(Role);
            IBaseAction.ActionPreview = false;

            CombatData.CountingOfLastUsing = CombatData.CountingOfCombatTimeUsing = 0;
            newAction = Invoke(out gcdAction);
            if (CombatData.InCombat || CountOfTracking == 0)
            {
                AverageCountOfLastUsing =
                    (AverageCountOfLastUsing * CountOfTracking + CombatData.CountingOfLastUsing)
                    / ++CountOfTracking;
                MaxCountOfLastUsing = Math.Max(MaxCountOfLastUsing, CombatData.CountingOfLastUsing);

                AverageCountOfCombatTimeUsing =
                    (AverageCountOfCombatTimeUsing * (CountOfTracking - 1) + CombatData.CountingOfCombatTimeUsing)
                    / CountOfTracking;
                MaxCountOfCombatTimeUsing = Math.Max(MaxCountOfCombatTimeUsing, CombatData.CountingOfCombatTimeUsing);
            }

            if (!IsValid) IsValid = true;
        }
        catch (Exception? ex)
        {
            WhyNotValid = $"Failed to invoke the next action,please contact to \"{{0}}\".";

            while (ex != null)
            {
                if (!string.IsNullOrEmpty(ex.Message)) WhyNotValid += "\n" + ex.Message;
                if (!string.IsNullOrEmpty(ex.StackTrace)) WhyNotValid += "\n" + ex.StackTrace;
                ex = ex.InnerException;
            }
            IsValid = false;
        }

        return newAction != null;
    }

    private void UpdateActions(JobRole role)
    {
        IBaseAction.TargetOverride = TargetType.Move;
        ActionMoveForwardGCD = MoveForwardGCD(out var act) ? act : null;
        IBaseAction.TargetOverride = null;

        if (!DataCenter.HPNotFull && role == JobRole.Healer)
        {
            ActionHealAreaGCD = ActionHealAreaAbility = ActionHealSingleGCD = ActionHealSingleAbility = null;
        }
        else
        {
            ActionHealAreaGCD = HealAreaGCD(out act) ? act : null;
            ActionHealSingleGCD = HealSingleGCD(out act) ? act : null;

            ActionHealAreaAbility = HealAreaAbility(out act) ? act : null;
            ActionHealSingleAbility = HealSingleAbility(out act) ? act : null;
        }

        IBaseAction.TargetOverride = TargetType.BeAttacked;
        ActionDefenseAreaGCD = DefenseAreaGCD(out act) ? act : null;
        ActionDefenseSingleGCD = DefenseSingleGCD(out act) ? act : null;
        IBaseAction.TargetOverride = null;

        ActionDispelStancePositionalGCD = role switch
        {
            JobRole.Healer => DataCenter.DispelTarget != null && DispelGCD(out act) ? act : null,
            _ => null,
        };
        IBaseAction.TargetOverride = TargetType.Death;

        ActionRaiseShirkGCD = role switch
        {
            JobRole.Healer => DataCenter.DeathTarget != null && RaiseSpell(out act, true) ? act : null,
            _ => null,
        };

        IBaseAction.TargetOverride = TargetType.BeAttacked;
        ActionDefenseAreaAbility = DefenseAreaAbility(out act) ? act : null;
        ActionDefenseSingleAbility = DefenseSingleAbility(out act) ? act : null;
        IBaseAction.TargetOverride = null;

        ActionDispelStancePositionalAbility = role switch
        {
            JobRole.Melee => TrueNorthPvE.CanUse(out act) ? act : null,
            JobRole.Tank => TankStance?.CanUse(out act) ?? false ? act : null,
            _ => null,
        };

        ActionRaiseShirkAbility = role switch
        {
            JobRole.Tank => ShirkPvE.CanUse(out act) ? act : null,
            _ => null,
        };
        ActionAntiKnockbackAbility = AntiKnockback(role, out act) ? act : null;

        IBaseAction.TargetOverride = TargetType.Move;
        var movingTarget = MoveForwardAbility(out act);
        IBaseAction.TargetOverride = null;
        ActionMoveForwardAbility = movingTarget ? act : null;

        //TODO: that is too complex! 
        if (movingTarget && act is IBaseAction a)
        {
            if(a.PreviewTarget.HasValue && a.PreviewTarget.Value.Target != CombatData.Player
                && a.PreviewTarget.Value.Target != null)
            {
                var dir = CombatData.Player.Position - a.PreviewTarget.Value.Position;
                var length = dir?.Length() ?? 0;
                if (length != 0 && dir.HasValue)
                {
                    var d = dir.Value / length;

                    MoveTarget = a.PreviewTarget.Value.Position + d * MathF.Min(length, CombatData.Player.HitboxRadius + a.PreviewTarget.Value.Target.HitboxRadius);
                }
                else
                {
                    MoveTarget = a.PreviewTarget.Value.Position;
                }
            }
            else
            {
                float distance = (ActionID)a.ID switch
                {
                    ActionID.EnAvantPvE => 10,
                    ActionID.AetherialShiftPvE or ActionID.HellsIngressPvE => 15,
                    _ => (float)0,
                };
                if (distance > 0)
                {
                    var dir = new Vector3(MathF.Sin(CombatData.Player.Rotation), 0, MathF.Cos(CombatData.Player.Rotation));
                    MoveTarget = CombatData.Player.Position + dir * distance;
                }
                else
                {
                    MoveTarget = a.PreviewTarget?.Position == a.PreviewTarget?.Target?.Position ? null : a.PreviewTarget?.Position;
                }
            }
        }
        else
        {
            MoveTarget = null;
        }

        ActionMoveBackAbility = MoveBackAbility(out act) ? act : null;
        ActionSpeedAbility = SpeedAbility(out act) ? act : null;
        ActionLimitBreak = UseLimitBreak(out act) ? act : null;
    }

    private IAction? Invoke(out IAction? gcdAction)
    {
        IBaseAction.ShouldEndSpecial = false;
        IBaseAction.IgnoreClipping = true;

        var countDown = Service.CountDownTime;
        if (countDown > 0)
        {
            if (DataCenter.Role is JobRole.Tank)
            {

            }
            else
            {
                IBaseAction.TargetOverride = countDown < 1
                    ? TargetType.Move : TargetType.BeAttacked;
            }

            gcdAction = null;
            return CountDownAction(countDown);
        }
        IBaseAction.TargetOverride = null;

        gcdAction = GCD();
        IBaseAction.IgnoreClipping = false;

        if (gcdAction != null)
        {
            if (DataCenter.WeaponTotal < DataCenter.CastingTotal) return gcdAction;

            if (Ability(gcdAction, out var ability)) return ability;

            return gcdAction;
        }
        else
        {
            IBaseAction.IgnoreClipping = true;
            if (Ability(AddlePvE, out var ability)) return ability;
            IBaseAction.IgnoreClipping = false;

            return null;
        }
    }

    /// <summary>
    /// The action in countdown.
    /// </summary>
    /// <param name="remainTime"></param>
    /// <returns></returns>
    protected virtual IAction? CountDownAction(float remainTime) => null;
}
