using Lumina.Excel.GeneratedSheets;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using RotationSolver.GameData.Getters;
using RotationSolver.GameData.Getters.Actions;
using System.Net;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static RotationSolver.GameData.SyntaxHelper;

namespace RotationSolver.GameData;

internal static class CodeGenerator
{
    public static async Task CreateCode(Lumina.GameData gameData, DirectoryInfo dirInfo)
    {
        await GetStatusID(gameData, dirInfo);
        var actionIdData = await GetActionID(gameData, dirInfo);
        await GetContentType(gameData, dirInfo);
        await GetActionCategory(gameData, dirInfo);
        await GetDutyRotation(gameData, dirInfo, actionIdData);
        await GetBaseRotation(gameData, dirInfo, actionIdData);
        await GetRotations(gameData, dirInfo, actionIdData);
        await GetOpCode(dirInfo);
    }

    private static async Task GetDutyRotation(Lumina.GameData gameData, DirectoryInfo dirInfo, ActionIdGetter getter)
    {
        var dutyRotationBase = new ActionDutyRotationGetter(gameData, getter);
        var rotationNodes = dutyRotationBase.GetNodes();


        var type = ClassDeclaration("DutyRotation")
            .WithModifiers(
                TokenList(
                    [
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.AbstractKeyword),
                        Token(SyntaxKind.PartialKeyword)]))
            .AddAttributeLists(GeneratedCodeAttribute(typeof(CodeGenerator))
            .WithXmlComment($$"""
             /// <summary>
             /// The Custom Rotation.
             /// <br>Number of Actions: {{dutyRotationBase.Count}}</br>
             /// </summary>
             """))
            .AddMembers(rotationNodes);

        var majorNameSpace = NamespaceDeclaration("RotationSolver.Basic.Rotations.Duties").AddMembers(type);

