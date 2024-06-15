namespace RotationSolver.Basic.Configuration.Condition;

[Description("Condition Set")]
internal class ConditionSet : DelayCondition
{
    public List<ICondition> Conditions { get; set; } = [];

    public LogicalType Type;

    protected override bool IsTrueInside(ICustomRotation rotation)
    {
        if (Conditions.Count == 0) return false;

        return Type switch
        {
            LogicalType.All => Conditions.All(c => c.IsTrue(rotation) ?? false),
            LogicalType.Any => Conditions.Any(c => c.IsTrue(rotation) ?? false),
            _ => false,
        };
    }
}