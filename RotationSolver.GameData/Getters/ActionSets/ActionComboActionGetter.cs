using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RotationSolver.GameData.Getters.Actions;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.GameData.Getters.ActionSets;
internal class ActionComboActionGetter(ActionSingleRotationGetter actionGetter, ReplaceActionGetter replace)
{
    public int Count { get; private set; } = 0;
    public IEnumerable<ExpressionStatementSyntax> GetInits()
    {
        foreach (var action in actionGetter.Items.Keys)
        {
            var comboActions = GetComboAction(action);
            if (comboActions.Length < 2) continue;

            var writer = new ActionSetWriter(actionGetter.Items[action] + "Combo");

            yield return writer.GetInit(comboActions, actionGetter, replace, false);
        }
    }

    public IEnumerable<PropertyDeclarationSyntax> GetDeclarations()
    {
        foreach (var action in actionGetter.Items.Keys)
        {
            var comboActions = GetComboAction(action);
            if (comboActions.Length < 2) continue;

            Count++;

            var writer = new ActionSetWriter(actionGetter.Items[action] + "Combo");

            yield return writer.GetDeclaration($$"""
            /// <summary>
            /// {{string.Join(" -> ", comboActions.Select(GetName))}}
            /// </summary>
            """);
        }

        string GetName(Action action)
        {
            return $"<seealso cref=\"{actionGetter.Items[action]}\"/>";
        }
    }

    private static Action[] GetComboAction(Action action) => GetComboAction([action]).Reverse().ToArray();

    private static IEnumerable<Action> GetComboAction(IEnumerable<Action> actions)
    {
        if (!actions.Any()) return actions;
        var last = actions.Last();
        var oneMore = last.ActionCombo.Value;
        if (oneMore == null || oneMore.RowId == 0) return actions;
        return GetComboAction(actions.Append(oneMore));
    }
}
