using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using RotationSolver.GameData.Getters.Actions;
using Action = Lumina.Excel.GeneratedSheets.Action;
namespace RotationSolver.GameData.Getters.ActionSets;

internal class ActionIndirectionGetter(Lumina.GameData gameData, ActionSingleRotationGetter actionGetter, ReplaceActionGetter replace)
    : ActionSetGetterBase<ActionIndirection>(gameData, actionGetter, replace)
{
    public override Action[] GetActions(ActionIndirection item)
    {
        var action = item.Name.Value;
        if (action == null || action.RowId == 0) return [];
        var indirections = _gameData.GetExcelSheet<ActionIndirection>();
        if (indirections == null) return [];

        var result = GetActions([action], indirections).Reverse();
        return [.. result];
    }

    private static IEnumerable<Action> GetActions(IEnumerable<Action> actions, ExcelSheet<ActionIndirection> sheet)
    {
        if (!actions.Any()) return [];
        var last = actions.Last();

        //Not the top.
        if(!OnTheTop(actions, last, sheet))
        {
            return [];
        }

        var indirection = sheet.FirstOrDefault(i => i.Name.Value == last);
        if (indirection == null 
            || indirection.PreviousComboAction.Value is not Action privousAction
            || privousAction.RowId == 0)
        {
            return actions;
        }
        else
        {
            return GetActions(actions.Append(privousAction), sheet);
        }
    }

    private static bool OnTheTop(IEnumerable<Action> actions, Action action, ExcelSheet<ActionIndirection> sheet)
    {
        var parents = sheet.Where(i => i.PreviousComboAction.Value == action);
        if (!parents.Any()) return true;
        return parents.Any(i => actions.Contains(i.Name.Value));
    }

    protected override string GetXmlComment(ActionIndirection item, Action[] actions)
    {
        return $$"""
            /// <summary>
            /// <see cref="{{typeof(ActionIndirection).FullName!}}"/>
            /// {{string.Join(" -> ", actions.Select(GetName))}}
            /// </summary>
            """;

        string GetName(Action action)
        {
            return $"<seealso cref=\"{actionGetter.Items[action]}\"/>";
        }
    }

    protected override string ToName(ActionIndirection item)
    {
        var act = item.Name.Value;
        return act!.Name.RawString + (act.IsPvP ? "PvP" : "PvE") + "Indirection";
    }
}
