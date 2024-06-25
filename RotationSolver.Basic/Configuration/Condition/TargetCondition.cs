using Lumina.Excel.GeneratedSheets;
using RotationSolver.Basic.Configuration.Target;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Condition;

[Description("Target Condition")]
internal class TargetCondition : DelayConditionBase
{
    [UI("Type")]
    public TargetType TargetType { get; set; } = TargetType.Action;

    internal IBaseAction? _action;
    [UI("Action", (int)TargetType.Action, Parent = nameof(TargetType))]
    public ActionID ID { get; set; } = ActionID.None;

    [UI("Condition")]
    public TargetingConditionSet Condition { get; set; } = new();

    public override bool? CheckBefore()
    {
        var rotation = DataCenter.RightNowRotation;
        if (rotation == null) return null;
        var basic = base.CheckBefore();
        if (basic == null) return null;
        return CheckBaseAction(rotation, ID, ref _action);
    }
    protected override bool IsTrueInside()
    {
        var tar = TargetType switch
        {
            TargetType.Action => _action?.TargetInfo.FindTarget(true, false)?.Target,
            TargetType.Target => CombatData.CurrentTarget,
            TargetType.HostileTarget => CombatData.HostileTarget,
            TargetType.Player => CombatData.Player,
            _ => null,
        };

        if (tar == null) return false;

        return Condition.IsTrue(tar) ?? false;
    }
}

internal enum TargetType : byte
{
    [Description("Action")]
    Action,

    [Description("Hostile Target")]
    HostileTarget,

    [Description("Player")]
    Player,

    [Description("Target")]
    Target,
}