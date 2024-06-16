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

    public override bool CheckBefore(ICustomRotation rotation)
    {
        return CheckBaseAction(rotation, ID, ref _action) && base.CheckBefore(rotation);
    }

    protected override bool IsTrueInside(ICustomRotation rotation)
    {
        if (_action == null) return false;

        switch (ActionConditionType)
        {
            case ActionConditionType.Elapsed:
                return _action.CD.ElapsedOneChargeAfter(Time); // Bigger

            case ActionConditionType.ElapsedGCD:
                return _action.CD.ElapsedOneChargeAfterGCD((uint)Gcd, Offset); // Bigger

            case ActionConditionType.Remain:
                return !_action.CD.WillHaveOneCharge(Time); //Smaller

            case ActionConditionType.RemainGCD:
                return !_action.CD.WillHaveOneChargeGCD((uint)Gcd, Offset); // Smaller

            case ActionConditionType.CanUse:
                return _action.CanUse(out _, CanUse);

            case ActionConditionType.EnoughLevel:
                return _action.EnoughLevel;

            case ActionConditionType.IsCoolDown:
                return _action.CD.IsCoolingDown;

            case ActionConditionType.CurrentCharges:
                switch (Comparison)
                {
                    case Comparison.Bigger:
                        return _action.CD.CurrentCharges > Count;

                    case Comparison.BiggerOrEqual:
                        return _action.CD.CurrentCharges >= Count;

                    case Comparison.Smaller:
                        return _action.CD.CurrentCharges < Count;

                    case Comparison.SmallerOrEqual:
                        return _action.CD.CurrentCharges <= Count;

                    case Comparison.Equal:
                        return _action.CD.CurrentCharges == Count;
                }
                break;

            case ActionConditionType.MaxCharges:
                switch (Comparison)
                {
                    case Comparison.Bigger:
                        return _action.CD.MaxCharges > Count;

                    case Comparison.BiggerOrEqual:
                        return _action.CD.MaxCharges >= Count;

                    case Comparison.Smaller:
                        return _action.CD.MaxCharges < Count;

                    case Comparison.SmallerOrEqual:
                        return _action.CD.MaxCharges <= Count;

                    case Comparison.Equal:
                        return _action.CD.MaxCharges == Count;
                }
                break;
        }
        return false;
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
