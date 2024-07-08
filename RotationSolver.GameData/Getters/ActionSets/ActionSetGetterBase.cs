using Lumina.Excel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RotationSolver.GameData.Getters.Actions;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.GameData.Getters.ActionSets;
internal abstract class ActionSetGetterBase<T>(Lumina.GameData gameData, ActionSingleRotationGetter actionGetter, bool isReplace)
    : ExcelRowGetter<T, PropertyDeclarationSyntax>(gameData) where T : ExcelRow
{
    public virtual bool ReplaceAction => false;

    protected override bool AddToList(T item)
    {
        var actions = GetActions(item);
        if (actions.Length < 2) return false;

        foreach (var action in actions)
        {
            if (!actionGetter.Items.ContainsKey(action))
            {
                return false;
            }
        }

        if (!isReplace)
        {
            if (!Util.Enqueue(actions)) return false;
        }

        return true;
    }

    public IEnumerable<ExpressionStatementSyntax> GetInit()
    {
        foreach (var pair in Items)
        {
            var writer = new ActionSetWriter(pair.Value);
            yield return writer.GetInit(GetActions(pair.Key), actionGetter, ReplaceAction);
        }
    }
    protected abstract string GetXmlComment(T item, Action[] actions);

    protected override PropertyDeclarationSyntax[] ToNodes(T item, string name)
    {
        var writer = new ActionSetWriter(name);
        var actions = GetActions(item);
        return [ writer.GetDeclaration(GetXmlComment(item, actions))];
    }

    public abstract Action[] GetActions(T item);
}
