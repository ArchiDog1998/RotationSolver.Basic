using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.TerritoryAction;

internal class ActionGroupAction : ITerritoryAction
{
    private class ActouGroupChoicesAttribute : ChoicesAttribute
    {
        protected override Pair[] GetChoices()
        {
            return [.. Service.Config.ActionGroups.Select(i => i.Name)];
        }
    }

    [ActouGroupChoices, UI("Action Group Name")]
    public string ActionGroupName { get; set; } = Service.Config.ActionGroups.FirstOrDefault()?.Name ?? string.Empty;

    [UI("Is On")]
    public bool IsOn { get; set; } = true;

    public void Enable()
    {
        foreach (var grp in Service.Config.ActionGroups)
        {
            if (grp.Name != ActionGroupName) continue;
            grp.Enable = IsOn;
        }
    }

    public void Disable()
    {
        foreach (var grp in Service.Config.ActionGroups)
        {
            if (grp.Name != ActionGroupName) continue;
            grp.Enable = !IsOn;
        }
    }
}
