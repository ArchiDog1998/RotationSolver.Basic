using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Condition;

[Description("Last Action Condition")]
internal class LastActionCondition : DelayConditionBase
{
    [UI("Type")]
    public LastActionType LastAction { get; set; } = LastActionType.GCD;

    internal IBaseAction? _action;

    [UI("Action")]
    public ActionID ID { get; set; } = ActionID.None;

    [UI("Is Adjusted")]
    public bool IsAdjust { get; set; } = true;

    public override bool? CheckBefore()
    {
        var rotation = DataCenter.RightNowRotation;
        if (rotation == null) return null;
        CheckBaseAction(rotation, ID, ref _action);
        if (_action == null) return null;
        return base.CheckBefore();
    }

    protected override bool IsTrueInside()
    {
        if (_action == null) return false;
        return LastAction switch
        {
            LastActionType.GCD => CombatData.IsLastGCD(IsAdjust, _action),
            LastActionType.Ability => CombatData.IsLastAbility(IsAdjust, _action),
            LastActionType.Action => CombatData.IsLastAction(IsAdjust, _action),
            _ => false,
        };
    }
}

internal enum LastActionType : byte
{
    GCD,
    Action,
    Ability,
}
