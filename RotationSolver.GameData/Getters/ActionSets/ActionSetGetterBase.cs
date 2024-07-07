using Lumina.Excel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RotationSolver.GameData.Getters.Actions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static RotationSolver.GameData.SyntaxHelper;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.GameData.Getters.ActionSets;
internal abstract class ActionSetGetterBase<T>(Lumina.GameData gameData, ActionSingleRotationGetter actionGetter, ReplaceActionGetter? replace)
    : ExcelRowGetter<T, PropertyDeclarationSyntax>(gameData) where T : ExcelRow
{
    public virtual bool ReplaceAction => false;

    public bool IsReplace => replace == null;

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

        if (!IsReplace)
        {
            if (!Util.Enqueue(actions)) return false;
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

        var expElements = actions.Reverse().Select(i => ExpressionElement(IdentifierName(GetName(i))));

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
                                                                items)))),
                                             Token(SyntaxKind.CommaToken),
                                             Argument(
                                                 LiteralExpression(
                                                    IsReplace ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression))
                                            })))));

        return init;
    }

    private string GetName(Action i)
    {
        if (replace == null || !ReplaceAction) return actionGetter.Items[i];

        foreach ((var key, var value) in replace.Items)
        {
            if (replace.GetActions(key).Contains(i))
            {
                return value;
            }
        }

        return actionGetter.Items[i];
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


    public abstract Action[] GetActions(T item);

}
