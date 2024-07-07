using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using RotationSolver.GameData.Getters.Actions;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.GameData.Getters.ActionSets;
internal class ComboActionGetter(Lumina.GameData gameData, ActionSingleRotationGetter actionGetter)
    : ActionSetGetterBase<ActionComboRoute>(gameData, actionGetter, false)
{
    protected override Action[] GetActions(ActionComboRoute item)
    {
        var result = new List<Action>();
        foreach (var action in item.Action)
        {
            if (action == null) continue;
            var act = action.Value;
            if (act == null) continue;
            if (act.RowId == 0) continue;
            result.Add(act);
        }
        var actions = _gameData.GetExcelSheet<Action>();

        if (actions != null)
        {
            AddRow(ref result, item.Unknown6, actions);
            AddRow(ref result, item.Unknown7, actions);
            AddRow(ref result, item.Unknown8, actions);
        }
        return [.. result];
    }

    private static void AddRow(ref List<Action> list, ushort row, ExcelSheet<Action> sheet)
    {
        if (row == 0) return;
        var action = sheet.GetRow(row);
        if (action == null) return;
        list.Add(action);
    }

    protected override string ToName(ActionComboRoute item)
    {
        return item.Name.RawString + (item.Action[0].Value!.IsPvP ? "PvP" : "PvE");
    }

    protected override string GetXmlComment(ActionComboRoute item, Action[] actions)
    {
       var desc = _gameData.GetExcelSheet<Lumina.Excel.GeneratedSheets2.ActionComboRouteTransient>()?.GetRow(item.RowId)?.Unknown0.RawString ?? string.Empty;

        return $"""
        /// <summary>
        /// <strong>{item.Name.RawString}</strong>
        /// <para>{desc.Replace("\n", "</para>\n/// <para>")}</para>
        /// {string.Join(" -> ", actions.Select(GetName))}
        /// </summary>
        """;

        string GetName(Action action)
        {
            return $"<seealso cref=\"{actionGetter.Items[action]}\"/>";
        }
    }
}
