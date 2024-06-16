using ECommons.DalamudServices;
using ECommons.GameHelpers;
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

    public override bool CheckBefore(ICustomRotation rotation)
    {
        return CheckBaseAction(rotation, ID, ref _action) && base.CheckBefore(rotation);
    }
    protected override bool IsTrueInside(ICustomRotation rotation)
    {
        var tar = TargetType switch
        {
            TargetType.Action => _action?.TargetInfo.FindTarget(true, false)?.Target,
            TargetType.Target => Svc.Targets.Target as BattleChara,
            TargetType.HostileTarget => DataCenter.HostileTarget,
            TargetType.Player => Player.Object,
            _ => null,
        };

        if (tar == null) return false;

        return Condition.IsTrue(tar);
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