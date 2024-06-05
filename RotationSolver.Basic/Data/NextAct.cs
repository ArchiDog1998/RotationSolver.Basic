namespace RotationSolver.Basic.Data;

/// <summary>
/// The Command of the next action.
/// </summary>
/// <param name="Act">the action it self.</param>
/// <param name="DeadTime">When to stop</param>
/// <param name="Type"></param>
public record NextAct(IAction Act, DateTime DeadTime, TargetType Type);
