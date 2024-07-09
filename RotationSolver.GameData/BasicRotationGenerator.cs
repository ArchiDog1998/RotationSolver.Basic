using Lumina.Excel.GeneratedSheets;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RotationSolver.GameData.Getters;
using RotationSolver.GameData.Getters.Actions;
using RotationSolver.GameData.Getters.ActionSets;
using System.Reflection;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static RotationSolver.GameData.SyntaxHelper;

namespace RotationSolver.GameData;
internal static class BasicRotationGenerator
{
    internal static async Task GetRotation(Lumina.GameData gameData, ClassJob job, DirectoryInfo dirInfo, ActionIdGetter getter)
    {
        var className = (job.NameEnglish.RawString + " Rotation").ToPascalCase();
        var jobName = job.NameEnglish.RawString;

        Util.Clear();
        var rotationsGetter = new ActionSingleRotationGetter(gameData, job, getter);
        var traitsGetter = new TraitRotationGetter(gameData, job);
        var replaceActions = new ReplaceActionGetter(gameData, rotationsGetter);
        var actionsCombo = new ActionComboActionGetter(rotationsGetter, replaceActions);

        List<MemberDeclarationSyntax> list = [.. rotationsGetter.GetNodes(), .. traitsGetter.GetNodes(), ..replaceActions.GetNodes(),..actionsCombo.GetDeclarations(),
        .. CodeGenerator.GetArrayProperty("global::RotationSolver.Basic.Traits.IBaseTrait", "AllTraits", [SyntaxKind.PublicKeyword, SyntaxKind.OverrideKeyword], [.. traitsGetter.AddedNames])];

        if (!job.IsLimitedJob)
        {
            var names = new List<string>();
            var jobgauge = GetJobGauge(job, ref names);
            list.AddRange(jobgauge);

            if (names.Count > 0)
            {
                list.Add(GetDisplayStatus(names));
            }
        }

        var rotationNames = rotationsGetter.AddedNames;
        if (job.LimitBreak1.Value is Lumina.Excel.GeneratedSheets.Action a
            && a.RowId != 0)
        {
            list.AddRange(GetLBInRotation(a, 1, getter));
            rotationNames.Add("LimitBreak1");
        }
        if (job.LimitBreak2.Value is Lumina.Excel.GeneratedSheets.Action b
            && b.RowId != 0)
        {
            list.AddRange(GetLBInRotation(b, 2, getter));
            rotationNames.Add("LimitBreak2");
        }
        if (job.LimitBreak3.Value is Lumina.Excel.GeneratedSheets.Action c
            && c.RowId != 0)
        {
            list.AddRange(GetLBInRotation(c, 3, getter));
            rotationNames.Add("LimitBreak3");
        }
        if (replaceActions.Count + actionsCombo.Count > 0)
        {
            var ctor = ConstructorDeclaration(Identifier(className))
            .AddAttributeLists(GeneratedCodeAttribute(typeof(BasicRotationGenerator)))
            .WithXmlComment("/// <inheritdoc/>")
            .WithModifiers(
                TokenList(Token(SyntaxKind.ProtectedKeyword)))
            .WithBody(Block(replaceActions.GetInit().Union(actionsCombo.GetInits())));
            list.Add(ctor);
        }

        list.AddRange(CodeGenerator.GetArrayProperty("global::RotationSolver.Basic.Actions.IBaseAction", "AllBaseActions", [SyntaxKind.PublicKeyword, SyntaxKind.OverrideKeyword], [.. rotationNames]));

        var type = ClassDeclaration(className)
            .WithModifiers(
                TokenList([
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.AbstractKeyword),
                    Token(SyntaxKind.PartialKeyword)]))
            .WithBaseList(
                BaseList(SingletonSeparatedList<BaseTypeSyntax>(
                    SimpleBaseType(IdentifierName("global::RotationSolver.Basic.Rotations.CustomRotation")))))
            .WithAttributeLists(SingletonList(JobsAttribute(job)))
            .WithXmlComment($$"""
            /// <summary>
            /// <see href="https://na.finalfantasyxiv.com/jobguide/{{jobName.Replace(" ", "").ToLower()}}"><strong>{{jobName}}</strong></see>
            /// <br>Number of Actions: {{rotationsGetter.Count}}</br>
            /// <br>Number of Replace Actions: {{replaceActions.Count}}</br>
            /// <br>Number of Action Combos: {{actionsCombo.Count}}</br>
            /// <br>Number of Traits: {{traitsGetter.Count}}</br>
            /// </summary>
            """)
            .AddMembers([.. list]);

