namespace RotationSolver.Basic.Configuration.Condition;

internal interface ICondition
{
    bool? IsTrue(ICustomRotation rotation);
    bool CheckBefore(ICustomRotation rotation);
}