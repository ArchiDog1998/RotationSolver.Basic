using Lumina.Excel.GeneratedSheets;
using RotationSolver.GameData.Getters.Actions;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.GameData.Getters.ActionSets;

internal class ReplaceActionGetter(Lumina.GameData gameData, ActionSingleRotationGetter actionGetter)
    : ActionSetGetterBase<ReplaceAction>(gameData, actionGetter, true)
{
    public override bool ReplaceAction => true;


    protected override string ToName(ReplaceAction item)
    {
        var actions = GetActions(item);
        return actionGetter.Items[actions[0]] + "Replace";
    }

    public override Action[] GetActions(ReplaceAction item)
    {
        List<Action> actionList = [];
        if (item.Action.Value is Action act && act.RowId != 0) actionList.Add(act);
        if (item.ReplaceAction1.Value is Action act1 && act1.RowId != 0/* && item.Type1 is not 4*/) actionList.Add(act1);
        if (item.ReplaceAction2.Value is Action act2 && act2.RowId != 0/* && item.Type2 is not 4*/) actionList.Add(act2);
        if (item.ReplaceAction3.Value is Action act3 && act3.RowId != 0/* && item.Type3 is not 4*/) actionList.Add(act3);
        return [.. actionList];
    }

    protected override string GetXmlComment(ReplaceAction item, Action[] actions)
    {
        return $$"""
            /// <summary>
            /// <see cref="{{typeof(ReplaceAction).FullName!}}"/>
            /// {{string.Join(" -> ", actions.Select(GetName))}}
            /// </summary>
            """;

        string GetName(Action action)
        {
            return $"<seealso cref=\"{actionGetter.Items[action]}\"/>";
        }
    }
}
