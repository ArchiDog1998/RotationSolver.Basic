using Lumina.Excel.GeneratedSheets;
using RotationSolver.GameData.Getters.Actions;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.GameData.Getters.ActionSets;

internal class ReplaceActionGetter(Lumina.GameData gameData, ActionSingleRotationGetter actionGetter)
    : ActionSetGetterBase<ReplaceAction>(gameData, actionGetter)
{
    private bool _isReplace = true;
    protected override bool IsReplace => _isReplace;

    private static readonly List<Action> _sayedActions = [];
    private static readonly Dictionary<uint, (uint[], bool)> _replaceActions = new()
    {
        { 119, ([127, 3568, 7431, 16533, 25859], true) }, //WHM Stone PvE
        { 17869, ([3584, 7435, 16541, 25865], true) }, //SCH Ruin PvE
        { 3596, ([3598, 7442, 16555, 25871], true) }, //AST Malefic PvE
        { 24283, ([24306, 24312], true) }, //SGE Dosis PvE

        { 34689, ([34664, 34665, 34666, 34667], false) }, //PCT Creature Motif PvE
        { 35347, ([34670, 34671, 34672, 34673], false) }, //PCT Living Muse PvE

        { 35920, ([34634, 34635, 34640, 34641, 34642, 34643], false) }, //VPR Serpent's Tail PvE
        { 35921, ([34636, 34638, 34644], false) }, //VPR Twinfang PvE
        { 35922, ([34637, 34639, 34645], false) }, //VPR Twinblood PvE

        { 25764, ([25765, 3545, 36948, 25882, 25768, 3543, 25769], false) }, //MNK Masterful Blitz PvE
    };

    protected override string ToName(ReplaceAction item)
    {
        var actions = GetActions(item);
        return actionGetter.Items[actions[0]] + "Replace";
    }

    public override Action[] GetActions(ReplaceAction item)
    {
        List<Action> actionList = [];
        if (item.Action.Value is Action act && act.RowId != 0)
        {
            actionList.Add(act);
        }
        else
        {
            return [];
        }
        if (item.ReplaceAction1.Value is Action act1 && act1.RowId != 0/* && item.Type1 is not 4*/)
        {
            _isReplace = item.Type1 is not 1;
            actionList.Add(act1);
        }
        if (item.ReplaceAction2.Value is Action act2 && act2.RowId != 0/* && item.Type2 is not 4*/)
        {
            _isReplace |= item.Type1 is not 1;
            actionList.Add(act2);
        }
        if (item.ReplaceAction3.Value is Action act3 && act3.RowId != 0/* && item.Type3 is not 4*/)
        {
            _isReplace |= item.Type1 is not 1;
            actionList.Add(act3);
        }

        if (actionList.Count < 2)
        {
            var action = actionList[0];
            var hasActions = _replaceActions.TryGetValue(action.RowId, out var actionPair);
            if (hasActions)
            {
                var data = _gameData.GetExcelSheet<Action>();
                if (data != null)
                {
                    foreach (var actionId in actionPair.Item1)
                    {
                        actionList.Add(data.GetRow(actionId)!);
                    }
                }
                _isReplace = actionPair.Item2;
            }

            if (!_sayedActions.Contains(action))
            {
                _sayedActions.Add(action);
                Console.ForegroundColor = hasActions ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"Replace Action: {action.Name}({action.RowId})");
                Console.ResetColor();
            }

        }

        return [.. actionList];
    }

    protected override string GetXmlComment(ReplaceAction item, Action[] actions)
    {
        return $$"""
            /// <summary>
            /// {{string.Join(" -> ", actions.Select(GetName))}}
            /// </summary>
            """;

        string GetName(Action action)
        {
            return $"<seealso cref=\"{actionGetter.Items[action]}\"/>";
        }
    }
}
