﻿using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

internal class ConditionSetAttribute : ChoicesAttribute
{
    protected override bool Lazy => false;

    protected override Pair[] GetChoices()
    {
        return [..Service.Config.NamedTargetingConditions.Select(i => i?.Name ?? string.Empty)
            .Where(i => !string.IsNullOrEmpty(i))];
    }
}

[Description("Named Condition")]

internal class TargetingNamedCondition : TargetingConditionBase
{
    [ConditionSet, UI("Condition")]
    public string ConditionName { get; set; } = string.Empty;

    protected override bool IsTrueInside(IGameObject obj)
    {
        foreach (var item in Service.Config.NamedTargetingConditions)
        {
            if (item == null) continue;
            if (item.Name != ConditionName) continue;

            return item.Item.IsTrue(obj) ?? false;
        }
        return false;
    }
}
