namespace RotationSolver.Basic.Actions;

internal class BaseActionSet(Func<IEnumerable<ICanUse>> getActions, ICustomRotation? rotation) : IBaseActionSet
{
    private readonly Dictionary<ICanUse, ICanUse> _actionReplace = [];

    private IBaseActionSet[] ActionSets
    {
        get
        {
            if (rotation is null) return [];
            return rotation.GetType().GetRuntimeProperties()
                .Where(p => p.PropertyType == typeof(IBaseActionSet))
                .Select(p => (IBaseActionSet)p.GetValue(rotation)!)
                .Where(a => a.IsReplace)
                .ToArray();
        }
    }

    public bool IsReplace => rotation is null;
    public IEnumerable<ICanUse> Actions => getActions();
    public IBaseAction? ChosenAction { get; private set; }

    public bool CanUse(out IAction act, bool skipStatusProvideCheck = false, bool skipComboCheck = false, bool skipCastingCheck = false, bool usedUp = false, bool onLastAbility = false, bool skipClippingCheck = false, bool skipAoeCheck = false, byte gcdCountForAbility = 0)
    {
        byte level = byte.MaxValue;
        foreach (var action in Actions)
        {
            if (!_actionReplace.TryGetValue(action, out var replacedAction))
            {
                _actionReplace[action] = replacedAction = ActionSets.FirstOrDefault(s => s.Actions.Contains(action)) ?? action;
            }
            if (replacedAction.CanUse(out act,skipStatusProvideCheck, skipComboCheck, skipCastingCheck, usedUp, onLastAbility, skipClippingCheck, skipAoeCheck, gcdCountForAbility)
                && act is IBaseAction baseAction)
            {
                ChosenAction = baseAction;
                return true;
            }
            if (IsReplace && act.EnoughLevel && level < act.Level)
            {
                break;
            }
            level = act.Level;
        }
        ChosenAction = null;
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

    public bool Use() => ChosenAction?.Use() ?? false;
}
