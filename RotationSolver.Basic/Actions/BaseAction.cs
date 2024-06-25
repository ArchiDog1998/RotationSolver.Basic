﻿using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.Basic.Actions;

/// <summary>
/// The base action for all actions.
/// </summary>
public class BaseAction : IBaseAction
{
    /// <inheritdoc/>
    public TargetResult Target { get; set; } = new(Player.Object, [], null);

    /// <inheritdoc/>
    public TargetResult? PreviewTarget { get; private set; } = null;

    /// <inheritdoc/>
    public Action Action { get; }

    /// <inheritdoc/>
    public ActionTargetInfo TargetInfo { get; }

    /// <inheritdoc/>
    public ActionBasicInfo Info { get; }

    /// <inheritdoc/>
    public ActionCooldownInfo CD { get; }

    ICooldown IAction.CD => CD;

    /// <inheritdoc/>
    public uint ID => Info.ID;

    /// <inheritdoc/>
    public uint AdjustedID => Info.AdjustedID;

    /// <inheritdoc/>
    public float AnimationLockTime => Info.AnimationLockTime;

    /// <inheritdoc/>
    public uint SortKey => CD.CoolDownGroup;

    /// <inheritdoc/>
    public uint IconID => Info.IconID;

    /// <inheritdoc/>
    public string Name => Info.Name;

    /// <inheritdoc/>
    public string Description => string.Empty;

    /// <inheritdoc/>
    public byte Level => Info.Level;

    /// <inheritdoc/>
    public bool IsEnabled 
    { 
        get => Config.IsEnabled; 
        set => Config.IsEnabled = value;
    }

    /// <inheritdoc/>
    public bool IsInCD
    {
        get => Config.IsInCooldown;
        set => Config.IsInCooldown = value;
    }

    /// <inheritdoc/>
    public bool EnoughLevel => Info.EnoughLevel;

    /// <inheritdoc/>
    public ActionSetting Setting { get; set; }

    ActionConfig IBaseAction.Config => Config;
    /// <inheritdoc/>
    internal ActionConfig Config
    {
        get
        {
            if (!Service.Config.RotationActionConfig.TryGetValue(ID, out var value))
            {
                Service.Config.RotationActionConfig[ID] = value 
                    = Setting.CreateConfig?.Invoke() ?? new();
                if (Action.ClassJob.Value?.GetJobRole() == JobRole.Tank)
                {
                    value.AoeCount = 2;
                }
                if (value.TimeToUntargetable == 0)
                {
                    value.TimeToUntargetable = value.TimeToKill;
                }
                if (Setting.TargetStatusProvide != null)
                {
                    value.TimeToKill = 10;
                }
            }
            return value;
        }
    }

    WhyActionCantUse IBaseAction.WhyCant => _whyCant;
    private WhyActionCantUse _whyCant = WhyActionCantUse.None;

    /// <summary>
    /// The default constructor
    /// </summary>
    /// <param name="actionID">action id</param>
    /// <param name="isDutyAction">is this action a duty action</param>
    public BaseAction(ActionID actionID, bool isDutyAction = false)
    {
        Action = Service.GetSheet<Action>().GetRow((uint)actionID)!;
        TargetInfo = new(this);
        Info = new(this, isDutyAction);
        CD = new(this);

        Setting = new();

        IsFriendly();
        Penalty();

        void IsFriendly()
        {
            var a = Action;
            if (a.CanTargetFriendly || a.CanTargetParty)
            {
                Setting.IsFriendly = true;
            }
            else if (a.CanTargetHostile)
            {
                Setting.IsFriendly = false;
            }
            else
            {
                Setting.IsFriendly = TargetInfo.EffectRange > 5;
            }

            //TODO: better target type check. (NoNeed?) 
        }

        void Penalty()
        {
            if (Info.AttackType == AttackType.Magic)
            {
                Setting.TargetStatusPenalty = [.. StatusHelper.MagicResistance,
                .. Setting.TargetStatusPenalty ?? []];
            }
            else if (Info.Aspect != Aspect.Piercing) // Physic
            {
                Setting.TargetStatusPenalty = [.. StatusHelper.PhysicResistancec,
                .. Setting.TargetStatusPenalty ?? []];
            }
            if (TargetInfo.Range >= 20) // Range
            {
                Setting.TargetStatusPenalty = 
                [
                    StatusID.RangedResistance,
                    StatusID.EnergyField,
                    .. Setting.TargetStatusPenalty ?? []
                ];
            }
        }
    }

