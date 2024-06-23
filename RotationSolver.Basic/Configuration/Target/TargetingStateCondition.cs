using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("State Condition")]
internal class TargetingStateCondition : TargetingConditionBase
{
    [UI("State")]
    public TargetingState TargetingState { get; set; } = TargetingState.IsDying;
    protected override bool IsTrueInside(GameObject obj)
    {
        if (obj is not BattleChara battle) return false;
        return TargetingState switch
        {
            TargetingState.IsBossFromTTK => battle.IsBossFromTTK(),
            TargetingState.IsBossFromIcon => battle.IsBossFromIcon(),
            TargetingState.IsDying => battle.IsDying(),
            TargetingState.InCombat => battle.InCombat(),
            _ => false,
        };
    }
}

internal enum TargetingState : byte
{
    [Description("Is Dying")]
    IsDying,

    [Description("Is Boss From TTK")]
    IsBossFromTTK,

    [Description("Is Boss From Icon")]
    IsBossFromIcon,

    [Description("In Combat")]
    InCombat,
}
