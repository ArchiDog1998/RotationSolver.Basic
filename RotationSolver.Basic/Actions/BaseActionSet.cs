﻿namespace RotationSolver.Basic.Actions;

internal class BaseActionSet(Func<IEnumerable<IBaseAction>> getActions, bool onlyLevel) : IBaseActionSet
{
    public IBaseAction? ChosenAction { get; private set; }

    public bool CanUse(out IAction act, bool skipStatusProvideCheck = false, bool skipComboCheck = false, bool skipCastingCheck = false, bool usedUp = false, bool onLastAbility = false, bool skipClippingCheck = false, bool skipAoeCheck = false, byte gcdCountForAbility = 0)
    {
        foreach (var action in getActions())
        {
            if (action.CanUse(out act,skipStatusProvideCheck, skipComboCheck, skipCastingCheck, usedUp, onLastAbility, skipClippingCheck, skipAoeCheck, gcdCountForAbility))
            {
                ChosenAction = action;
                return true;
            }
            if (onlyLevel && action.EnoughLevel)
            {
                break;
            }
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
