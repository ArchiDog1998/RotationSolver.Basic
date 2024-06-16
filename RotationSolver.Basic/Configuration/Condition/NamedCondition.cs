using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Condition;

internal class ConditionSetAttribute : ChoicesAttribute
{
    protected override bool Lazy => false;

    protected override Pair[] GetChoices()
    {
        return [.. DataCenter.RightSet.NamedConditions.Select(i => new Pair(i.Name, i.Name))];
    }
}

[Description("Named Condition")]
internal class NamedCondition : DelayConditionBase
{
    [ConditionSet, UI("Condition")]
    public string ConditionName { get; set; } = "Not Chosen";
    protected override bool IsTrueInside(ICustomRotation rotation)
    {
        foreach (var (Name, Condition) in DataCenter.RightSet.NamedConditions)
        {
            if (Name != ConditionName) continue;

            return Condition.IsTrue() ?? false;
        }
        return false;
    }
}
