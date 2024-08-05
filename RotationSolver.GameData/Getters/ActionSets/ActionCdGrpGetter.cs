using RotationSolver.GameData.Getters.Actions;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.GameData.Getters.ActionSets;

internal class ActionCdGrpGetter(ActionSingleRotationGetter actionGetter, ReplaceActionGetter replace)
    : ActionActionsGetter(actionGetter, replace)
{
    public override string Postfix => "CdGrp";

    private readonly Dictionary<Action, Action[]> _cdGrps = [];

    private void UpdateGrps()
    {
        foreach(var items in actionGetter.Items.Keys.GroupBy(i => i.IsPvP))
        {
            var grps = items.GroupBy(a => a.CooldownGroup == 58 ? a.AdditionalCooldownGroup : a.CooldownGroup);

            foreach (var kvp in grps)
            {
                if (kvp.Key is 0 or 58) continue;

                if (kvp.Count() < 2) continue;

                var acts = kvp.OrderBy(a => a.CastType)
                    .ThenBy(a => a.ClassJobLevel).ToArray();

                _cdGrps[acts[0]] = acts;
            }
        }
    }

    protected override Action[] GetComboAction(Action action)
    {
        if (_cdGrps.Count == 0)
        {
            UpdateGrps();
        }

        if (_cdGrps.TryGetValue(action, out var actions))
        {
            return actions;
        }
        else
        {
            return [];
        }
    }
}
