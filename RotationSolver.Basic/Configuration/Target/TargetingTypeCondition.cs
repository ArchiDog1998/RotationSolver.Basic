using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons.GameHelpers;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Type Condition")]

internal class TargetingTypeCondition : TargetingConditionBase
{
    internal enum TargetingTypeCon: byte
    {
        MySelf,
        Player,
        Battle,
        Hostile,
        Friendly,
    }

    [UI("Type")]
    public TargetingTypeCon TargetingType { get; set; } = TargetingTypeCon.MySelf;

    public override bool IsTrue(GameObject obj)
    {
        return TargetingType switch
        {
            TargetingTypeCon.MySelf => obj.ObjectId == Player.Object.ObjectId,
            TargetingTypeCon.Player => obj is PlayerCharacter,
            TargetingTypeCon.Battle => obj is BattleChara,
            TargetingTypeCon.Hostile => obj is BattleChara battle && battle.IsEnemy(),
            TargetingTypeCon.Friendly => obj is BattleChara battle && battle.IsAlliance(),
            _ => false,
        };
    }
}