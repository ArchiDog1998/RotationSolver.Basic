﻿using ECommons.ExcelServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using RotationSolver.Basic.Configuration;
using XIVConfigUI;

namespace RotationSolver.Basic.Actions;

/// <summary>
/// The action info for the <see cref="Lumina.Excel.GeneratedSheets.Action"/>.
/// </summary>
public readonly struct ActionBasicInfo
{
    internal static readonly uint[] ActionsNoNeedCasting =
    [
        5,
        (uint)ActionID.PowerfulShotPvP,
        (uint)ActionID.BlastChargePvP,
    ];

    private readonly IBaseAction _action;

    /// <summary>
    /// The name of the action.
    /// </summary>
    public readonly string Name => _action.Action.Name;

    /// <summary>
    /// The ID of the action.
    /// </summary>
    public readonly uint ID => _action.Action.RowId;

    /// <summary>
    /// The icon of the action.
    /// </summary>
    public readonly uint IconID => _action.Action.GetActionIcon();

    /// <summary>
    /// Is the action highlighted.
    /// </summary>
    public unsafe readonly bool IsHighlighted => ((ActionID)ID).IsHighlight();

    /// <summary>
    /// The attack type of this action.
    /// </summary>
    public readonly AttackType AttackType => (AttackType)(_action.Action.AttackType.Value?.RowId ?? byte.MaxValue);

    /// <summary>
    /// The aspect of this action.
    /// </summary>
    public Aspect Aspect { get; }

    /// <summary>
    /// The animation lock time of this action.
    /// </summary>
    public readonly float AnimationLockTime => OtherConfiguration.AnimationLockTime?.TryGetValue(ID, out var time) ?? false ? time : 0.6f;

    /// <summary>
    /// The level of this action.
    /// </summary>
    public readonly byte Level => _action.Action.ClassJobLevel;

    /// <summary>
    /// If this action is enough level to use.
    /// </summary>
    public readonly bool EnoughLevel => Player.Level >= Level;

    /// <summary>
    /// If this action a pvp action.
    /// </summary>
    public readonly bool IsPvP => _action.Action.IsPvP;

    /// <summary>
    /// Casting time.
    /// </summary>
    public readonly unsafe float CastTime => ((ActionID)ID).GetCastTime();

    /// <summary>
    /// Recasting time.
    /// </summary>
    public readonly unsafe float RecastTime => ((ActionID)ID).GetRecastTime();

    /// <summary>
    /// Status that this action provides.
    /// </summary>

    public StatusID[] StatusProvide
    {
        get
        {
            if (!OtherConfiguration.StatusProvide.TryGetValue(ID, out var statusProvide)) statusProvide = [];
            if (_action.Setting.StatusProvide != null)
            {
                statusProvide = [.. statusProvide, .. _action.Setting.StatusProvide];
            }
            return [..statusProvide.ToHashSet()];
        }
    }

    /// <summary>
    /// The status that it provides to the target.
    /// </summary>
    public StatusID[] TargetStatusProvide
    {
        get
        {
            if (!OtherConfiguration.TargetStatusProvide.TryGetValue(ID, out var targetStatusProvide)) targetStatusProvide = [];
            if (_action.Setting.TargetStatusProvide != null)
            {
                targetStatusProvide = [.. targetStatusProvide, .. _action.Setting.TargetStatusProvide];
            }
            return [.. targetStatusProvide.ToHashSet()];
        }
    }

    /// <summary>
    /// The combo actions
    /// </summary>
    public ActionID[] ComboActions
    {
        get
        {
            var comboActions = (_action.Action.ActionCombo?.Row ?? 0) == 0 ? []
                : new ActionID[] { (ActionID)_action.Action.ActionCombo!.Row };

            if (_action.Setting.ComboIds == null) return comboActions;
            else return [.. comboActions, .. _action.Setting.ComboIds];
        }
    }


    /// <summary>
    /// The status that this action needs
    /// </summary>
    public StatusID[] TargetStatusNeed
    {
        get
        {
            IEnumerable<StatusID> statusNeed = [];
            if (_action.Action.SecondaryCostType == 32)
            {
                statusNeed = statusNeed.Append((StatusID)_action.Action.SecondaryCostValue);
            }

            if (_action.Setting.StatusNeed != null)
            {
                statusNeed = statusNeed.Union(_action.Setting.StatusNeed);
            }
            return [.. statusNeed.ToHashSet()];
        }
    }

    /// <summary>
    /// How many mp does this action needs.
    /// </summary>
    public readonly unsafe uint MPNeed
    {
        get
        {
            var mpOver = _action.Setting.MPOverride?.Invoke();
            if (mpOver.HasValue) return mpOver.Value;

            var mp = (uint)ActionManager.GetActionCost(ActionType.Action, ID, 0, 0, 0, 0);
            if (mp < 100) return 0;
            return mp;
        }
    }

    /// <summary>
    /// Is this action on the slot.
    /// </summary>
    public readonly bool IsOnSlot
    {
        get
        {
            if (_action.Action.ClassJob.Row == (uint)Job.BLU)
            {
                return DataCenter.BluSlots.Contains(ID);
            }

            if (IsDutyAction)
            {
                return DataCenter.DutyActions.Contains(ID);
            }

            return IsPvP == DataCenter.Territory?.IsPvpZone;
        }
    }

    /// <summary>
    /// Is this action is a lb action.
    /// </summary>
    public bool IsLimitBreak { get; }

    /// <summary>
    /// Is this action a general gcd.
    /// </summary>
    public bool IsGeneralGCD { get; }

    /// <summary>
    /// Is this action a real gcd.
    /// </summary>
    public bool IsRealGCD { get; }

    /// <summary>
    /// Is this action a duty action.
    /// </summary>
    public bool IsDutyAction { get; }

    /// <summary>
    /// The basic way to create a basic info
    /// </summary>
    /// <param name="action">the action</param>
    /// <param name="isDutyAction">if it is a duty action.</param>
    public ActionBasicInfo(IBaseAction action, bool isDutyAction)
    {
        _action = action;
        IsGeneralGCD = _action.Action.IsGeneralGCD();
        IsRealGCD = _action.Action.IsRealGCD();
        IsLimitBreak = (ActionCate?)_action.Action.ActionCategory?.Value?.RowId
            is ActionCate.LimitBreak or ActionCate.LimitBreak_15;
        IsDutyAction = isDutyAction;
        Aspect = (Aspect)_action.Action.Aspect;
    }

    internal readonly bool BasicCheck(bool skipStatusProvideCheck, bool skipComboCheck, bool skipCastingCheck, out WhyActionCantUse whyCant)
    {
        if (!_action.Config.IsEnabled || !IsOnSlot)
        {
            whyCant = WhyActionCantUse.Disabled;
            return false;
        }

        //Disabled.
        if (DataCenter.DisabledActionSequencer?.Contains(ID) ?? false)
        {
            whyCant = WhyActionCantUse.DisabledSequencer;
            return false;
        }

        if (!EnoughLevel)
        {
            whyCant = WhyActionCantUse.NotEnoughLevel;
            return false;
        }

        if (DataCenter.CurrentMp < MPNeed)
        {
            whyCant = WhyActionCantUse.NoMp;
            return false;
        }

        var player = Player.Object;


        var statusNeed = TargetStatusNeed;
        if (statusNeed.Length != 0)
        {
            if (player.WillStatusEndGCD(0, 0,
                _action.Setting.StatusFromSelf, statusNeed))
            {
                whyCant = WhyActionCantUse.NoStatusNeed;
                return false;
            }
        }

        if (_action.Setting.StatusPenalty != null)
        {
            if (!player.WillStatusEndGCD(0, 0,
                false, _action.Setting.StatusPenalty))
            {
                whyCant = WhyActionCantUse.HasThePenaltyStatus;
                return false;
            }
        }

        if (StatusProvide.Length > 0 && !skipStatusProvideCheck)
        {
            if (!player.WillStatusEndGCD(_action.Config.StatusGcdCount, 0,
                _action.Setting.StatusFromSelf, StatusProvide))
            {
                whyCant = WhyActionCantUse.HasTheStatus;
                return false;
            }
        }

        if (_action.Action.ActionCategory.Row == 15)
        {
            if (CombatData.LimitBreakLevel <= 1)
            {
                whyCant = WhyActionCantUse.LimitBreakPvP;
                return false;
            }
        }

        if (!skipComboCheck && IsGeneralGCD)
        {
            if (!CheckForCombo())
            {
                whyCant = WhyActionCantUse.Combo;
                return false;
            }
        }

        if (_action.Action.IsRoleAction)
        {
            if (!_action.Action.ClassJobCategory.Value?.IsJobInCategory(DataCenter.Job) ?? false)
            {
                whyCant = WhyActionCantUse.JobMeet;
                return false;
            }
        }

        if (_action.Setting.NeedsHighlight && !IsHighlighted)
        {
            whyCant = WhyActionCantUse.Highlight;
            return false;
        }

        //Need casting.
        if (CastTime > 0 && !player.HasStatus(true, 
            [
                StatusID.Swiftcast,
                StatusID.Triplecast,
                StatusID.Dualcast,
            ])
            && !ActionsNoNeedCasting.Contains(ID))
        {
            //No casting.
            if (DataCenter.SpecialType == SpecialCommandType.NoCasting)
            {
                whyCant = WhyActionCantUse.NoCasting;
                return false;
            }

            //Is knocking back.
            if (DateTime.Now > DataCenter.KnockbackStart && DateTime.Now < DataCenter.KnockbackFinished)
            {
                whyCant = WhyActionCantUse.KnockingBack;
                return false;
            }

            if (DataCenter.NoPoslock && DataCenter.IsMoving && !skipCastingCheck)
            {
                whyCant = WhyActionCantUse.Moving;
                return false;
            }
        }

        if (!(_action.Setting.ActionCheck?.Invoke() ?? true))
        {
            whyCant = WhyActionCantUse.ActionCheck;
            return false;
        }

        if (!IBaseAction.ForceEnable && !(_action.RotationCheck?.Invoke() ?? true))
        {
            whyCant = WhyActionCantUse.RotationCheck;
            return false;
        }

        unsafe
        {
            if (ConfigurationHelper.BadStatus.Contains(ActionManager.Instance()->GetActionStatus(ActionType.Action, ID)))
            {
                whyCant = WhyActionCantUse.BadStatus;
                return false;
            }
            if (_action.Ninjutsu == null)
            {
                //No resources... TODO: Maybe MP LB..., etc..... which can be simplify.
                if (ActionManager.Instance()->CheckActionResources(ActionType.Action, ID) != 0)
                {
                    whyCant = WhyActionCantUse.ActionResources;
                    return false;
                }
            }
        }

        whyCant = WhyActionCantUse.None;
        return true;
    }

    private readonly bool CheckForCombo()
    {
        if (_action.Setting.ComboIdsNot != null)
        {
            if (_action.Setting.ComboIdsNot.Contains(DataCenter.LastComboAction)) return false;
        }

        if (_action.Setting.SkipComboCheck) return true;

        var comboActions = ComboActions;

        if (comboActions.Length > 0)
        {
            if (comboActions.Contains(DataCenter.LastComboAction))
            {
                if (DataCenter.ComboTime < DataCenter.WeaponRemain) return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}