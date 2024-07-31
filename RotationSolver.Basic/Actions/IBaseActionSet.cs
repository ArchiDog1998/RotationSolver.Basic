namespace RotationSolver.Basic.Actions;

/// <summary>
/// </summary>
public interface IBaseActionSet : ICanUse
{
    /// <summary>
    /// The chosen Action. Please get it after using <see cref="ICanUse.CanUse(out IAction, CanUseOption, byte)"/>
    /// </summary>
    IBaseAction? ChosenAction { get; }

    /// <summary>
    /// The actions.
    /// </summary>
    IEnumerable<ICanUse> Actions { get; }
}
