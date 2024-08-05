using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RotationSolver.GameData.Getters.Actions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static RotationSolver.GameData.SyntaxHelper;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.GameData.Getters.ActionSets;

internal class ActionSetWriter(string name)
{
    public static bool IsValid(Action[] actions, ActionSingleRotationGetter actionGetter, ReplaceActionGetter? replace)
    {
        return actions.Select(a => GetName(a, actionGetter, replace)).ToHashSet().Count > 1;
    }

    public ExpressionStatementSyntax GetInit(Action[] actions, ActionSingleRotationGetter actionGetter, ReplaceActionGetter? replace)
    {
        var expElements = actions.Reverse().Select(i => GetName(i, actionGetter, replace)).ToHashSet()
            .Select(i => ExpressionElement(IdentifierName(i)));

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
                                        SeparatedList<ArgumentSyntax>(
                                            new SyntaxNodeOrToken[]{
                                            Argument(
                                                ParenthesizedLambdaExpression()
                                                .WithExpressionBody(
                                                    CollectionExpression(
                                                        SeparatedList<CollectionElementSyntax>(
                                                                items))))})))));

        return init;
    }

    public static string GetDescription(Action[] actions, ActionSingleRotationGetter actionGetter, ReplaceActionGetter? replace)
    {
        return string.Join(" -> ", actions.Select(a => $"<seealso cref=\"{GetName(a, actionGetter, replace)}\"/>").ToHashSet());
    }

    private static string GetName(Action action, ActionSingleRotationGetter actionGetter, ReplaceActionGetter? replace)
    {
        if (replace != null)
        {
            foreach ((var key, var value) in replace.Items)
            {
                if (replace.GetActions(key).Contains(action))
                {
                    return value;
                }
            }
        }

        return actionGetter.Items[action];
    }

    public PropertyDeclarationSyntax GetDeclaration(string comment)
    {
        return PropertyDeclaration(
            IdentifierName("global::RotationSolver.Basic.Actions.IBaseActionSet"),
            Identifier(name))
                .AddAttributeLists(GeneratedCodeAttribute(typeof(BasicRotationGenerator)))
            .WithXmlComment(comment)
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
    }
}
