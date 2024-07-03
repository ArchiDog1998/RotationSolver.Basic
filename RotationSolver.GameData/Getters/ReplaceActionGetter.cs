using Microsoft.CodeAnalysis.CSharp.Syntax;
using Lumina.Excel.GeneratedSheets;
using RotationSolver.GameData.Getters.Actions;
using Action = Lumina.Excel.GeneratedSheets.Action;
using Microsoft.CodeAnalysis.CSharp;
using static RotationSolver.GameData.SyntaxHelper;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis;

namespace RotationSolver.GameData.Getters;

internal class ReplaceActionGetter(Lumina.GameData gameData, ActionSingleRotationGetter actionGetter)
    : ExcelRowGetter<ReplaceAction, PropertyDeclarationSyntax>(gameData)
{
    protected override bool AddToList(ReplaceAction item)
    {
        var actions = GetActions(item);
        if (actions.Length < 2) return false;
        foreach (var action in actions)
        {
            if (!actionGetter.Items.ContainsKey(action)) return false;
        }
        return true;
    }

    protected override string ToName(ReplaceAction item)
    {
        var actions = GetActions(item);
        return actionGetter.Items[actions[0]] + "Set";
    }

    protected override PropertyDeclarationSyntax[] ToNodes(ReplaceAction item, string name)
    {
        var actions = GetActions(item);
        
        var prop = PropertyDeclaration(
            IdentifierName("global::RotationSolver.Basic.Actions.IBaseActionSet"),
            Identifier(name))
                .AddAttributeLists(GeneratedCodeAttribute(typeof(BasicRotationGenerator)))
            .WithXmlComment($$"""
            /// <summary
            /// {{string.Join(" -> ", actions.Reverse().Select(GetName))}}
            /// </summary>
            """)
        .WithModifiers(
            TokenList(
                Token(SyntaxKind.PublicKeyword)))
        .WithAccessorList(
            AccessorList(
                SingletonList(
                    AccessorDeclaration(
                        SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(
                        Token(SyntaxKind.SemicolonToken)))));

        return [prop];

        string GetName(Action action)
        {
            return $"<seealso cref=\"{actionGetter.Items[action]}\"/>";
        }
    }

    private static Action[] GetActions(ReplaceAction item)
    {
        List<Action> actionList = [];
        if (item.Action.Value is Action act && act.RowId != 0) actionList.Add(act);
        if (item.ReplaceAction1.Value is Action act1 && act1.RowId != 0 && item.Type1 is not 8) actionList.Add(act1);
        if (item.ReplaceAction2.Value is Action act2 && act2.RowId != 0 && item.Type2 is not 8) actionList.Add(act2);
        if (item.ReplaceAction3.Value is Action act3 && act3.RowId != 0 && item.Type3 is not 8) actionList.Add(act3);
        return [.. actionList];
    }

    public IEnumerable<ExpressionStatementSyntax> GetInit()
    {
        foreach (var pair in Items)
        {
            yield return GetInit(pair.Key, pair.Value);
        }
    }
    private ExpressionStatementSyntax GetInit(ReplaceAction item, string name)
    {
        var actions = GetActions(item);

        var expElements = actions.Reverse().Select(i => ExpressionElement(IdentifierName(actionGetter.Items[i])));

        List<SyntaxNodeOrToken> items = [];
        foreach (var element in expElements)
        {
            if(items.Count > 0)
            {
                items.Add(Token(SyntaxKind.CommaToken));
            }
            items.Add(element);
        }

        var init = ExpressionStatement(
                            AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                IdentifierName(name),
                                ObjectCreationExpression(
                                    IdentifierName("global::RotationSolver.Basic.Actions.BaseActionSet"))
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList(
                                            Argument(
                                                ParenthesizedLambdaExpression()
                                                .WithExpressionBody(
                                                    CollectionExpression(
                                                        SeparatedList<CollectionElementSyntax>(
                                                                items)))))))));

        return init;
    }
}
