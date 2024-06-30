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

    protected override bool IsTrueInside(IGameObject obj)
    {
        return TargetingType switch
        {
            TargetingTypeCon.MySelf => obj.EntityId == Player.Object.EntityId,
            TargetingTypeCon.Player => obj is IPlayerCharacter,
            TargetingTypeCon.Battle => obj is IBattleChara,
            TargetingTypeCon.Hostile => obj is IBattleChara battle && battle.IsEnemy(),
            TargetingTypeCon.Friendly => obj is IBattleChara battle && battle.IsAlliance(),
            _ => false,
        };
    }
}