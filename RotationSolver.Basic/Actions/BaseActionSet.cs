﻿using ECommons.DalamudServices;
using XIVConfigUI;

namespace RotationSolver.Basic.Actions;

internal class BaseActionSet(Func<IEnumerable<ICanUse>> getActions, bool isReplace) : IBaseActionSet
{
    public bool IsReplace => isReplace;
    public IEnumerable<ICanUse> Actions => getActions();
    public IBaseAction? ChosenAction { get; private set; }

    public bool CanUse(out IAction act, bool skipStatusProvideCheck = false, bool skipComboCheck = false, bool skipCastingCheck = false, bool usedUp = false, bool onLastAbility = false, bool skipClippingCheck = false, bool skipAoeCheck = false, byte gcdCountForAbility = 0)
    {
        byte level = 0;
        foreach (var action in Actions)
        {
            if (action.CanUse(out act,skipStatusProvideCheck, skipComboCheck, skipCastingCheck, usedUp, onLastAbility, skipClippingCheck, skipAoeCheck, gcdCountForAbility)
                && act is IBaseAction baseAction)
            {
                if (IsReplace && act.EnoughLevel && level > act.Level)
                {
                    break;
                }
                ChosenAction = baseAction;
                return true;
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
