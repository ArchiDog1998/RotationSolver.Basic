using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Condition;

[Description("Action Condition")]
internal class ActionCondition : DelayConditionBase
{
    internal IBaseAction? _action = null;

    [UI("Action Id")]
    public ActionID ID { get; set; } = ActionID.None;

    [UI("Type")]
    public ActionConditionType ActionConditionType { get; set; } = ActionConditionType.Elapsed;

    [Range(0,0, ConfigUnitType.Seconds)]
    [UI("Time", (int)ActionConditionType.Elapsed,
        (int)ActionConditionType.Remain,
        Parent = nameof(ActionConditionType))]
    public float Time { get; set; }

    [UI("Gcd", (int)ActionConditionType.ElapsedGCD,
        (int)ActionConditionType.RemainGCD,
        Parent = nameof(ActionConditionType))]
    public int Gcd { get; set; }

    [UI("Offset", (int)ActionConditionType.ElapsedGCD,
        (int)ActionConditionType.RemainGCD,
        Parent = nameof(ActionConditionType))]
    public float Offset { get; set; }

    [UI("Can Use", (int)ActionConditionType.CanUse,
        Parent = nameof(ActionConditionType))]
    public CanUseOption CanUse { get; set; } = CanUseOption.None;

    [UI("Comparison", (int)ActionConditionType.CurrentCharges,
        (int)ActionConditionType.MaxCharges,
        Parent = nameof(ActionConditionType))]
    public Comparison Comparison { get; set; } = Comparison.Bigger;

    [UI("Count", (int)ActionConditionType.CurrentCharges,
        (int)ActionConditionType.MaxCharges,
        Parent = nameof(ActionConditionType))]
    public int Count { get; set; }

    public override bool? CheckBefore()
    {
        var rotation = DataCenter.RightNowRotation;
        if (rotation == null) return null;
        var basic = base.CheckBefore();
        if (basic == null) return null;
        return basic.Value && CheckBaseAction(rotation, ID, ref _action);
    }

    protected override bool IsTrueInside()
    {
        if (_action == null) return false;

        return ActionConditionType switch
        {
            ActionConditionType.Elapsed => _action.CD.ElapsedOneChargeAfter(Time),// Bigger
            ActionConditionType.ElapsedGCD => _action.CD.ElapsedOneChargeAfterGCD((uint)Gcd, Offset),// Bigger
            ActionConditionType.Remain => !_action.CD.WillHaveOneCharge(Time),//Smaller
            ActionConditionType.RemainGCD => !_action.CD.WillHaveOneChargeGCD((uint)Gcd, Offset),// Smaller
            ActionConditionType.CanUse => _action.CanUse(out _, CanUse),
            ActionConditionType.EnoughLevel => _action.EnoughLevel,
            ActionConditionType.IsCoolDown => _action.CD.IsCoolingDown,
            ActionConditionType.CurrentCharges => Comparison.Compare(_action.CD.CurrentCharges, Count),
            ActionConditionType.MaxCharges => Comparison.Compare(_action.CD.MaxCharges, Count),
            _ => false,
        };
    }
}

internal enum ActionConditionType : byte
{
    [Description("Elapsed")]
    Elapsed,

    [Description("Elapsed GCD")]
    ElapsedGCD,

    [Description("Remain Time")]
    Remain,

    [Description("Remain Time GCD")]
    RemainGCD,

    [Description("Can Use")]
    CanUse,

    [Description("Enough Level")]
    EnoughLevel,

    [Description("Is CoolDown")]
    IsCoolDown,

    [Description("Current Charges")]
    CurrentCharges,

    [Description("Max Charges")]
    MaxCharges,
}