    /// <inheritdoc/>
    public bool CanUse(out IAction act, bool skipStatusProvideCheck = false, bool skipComboCheck = false, bool skipCastingCheck = false, 
        bool usedUp = false, bool onLastAbility = false, bool skipClippingCheck = false, bool skipAoeCheck = false, byte gcdCountForAbility = 0)
    {
        act = this!;

        if (IBaseAction.ActionPreview)
        {
            skipCastingCheck = skipClippingCheck = true;
        }
        else
        {
            Setting.EndSpecial = IBaseAction.ShouldEndSpecial;
        }
        if (IBaseAction.AllEmpty)
        {
            usedUp = true;
        }
        if (IBaseAction.IgnoreClipping)
        {
            skipClippingCheck = true;
        }

        if (!Info.BasicCheck(skipStatusProvideCheck, skipComboCheck, skipCastingCheck, out var whyCant))
        {
            _whyCant = whyCant;
            return false;
        }

        if (!CD.CooldownCheck(usedUp, onLastAbility, skipClippingCheck, gcdCountForAbility, out whyCant))
        {
            _whyCant = whyCant;
            return false;
        }

        if (Setting.SpecialType is SpecialActionType.MeleeRange
            && IActionHelper.IsLastAction(IActionHelper.MovingActions))
        {
            _whyCant = WhyActionCantUse.NoRangeActionsAfterMovingForMelee;
            return false;
        }

        if (DataCenter.AverageTimeToKill < Config.TimeToKill)
        {
            _whyCant = WhyActionCantUse.TTK;
            return false;
        }
        if (DataCenter.TimeToUntargetable < Config.TimeToUntargetable)
        {
            _whyCant = WhyActionCantUse.TimeToUntargetable;
            return false;
        }

        PreviewTarget = TargetInfo.FindTarget(skipAoeCheck, skipStatusProvideCheck);
        if (PreviewTarget == null)
        {
            _whyCant = WhyActionCantUse.Target;
            return false;
        }
        if (!IBaseAction.ActionPreview)
        {
            Target = PreviewTarget.Value;
        }

        _whyCant = WhyActionCantUse.None;
        return true;
    }

    /// <inheritdoc/>
    public bool CanUse(out IAction act, CanUseOption option, byte gcdCountForAbility = 0)
    {
        return CanUse(out act, 
            option.HasFlag(CanUseOption.SkipStatusProvideCheck),
            option.HasFlag(CanUseOption.SkipComboCheck),
            option.HasFlag(CanUseOption.SkipCastingCheck),
            option.HasFlag(CanUseOption.UsedUp),
            option.HasFlag(CanUseOption.OnLastAbility),
            option.HasFlag(CanUseOption.SkipClippingCheck),
            option.HasFlag(CanUseOption.SkipAoeCheck),
            gcdCountForAbility);
    }

    /// <inheritdoc/>
    public unsafe bool Use()
    {
        var message = Service.InvalidUseString;
        if (!string.IsNullOrEmpty(message))
        {
            message.ShowToastWarning();
            return false;
        }

        var target = Target;

        var adjustId = AdjustedID;
        if (TargetInfo.IsTargetArea)
        {
            if (adjustId != ID) return false;
            if (!target.Position.HasValue) return false;

            var loc = target.Position.Value;

            return ActionManager.Instance()->UseActionLocation(ActionType.Action, adjustId, Player.Object.EntityId, &loc);
        }
        else if (Svc.Objects.SearchById(target.Target?.EntityId 
            ?? Player.Object?.EntityId ?? 0xE000_0000) == null)
        {
            return false;
        }
        else
        {
            return ActionManager.Instance()->UseAction(ActionType.Action, adjustId, target.Target?.EntityId ?? 0xE000_0000);
        }
    }

    /// <inheritdoc/>
    public override string ToString() => Name;
}
