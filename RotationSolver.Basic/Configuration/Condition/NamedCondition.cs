using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Condition;

internal class ConditionSetAttribute : ChoicesAttribute
{
    protected override bool Lazy => false;

    protected override Pair[] GetChoices()
    {
        return [..DataCenter.RightSet.NamedConditions.Select(i => i?.Name ?? string.Empty)
            .Where(i => !string.IsNullOrEmpty(i))];
    }
}

[Description("Named Condition")]
internal class NamedCondition : DelayConditionBase
{
    [ConditionSet, UI("Condition")]
    public string ConditionName { get; set; } = string.Empty;
    protected override bool IsTrueInside(ICustomRotation rotation)
    {
        foreach (var item in DataCenter.RightSet.NamedConditions)
        {
            if (item == null) continue;
            if (item.Name != ConditionName) continue;

            return item.Item.IsTrue() ?? false;
        }
        return false;
    }
}
