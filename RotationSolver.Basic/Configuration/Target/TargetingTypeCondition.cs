using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons.GameHelpers;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Type Condition")]

internal class TargetingTypeCondition : TargetingConditionBase
{
    [UI("Type")]
    public TargetingType TargetingType { get; set; } = TargetingType.MySelf;

    public override bool IsTrue(GameObject obj)
    {
        return TargetingType switch
        {
             TargetingType.MySelf => obj.ObjectId == Player.Object.ObjectId,
             TargetingType.Player => obj is PlayerCharacter,
             TargetingType.Battle => obj is BattleChara,
             TargetingType.Hostile => obj is BattleChara battle && battle.IsEnemy(),
             TargetingType.Fiendly => obj is BattleChara battle && battle.IsAlliance(),
            _ => false,
        };
    }
}

internal enum TargetingType : byte
{
    MySelf,
    Player,
    Battle,
    Hostile,
    Fiendly,
}
