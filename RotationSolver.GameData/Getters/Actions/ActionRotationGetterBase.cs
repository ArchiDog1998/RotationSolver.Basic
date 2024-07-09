using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RotationSolver.GameData.Getters.Actions;
internal abstract class ActionRotationGetterBase(Lumina.GameData gameData, ActionIdGetter getter)
    : ActionGetterBase<MemberDeclarationSyntax>(gameData)
{
    protected override MemberDeclarationSyntax[] ToNodes(Lumina.Excel.GeneratedSheets.Action item, string name)
    {
        return item.ToNodes(name, getter, IsDutyAction);
    }

    public abstract bool IsDutyAction { get; }
}
