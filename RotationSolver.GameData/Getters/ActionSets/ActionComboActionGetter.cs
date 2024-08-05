using Microsoft.CodeAnalysis;
using RotationSolver.GameData.Getters.Actions;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.GameData.Getters.ActionSets;
internal class ActionComboActionGetter(ActionSingleRotationGetter actionGetter, ReplaceActionGetter replace)
    : ActionActionsGetter(actionGetter, replace)
{
    public override string Postfix => "Combo";

    protected override Action[] GetComboAction(Action action)
        => GetComboAction([action]).Reverse().ToArray();

    private static IEnumerable<Action> GetComboAction(IEnumerable<Action> actions)
    {
        if (!actions.Any()) return actions;
        var last = actions.Last();
        var oneMore = last.ActionCombo.Value;
        if (oneMore == null || oneMore.RowId == 0) return actions;
        return GetComboAction(actions.Append(oneMore));
    }
}
