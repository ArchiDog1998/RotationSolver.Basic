using Microsoft.CodeAnalysis.CSharp.Syntax;
using RotationSolver.GameData.Getters.Actions;
using Action = Lumina.Excel.GeneratedSheets.Action;


namespace RotationSolver.GameData.Getters.ActionSets;
internal abstract class ActionActionsGetter(ActionSingleRotationGetter actionGetter, ReplaceActionGetter replace)
{
    public abstract string Postfix { get; }

    public int Count { get; private set; } = 0;

    public IEnumerable<PropertyDeclarationSyntax> GetDeclarations()
    {
        foreach (var action in actionGetter.Items.Keys)
        {
            var comboActions = GetComboAction(action);
            if (!ActionSetWriter.IsValid(comboActions, actionGetter, replace)) continue;

            Count++;

            var writer = new ActionSetWriter(actionGetter.Items[action] + Postfix);

            yield return writer.GetDeclaration($$"""
            /// <summary>
            /// {{ActionSetWriter.GetDescription(comboActions, actionGetter, replace)}}
            /// </summary>
            """);
        }
    }

    public IEnumerable<ExpressionStatementSyntax> GetInits()
    {
        foreach (var action in actionGetter.Items.Keys)
        {
            var comboActions = GetComboAction(action);
            if (!ActionSetWriter.IsValid(comboActions, actionGetter, replace)) continue;

            var writer = new ActionSetWriter(actionGetter.Items[action] + Postfix);

            yield return writer.GetInit(comboActions, actionGetter, replace);
        }
    }

    protected abstract Action[] GetComboAction(Action action);
}
