using Lumina.Excel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RotationSolver.GameData.Getters.Actions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static RotationSolver.GameData.SyntaxHelper;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.GameData.Getters.ActionSets;
internal abstract class ActionSetGetterBase<T>(Lumina.GameData gameData, ActionSingleRotationGetter actionGetter)
    : ExcelRowGetter<T, PropertyDeclarationSyntax>(gameData) where T : ExcelRow
{
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
        return true;
    }

    public IEnumerable<ExpressionStatementSyntax> GetInit()
    {
        foreach (var pair in Items)
        {
            yield return GetInit(pair.Key, pair.Value);
        }
    }
    private ExpressionStatementSyntax GetInit(T item, string name)
    {
        var actions = GetActions(item);

        var expElements = actions.Reverse().Select(i => ExpressionElement(IdentifierName(actionGetter.Items[i])));

        List<SyntaxNodeOrToken> items = [];
        foreach (var element in expElements)
        {
            if (items.Count > 0)
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

    protected abstract string GetXmlComment(T item, Action[] actions);

    protected override PropertyDeclarationSyntax[] ToNodes(T item, string name)
    {
        var actions = GetActions(item);

        var prop = PropertyDeclaration(
            IdentifierName("global::RotationSolver.Basic.Actions.IBaseActionSet"),
            Identifier(name))
                .AddAttributeLists(GeneratedCodeAttribute(typeof(BasicRotationGenerator)))
            .WithXmlComment(GetXmlComment(item, actions))
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


    }


    protected abstract Action[] GetActions(T item);

}