        var majorNameSpace = NamespaceDeclaration("RotationSolver.Basic.Rotations.Basic").AddMembers(type);

        await SaveNode(majorNameSpace, dirInfo, className);
    }

    private static MemberDeclarationSyntax[] GetLBInRotation(Lumina.Excel.GeneratedSheets.Action action, int index, ActionIdGetter getter)
    {
        var declarations = GetLBPvE(action, out var name, getter);

        var property = PropertyDeclaration(
            IdentifierName("global::RotationSolver.Basic.Actions.IBaseAction"),
            Identifier("LimitBreak" + index.ToString()))
        .WithModifiers(
            TokenList(
                [
                    Token(SyntaxKind.PrivateKeyword),
                    Token(SyntaxKind.SealedKeyword),
                    Token(SyntaxKind.ProtectedKeyword),
                    Token(SyntaxKind.OverrideKeyword)]))
        .WithExpressionBody(
            ArrowExpressionClause(
                IdentifierName(name)))
        .WithSemicolonToken(
            Token(SyntaxKind.SemicolonToken))
        .AddAttributeLists(GeneratedCodeAttribute(typeof(BasicRotationGenerator)).WithXmlComment($"/// <inheritdoc cref=\"{name}\"/>"));

        return [.. declarations, property];
    }

    private static MemberDeclarationSyntax[] GetLBPvE(Lumina.Excel.GeneratedSheets.Action action, out string name, ActionIdGetter getter)
    {
        name = action.Name.RawString.ToPascalCase() + $"PvE";
        return action.ToNodes(name, getter, false);
    }

    private static MethodDeclarationSyntax GetDisplayStatus(List<string> names)
    {
        var baseCall = ExpressionStatement(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            BaseExpression(),
                            IdentifierName("DisplayStatus"))));

        var method = MethodDeclaration(
           PredefinedType(Token(SyntaxKind.VoidKeyword)), Identifier("DisplayStatus"))
            .WithXmlComment("/// <inheritdoc/>")
            .WithModifiers(
                TokenList(
                    [
                         Token(SyntaxKind.PublicKeyword),
                         Token(SyntaxKind.OverrideKeyword)]))
            .WithBody(
           Block(names.Select(GetOneLine).Append(baseCall).ToList()));

        return method;
    }

    private static ExpressionStatementSyntax GetOneLine(string name)
    {
        return ExpressionStatement(
                       InvocationExpression(
                           MemberAccessExpression(
                               SyntaxKind.SimpleMemberAccessExpression,
                               IdentifierName("ImGui"),
                               IdentifierName("Text")))
                       .WithArgumentList(
                           ArgumentList(
                               SingletonSeparatedList(
                                   Argument(
                                       BinaryExpression(
                                           SyntaxKind.AddExpression,
                                           LiteralExpression(
                                               SyntaxKind.StringLiteralExpression,
                                               Literal(name + ": ")),
                                           InvocationExpression(
                                               MemberAccessExpression(
                                                   SyntaxKind.SimpleMemberAccessExpression,
                                                   IdentifierName(name),
                                                   IdentifierName("ToString")))))))));
    }

    private static PropertyDeclarationSyntax[] GetJobGauge(ClassJob job, ref List<string> names)
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var gaugeName = $"Dalamud.Game.ClientState.JobGauge.Types.{job.Abbreviation}Gauge";
        var assembly = Assembly.LoadFrom(path + "\\XIVLauncher\\addon\\Hooks\\dev\\Dalamud.dll");

        var gaugeType = assembly?.GetType(gaugeName);
        if (gaugeType == null) return [];

        var result = new List<PropertyDeclarationSyntax>() { GetJobGauge(IdentifierName("global::" + gaugeName)) };

        foreach (var prop in gaugeType.GetRuntimeProperties().Where(p => (p.GetMethod?.IsPublic ?? false) && p.DeclaringType == gaugeType))
        {
            result.AddRange(GetGaugeItemFromProperty(prop));
            names.Add(prop.Name);
        }

        return [.. result];
    }

    private static PropertyDeclarationSyntax[] GetGaugeItemFromProperty(PropertyInfo prop)
    {
        var typeName = prop.PropertyType.FullName ?? prop.PropertyType.Name;
        var name = prop.Name;
        if (name.Contains("Time"))
        {
            var result = PropertyDeclaration(PredefinedType(Token(SyntaxKind.FloatKeyword)), Identifier(name))
                .AddAttributeLists(GeneratedCodeAttribute(typeof(BasicRotationGenerator)))
                .WithXmlComment($"/// <inheritdoc cref=\"{prop.DeclaringType!.Name}.{name}\"/>")
                .WithModifiers(
                    TokenList(
                        [
                            Token(SyntaxKind.PublicKeyword),
                            Token(SyntaxKind.StaticKeyword)]))
                .WithExpressionBody(
                    ArrowExpressionClause(
                        BinaryExpression(
                            SyntaxKind.SubtractExpression,
                            BinaryExpression(
                                SyntaxKind.DivideExpression,
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("JobGauge"),
                                    IdentifierName(name)),
                                LiteralExpression(
                                    SyntaxKind.NumericLiteralExpression,
                                    Literal(
                                        "1000f",
                                        1000f))),
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("DataCenter"),
                                IdentifierName("WeaponRemain")))))
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
            return [result];
        }
        else
        {
            var result = PropertyDeclaration(IdentifierName(typeName), Identifier(name))
                .AddAttributeLists(GeneratedCodeAttribute(typeof(BasicRotationGenerator)))
                .WithXmlComment($"/// <inheritdoc cref=\"{prop.DeclaringType!.Name}.{name}\"/>")
                .WithModifiers(
                    TokenList(
                        [
                            Token(SyntaxKind.PublicKeyword),
                                    Token(SyntaxKind.StaticKeyword)]))
                .WithExpressionBody(
                    ArrowExpressionClause(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("JobGauge"),
                            IdentifierName(name))))
                .WithSemicolonToken(
                Token(SyntaxKind.SemicolonToken));
            return [result];
        }
    }

    private static PropertyDeclarationSyntax GetJobGauge(IdentifierNameSyntax jobgaugeName)
    {
        return PropertyDeclaration(jobgaugeName, Identifier("JobGauge"))
                .AddAttributeLists(GeneratedCodeAttribute(typeof(BasicRotationGenerator)))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.StaticKeyword)))
                .WithExpressionBody(
                    ArrowExpressionClause(
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName("global::ECommons.DalamudServices.Svc"),
                                    IdentifierName("Gauges")),
                                GenericName(
                                    Identifier("Get"))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SingletonSeparatedList<TypeSyntax>(
                                            jobgaugeName)))))))
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static AttributeListSyntax JobsAttribute(ClassJob job)
    {
        var list = new List<SyntaxNodeOrToken>()
        {
            GetJobArgument(job.Abbreviation)
        };

        if (job.RowId != 28 && job.RowId != job.ClassJobParent.Row)
        {
            list.Add(Token(SyntaxKind.CommaToken));
            list.Add(GetJobArgument(job.ClassJobParent.Value?.Abbreviation ?? "ADV"));
        }

        return AttributeList(
           SingletonSeparatedList(
               Attribute(IdentifierName("global::RotationSolver.Basic.Attributes.Jobs"))
               .WithArgumentList(
                   AttributeArgumentList(SeparatedList<AttributeArgumentSyntax>(list)))));

        static AttributeArgumentSyntax GetJobArgument(string name)
        {
            return AttributeArgument(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                   IdentifierName("global::ECommons.ExcelServices.Job"), IdentifierName(name)));
        }
    }
}