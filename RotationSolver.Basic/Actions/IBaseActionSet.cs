namespace RotationSolver.Basic.Actions;

/// <summary>
/// The <see cref="Lumina.Excel.GeneratedSheets.ReplaceAction"/>
/// </summary>
public interface IBaseActionSet : ICanUse
{
    /// <summary>
    /// The chosen Action. Please get it after using <see cref="ICanUse.CanUse(out IAction, CanUseOption, byte)"/>
    /// </summary>
    IBaseAction? ChosenAction { get; }
}
