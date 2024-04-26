using Dalamud.Utility;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using RotationSolver.Basic.Configuration;
using XIVConfigUI;
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

    /// <inheritdoc/>
    public ActionConfig Config
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
        if (!DownloadHelper.IsSupporter)
        {
            if (DataCenter.IsInHighEndDuty)
            {
                Svc.Toasts.ShowError(string.Format(UiString.CantUseInHighEnd.Local()));
                return false;
            }
            if (DataCenter.TerritoryContentType is TerritoryContentType.DeepDungeons)
            {
                Svc.Toasts.ShowError(string.Format(UiString.CantUseInDeepDungeons.Local()));
                return false;
            }
            else if (DataCenter.TerritoryContentType is TerritoryContentType.Eureka)
            {
                Svc.Toasts.ShowError(string.Format(UiString.CantUseInEureka.Local()));
                return false;
            }
            else if (DataCenter.TerritoryContentType is (TerritoryContentType)29)
            {
                Svc.Toasts.ShowError(string.Format(UiString.CantUseInBozja.Local()));
                return false;
            }

            if (!Service.Config.IWannaBeSaidHello)
            {
                var uiName = Service.Config.GetType().GetRuntimeProperty(nameof(Configs.IWannaBeSaidHello))?.LocalUIName() ?? string.Empty;

                if (DataCenter.IsPvP)
                {
                    Svc.Toasts.ShowError(string.Format(UiString.CantUseInPvP.Local(), uiName));
                    return false;
                }
                if (Player.Object.Level >= 90)
                {
                    Svc.Toasts.ShowError(string.Format(UiString.CantUseAtTopLevel.Local(), uiName));
                    return false;
                }
            }
        }

        var target = Target;

        var adjustId = AdjustedID;
        if (TargetInfo.IsTargetArea)
        {
            if (adjustId != ID) return false;
            if (!target.Position.HasValue) return false;

            var loc = (FFXIVClientStructs.FFXIV.Common.Math.Vector3)target.Position;

            return ActionManager.Instance()->UseActionLocation(ActionType.Action, adjustId, Player.Object.ObjectId, &loc);
        }
        else if (Svc.Objects.SearchById(target.Target?.ObjectId 
            ?? Player.Object?.ObjectId ?? GameObject.InvalidGameObjectId) == null)
        {
            return false;
        }
        else
        {
            return ActionManager.Instance()->UseAction(ActionType.Action, adjustId, target.Target?.ObjectId ?? GameObject.InvalidGameObjectId);
        }
    }

    /// <inheritdoc/>
    public override string ToString() => Name;

    bool IBaseAction.CanUse(out IAction act, bool skipStatusProvideCheck, bool skipComboCheck, bool skipCastingCheck, bool usedUp, bool onLastAbility, bool skipClippingCheck, bool skipAoeCheck, byte gcdCountForAbility)
    {
        throw new NotImplementedException();
    }
}
