﻿using static RotationSolver.Basic.CombatData;

using RotationSolver.Basic.Traits;

namespace RotationSolver.Basic.Rotations;

partial class CustomRotation
{
    #region Role Actions
    static partial void ModifyTrueNorthPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.TrueNorth];
    }

    static partial void ModifyAddlePvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.Addle];
        setting.StatusFromSelf = false;
    }

    static partial void ModifySwiftcastPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = StatusHelper.SwiftcastStatus;
    }

    static partial void ModifyEsunaPvE(ref ActionSetting setting)
    {
        setting.TargetType = TargetType.Dispel;
    }

    static partial void ModifyLucidDreamingPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Player.CurrentMp < 8000 && InCombat;
    }

    static partial void ModifySecondWindPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Player?.GetHealthRatio() < Service.Config.HealthSingleAbility && InCombat;
    }

    static partial void ModifyRampartPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = StatusHelper.RampartStatus;
    }

    static partial void ModifyBloodbathPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Player?.GetHealthRatio() < Service.Config.HealthSingleAbility && InCombat && HasHostilesInRange;
    }

    static partial void ModifyFeintPvE(ref ActionSetting setting)
    {
        setting.StatusFromSelf = false;
        setting.TargetStatusProvide = [StatusID.Feint];
    }

    static partial void ModifyLowBlowPvE(ref ActionSetting setting)
    {
        setting.CanTarget = o =>
        {
            if (o is not IBattleChara b) return false;

            if (b.IsBossFromIcon() || IsMoving || b.CastActionId == 0) return false;

            if (!b.IsCastInterruptible || ActionID.InterjectPvE.IsCoolingDown()) return true;
            return false;
        };
    }

    static partial void ModifyPelotonPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () =>
        {
            if (!NotInCombatDelay) return false;
            var players = PartyMembers.GetObjectInRadius(20);
            if (players.Any(ObjectHelper.InCombat)) return false;
            return players.Any(p => p.WillStatusEnd(3, false, StatusID.Peloton));
        };
    }

    static partial void ModifySprintPvE(ref ActionSetting setting)
    {
        setting.StatusPenalty = [StatusID.SprintPenalty];
        setting.StatusProvide = [StatusID.Dualcast];
    }
    #endregion

    #region PvP

    static partial void ModifyStandardissueElixirPvP(ref ActionSetting setting)
    {
        setting.ActionCheck = () => !HasHostilesInMaxRange
            && (Player.CurrentMp <= Player.MaxMp / 3 || Player.CurrentHp <= Player.MaxHp / 3)
            && !IsLastAction(ActionID.StandardissueElixirPvP);
        setting.IsFriendly = true;
    }

    static partial void ModifyRecuperatePvP(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Player.MaxHp - Player.CurrentHp > 15000;
        setting.IsFriendly = true;
    }

    static partial void ModifyPurifyPvP(ref ActionSetting setting)
    {
        setting.TargetType = TargetType.Dispel;
        setting.IsFriendly = true;
    }

    static partial void ModifySprintPvP(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Sprint_1342];
        setting.IsFriendly = true;
    }

    #endregion

    private protected virtual IBaseAction? Raise => null;
    private protected virtual IBaseActionSet? TankStance => null;
    private protected virtual IBaseAction? LimitBreak1 => null;
    private protected virtual IBaseAction? LimitBreak2 => null;
    private protected virtual IBaseAction? LimitBreak3 => null;

    /// <summary>
    /// All actions of this rotation.
    /// </summary>
    public virtual IAction[] AllActions => 
    [
        .. AllBaseActions.Where(i => i.Action.IsInJob()),
        .. Medicines.Where(i => i.HasIt),
        .. MpPotions.Where(i => i.HasIt),
        .. HpPotions.Where(i => i.HasIt),
        .. AllItems.Where(i => i.HasIt),
    ];

    /// <summary>
    /// All traits of this action.
    /// </summary>
    public virtual IBaseTrait[] AllTraits { get; } = [];
}
