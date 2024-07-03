namespace RotationSolver.Basic.Actions;

/// <summary>
/// 
/// </summary>
public interface ICanUse
{
    /// <summary>
    /// Can I use this action.
    /// </summary>
    /// <param name="act">The return action</param>
    /// <param name="skipStatusProvideCheck">Skip Status Provide Check</param>
    /// <param name="skipComboCheck">Skip Combo Check</param>
    /// <param name="skipCastingCheck">Skip Casting and Moving Check</param>
    /// <param name="usedUp">Is it used up all stacks</param>
    /// <param name="onLastAbility">Is it on the last ability</param>
    /// <param name="skipClippingCheck">Skip clipping Check</param>
    /// <param name="skipAoeCheck">Skip aoe Check</param>
    /// <param name="gcdCountForAbility">the gcd count for the ability.</param>
    /// <returns>can I use it</returns>
    bool CanUse(out IAction act, bool skipStatusProvideCheck = false, bool skipComboCheck = false, bool skipCastingCheck = false,
        bool usedUp = false, bool onLastAbility = false, bool skipClippingCheck = false, bool skipAoeCheck = false, byte gcdCountForAbility = 0);

    /// <summary>
    /// Can I use this action.
    /// </summary>
    /// <param name="act">The return action</param>
    /// <param name="option">The options</param>
    /// <param name="gcdCountForAbility">the gcd count for the ability.</param>
    /// <returns>can I use it</returns>
    bool CanUse(out IAction act, CanUseOption option, byte gcdCountForAbility = 0);
}
