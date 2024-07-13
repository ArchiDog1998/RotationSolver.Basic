namespace RotationSolver.Basic.Data;

/// <summary>
/// The Command of the next action.
/// </summary>
/// <param name="DeadTime">When to stop</param>
/// <param name="Action"></param>
public readonly record struct NextActTime(DateTime DeadTime, NextAct Action);

/// <summary>
/// 
/// </summary>
/// <param name="Act">the action it self.</param>
/// <param name="Type"></param>
/// <param name="Option"></param>
public readonly record struct NextAct(IAction Act, TargetType Type, CanUseOption Option);
