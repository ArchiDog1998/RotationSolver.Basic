

namespace RotationSolver.Basic.Actions;

internal class BaseReplaceAction(Func<IBaseAction[]> getActions) : IBaseAction
{
    readonly Lazy<IBaseAction[]> _getActions = new (getActions);
    private IBaseAction? _choosenAction = null;

    public Lumina.Excel.GeneratedSheets.Action Action => throw new NotImplementedException();

    public TargetResult Target { get => _choosenAction.Target; set => _choosenAction.Target = value; }

    public TargetResult? PreviewTarget => _choosenAction.PreviewTarget;

    public ActionTargetInfo TargetInfo => _choosenAction.TargetInfo;

    public ActionBasicInfo Info => _choosenAction.Info;

    public ActionCooldownInfo CD => _choosenAction.CD;

    public ActionSetting Setting { get => _choosenAction.Setting; set => _choosenAction.Setting = value; }

    public uint ID => _choosenAction.ID;

    public float AnimationLockTime => _choosenAction.AnimationLockTime;

    public uint SortKey => _choosenAction.SortKey;

    public bool IsInCD { get => _choosenAction.IsInCD; set => _choosenAction.IsInCD = value; }

    public uint IconID => _choosenAction.IconID;

    public string Name => _choosenAction.Name;

    public string Description => _choosenAction.Description;

    public bool IsEnabled { get => _choosenAction.IsEnabled; set => _choosenAction.IsEnabled = value; }

    public bool EnoughLevel => _choosenAction.EnoughLevel;

    ICooldown IAction.CD => ((IAction)_choosenAction).CD;

    WhyActionCantUse IBaseAction.WhyCant => throw new NotImplementedException();

    ActionConfig IBaseAction.Config => throw new NotImplementedException();

    byte IEnoughLevel.Level => throw new NotImplementedException();

    public bool CanUse(out IAction act, bool skipStatusProvideCheck = false, bool skipComboCheck = false, bool skipCastingCheck = false, bool usedUp = false, bool onLastAbility = false, bool skipClippingCheck = false, bool skipAoeCheck = false, byte gcdCountForAbility = 0)
    {
        foreach (var action in _getActions.Value)
        {
            if(action.CanUse(out act,skipStatusProvideCheck, skipComboCheck, skipCastingCheck, usedUp, onLastAbility, skipClippingCheck, skipAoeCheck, gcdCountForAbility))
            {
                _choosenAction = action;
                return true;
            }
        }
        _choosenAction = null;
        act = null!;
        return false;
    }

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

    public bool Use() => _choosenAction?.Use() ?? false;
}
