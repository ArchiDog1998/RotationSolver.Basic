using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

internal class ConditionSetAttribute : ChoicesAttribute
{
    protected override bool Lazy => false;

    protected override Pair[] GetChoices()
    {
        return [.. Service.Config.NamedTargetingConditions.Select(i => i.Name)];
    }
}

[Description("Named Condition")]

internal class TargetingNamedCondition : TargetingConditionBase
{
    [ConditionSet, UI("Condition")]
    public string ConditionName { get; set; } = "Not Chosen";

    protected override bool IsTrueInside(GameObject obj)
    {
        foreach (var item in Service.Config.NamedTargetingConditions)
        {
            if (item.Name != ConditionName) continue;

            return item.Item.IsTrue(obj) ?? false;
        }
        return false;
    }
}