        await SaveNode(majorNameSpace, dirInfo, "DutyRotation");
    }

    private static async Task GetBaseRotation(Lumina.GameData gameData, DirectoryInfo dirInfo, ActionIdGetter getter)
    {
        var rotationBase = new ActionRoleRotationGetter(gameData, getter);
        var rotationCodes = rotationBase.GetNodes();
        var rotationItems = new ItemGetter(gameData);
        var rotationItemCodes = rotationItems.GetNodes();

        var type = ClassDeclaration("CustomRotation")
            .WithModifiers(
                TokenList(
                    [
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.AbstractKeyword),
                    Token(SyntaxKind.PartialKeyword)]))
            .WithXmlComment($$"""
             /// <summary>
             /// The Custom Rotation.
             /// <br>Number of Actions: {{rotationBase.Count}}</br>
             /// </summary>
             """)
            .AddMembers([.. rotationCodes, .. rotationItemCodes,
            .. GetArrayProperty("global::RotationSolver.Basic.Actions.IBaseAction", "AllBaseActions", [SyntaxKind.PublicKeyword, SyntaxKind.VirtualKeyword], [.. rotationBase.AddedNames]),
            .. GetArrayProperty("global::RotationSolver.Basic.Actions.IBaseItem", "AllItems", [SyntaxKind.PublicKeyword], [.. rotationItems.AddedNames]),

            ]);

        var majorNameSpace = NamespaceDeclaration("RotationSolver.Basic.Rotations").AddMembers(type);

        await SaveNode(majorNameSpace, dirInfo, "CustomRotation");
    }

    private static async Task GetRotations(Lumina.GameData gameData, DirectoryInfo dirInfo, ActionIdGetter getter)
    {
        foreach (var job in gameData.GetExcelSheet<ClassJob>()!
            .Where(job => job.JobIndex > 0))
        {
            await BasicRotationGenerator.GetRotation(gameData, job, dirInfo, getter);
        }
    }

    internal static MemberDeclarationSyntax[] GetArrayProperty(string typeName, string propertyName, SyntaxKind[] keywords, string[] items)
    {
        var fieldName = "_" + propertyName;
        return [GetArrayField(typeName, fieldName), m_GetArrayProperty(typeName, propertyName, fieldName, keywords, items)];

        static FieldDeclarationSyntax GetArrayField(string typeName, string fieldName)
        {
            return FieldDeclaration(
                VariableDeclaration(
                    NullableType(
                        ArrayType(
                            IdentifierName(typeName))
                        .WithRankSpecifiers(
                            SingletonList(
                                ArrayRankSpecifier(
                                    SingletonSeparatedList<ExpressionSyntax>(
                                        OmittedArraySizeExpression()))))))
                .WithVariables(
                    SingletonSeparatedList(
                        VariableDeclarator(
                            Identifier(fieldName))
                        .WithInitializer(
                            EqualsValueClause(
                                LiteralExpression(
                                    SyntaxKind.NullLiteralExpression))))))
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PrivateKeyword)))
            .AddAttributeLists(GeneratedCodeAttribute(typeof(CodeGenerator)));
        }

        static PropertyDeclarationSyntax m_GetArrayProperty(string typeName, string propertyName, string fieldName, SyntaxKind[] keywords, string[] items)
        {
            var tokens = new List<SyntaxNodeOrToken>();
            foreach (var item in items)
            {
                if (tokens.Count > 0)
                {
                    tokens.Add(Token(SyntaxKind.CommaToken));
                }
                tokens.Add(ExpressionElement(IdentifierName(item)));
            }

            if (keywords.Contains(SyntaxKind.OverrideKeyword))
            {
                if (tokens.Count > 0)
                {
                    tokens.Add(Token(SyntaxKind.CommaToken));
                }
                tokens.Add(SpreadElement(MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    BaseExpression(),
                    IdentifierName(propertyName))));
            }

            return PropertyDeclaration(
                ArrayType(
                    IdentifierName(typeName))
                .WithRankSpecifiers(
                    SingletonList(
                        ArrayRankSpecifier(
                            SingletonSeparatedList<ExpressionSyntax>(
                                OmittedArraySizeExpression())))),
                Identifier(propertyName))
            .WithModifiers(TokenList(keywords.Select(Token).ToArray()))
            .WithExpressionBody(
                ArrowExpressionClause(
                    AssignmentExpression(
                        SyntaxKind.CoalesceAssignmentExpression,
                        IdentifierName(fieldName),
                        CollectionExpression(
                            SeparatedList<CollectionElementSyntax>(tokens)))))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
            .WithXmlComment("/// <inheritdoc/>")
            .AddAttributeLists(GeneratedCodeAttribute(typeof(CodeGenerator)));
        }
    }

    private static async Task GetContentType(Lumina.GameData gameData, DirectoryInfo dirInfo)
    {
        await GetEnums(gameData, dirInfo, new ContentTypeGetter(gameData).GetNodes(), "TerritoryContentType", "The TerritoryContentType", SyntaxKind.ByteKeyword);

    }
    private static async Task GetActionCategory(Lumina.GameData gameData, DirectoryInfo dirInfo)
    {
        await GetEnums(gameData, dirInfo, new ActionCategoryGetter(gameData).GetNodes(), "ActionCate", "The ActionCate", SyntaxKind.ByteKeyword);

    }
    private static async Task<ActionIdGetter> GetActionID(Lumina.GameData gameData, DirectoryInfo dirInfo)
    {
        var reuslt = new ActionIdGetter(gameData);
        await GetEnums(gameData, dirInfo, reuslt.GetNodes(), "ActionID", "The id of the action", SyntaxKind.UIntKeyword);
        return reuslt;
    }

    private static async Task GetStatusID(Lumina.GameData gameData, DirectoryInfo dirInfo)
    {
        await GetEnums(gameData, dirInfo, new StatusGetter(gameData).GetNodes(), "StatusID", "The id of the status", SyntaxKind.UShortKeyword);
    }

    private static async Task GetEnums(Lumina.GameData gameData, DirectoryInfo dirInfo, EnumMemberDeclarationSyntax[] members,
        string name, string description, SyntaxKind type)
    {
        var enumDefinition = EnumDeclaration(name)
            .AddBaseListTypes(SimpleBaseType(PredefinedType(Token(type))))
            .AddAttributeLists(GeneratedCodeAttribute(typeof(CodeGenerator)).WithXmlComment($"/// <summary> {description} </summary>"))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddMembers([EnumMember("None", 0).WithXmlComment($"/// <summary/>"), .. members]);

        var majorNameSpace = NamespaceDeclaration("RotationSolver.Basic.Data").AddMembers(enumDefinition);

        await SaveNode(majorNameSpace, dirInfo, name);
    }

    private static async Task GetOpCode(DirectoryInfo dirInfo)
    {
        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        using var result = await client.GetAsync("https://raw.githubusercontent.com/karashiiro/FFXIVOpcodes/master/opcodes.json");

        if (result.StatusCode != HttpStatusCode.OK) return;
        var responseStream = await result.Content.ReadAsStringAsync();

        var enums = JToken.Parse(responseStream)[0]!["lists"]!.Children()
            .SelectMany(i => i.Children()).SelectMany(i => i.Children()).Cast<JObject>()
            .Select(i =>
            {
                var name = (string)((JValue)i["name"]!).Value!;
                var value = (ushort)(long)((JValue)i["opcode"]!).Value!;
                var description = name!.Space();

                var desc = AttributeList(SingletonSeparatedList(DescriptionAttribute(description))).WithXmlComment($"/// <summary> {description} </summary>");

                return EnumMember(name, value).AddAttributeLists(desc);
            });

        var enumDefinition = EnumDeclaration("OpCode")
            .AddBaseListTypes(SimpleBaseType(PredefinedType(Token(SyntaxKind.UShortKeyword))))
            .AddAttributeLists(GeneratedCodeAttribute(typeof(CodeGenerator)).WithXmlComment($"/// <summary> The OpCode </summary>"))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddMembers([EnumMember("None", 0).WithXmlComment($"/// <summary/>"), ..enums]);

        var majorNameSpace = NamespaceDeclaration("RotationSolver.Basic.Data").AddMembers(enumDefinition);

        await SaveNode(majorNameSpace, dirInfo, "OpCode");
    }
}
