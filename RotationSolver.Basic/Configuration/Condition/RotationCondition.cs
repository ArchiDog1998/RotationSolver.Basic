using XIVConfigUI;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Condition;

internal class MethodChoicesAttribute : ChoicesAttribute
{
    protected override Pair[] GetChoices()
    {
        return 
        [
            nameof(CombatData.IsLastGCD),
            nameof(CombatData.IsLastAction),
            nameof(CombatData.IsLastAbility),
        ];
    }
}

internal class IntegerChoicesAttribute : ChoicesAttribute
{
    protected override bool Lazy => false;

    protected override Pair[] GetChoices()
    {
        return [.. DataCenter.RightNowRotation?.AllBytesOrInt.Select(p => new Pair(p.Name, p.Local()))];
    }
}

internal class BoolChoicesAttribute : ChoicesAttribute
{
    protected override bool Lazy => false;

    protected override Pair[] GetChoices()
    {
        return [.. DataCenter.RightNowRotation?.AllBools.Select(p => new Pair(p.Name, p.Local()))];
    }
}

internal class FloatChoicesAttribute : ChoicesAttribute
{
    protected override bool Lazy => false;

    protected override Pair[] GetChoices()
    {
        return [.. DataCenter.RightNowRotation?.AllFloats.Select(p => new Pair(p.Name, p.Local()))];
    }
}

[Obsolete]
[Description("Rotation Condition")]
internal class RotationCondition : DelayConditionBase
{
    [UI("Type")]
    public ComboConditionType ComboConditionType { get; set; } = ComboConditionType.Float;

    internal PropertyInfo? _prop;
    [JsonProperty]
    private string _propertyName = "Not Chosen";

    [JsonIgnore, IntegerChoices]
    [UI("Integer Property", (int) ComboConditionType.Integer, Parent = nameof(ComboConditionType))]
    public string IntegerName { get => _propertyName; set => _propertyName = value; }

    [JsonIgnore, BoolChoices]
    [UI("Bool Property", (int)ComboConditionType.Bool, Parent = nameof(ComboConditionType))]
    public string BoolName { get => _propertyName; set => _propertyName = value; }

    [JsonIgnore, FloatChoices]
    [UI("Float Property", (int)ComboConditionType.Float, Parent = nameof(ComboConditionType))]
    public string FloatName { get => _propertyName; set => _propertyName = value; }

    MethodInfo? _method;
    private string _methodName = "Not Chosen";
    [MethodChoices, UI("Method", (int)ComboConditionType.Last, Parent = nameof(ComboConditionType))]
    public string MethodName { get => _methodName; set => _methodName = value; }

    internal IBaseAction? _action;

    [UI("Action", (int)ComboConditionType.Last, Parent = nameof(ComboConditionType))]
    public ActionID ID { get; set; } = ActionID.None;

    [UI("Comparison", (int)ComboConditionType.Integer,
        (int)ComboConditionType.Float,
        Parent = nameof(ComboConditionType))]
    public Comparison Comparison { get; set; } = Comparison.Bigger;

    [UI("Count", (int)ComboConditionType.Integer,
        Parent = nameof(ComboConditionType))]
    public int Count { get; set; }


    [UI("Value", (int)ComboConditionType.Float,
        Parent = nameof(ComboConditionType))]
    public float Value { get; set; }

    [UI("Adjust", (int)ComboConditionType.Last,
        Parent = nameof(ComboConditionType))]
    public bool IsAdjust { get; set; } = false;

    public override bool? CheckBefore()
    {
        var rotation = DataCenter.RightNowRotation;
        if (rotation == null) return null;
        CheckBaseAction(rotation, ID, ref _action);
        CheckMemberInfo(rotation, ref _propertyName, ref _prop);
        CheckMemberInfo(rotation, ref _methodName, ref _method);
        return base.CheckBefore();
    }

    protected override bool IsTrueInside()
    {
        var rotation = DataCenter.RightNowRotation;
        if (rotation == null) return false;
        switch (ComboConditionType)
        {
            case ComboConditionType.Bool:
                if (_prop == null) return false;
                if (_prop.GetValue(rotation) is bool b)
                {
                    return b;
                }
                return false;

            case ComboConditionType.Integer:
                if (_prop == null) return false;

                var value = _prop.GetValue(rotation);
                if (value is byte by)
                {
                    return Comparison.Compare(by, Count);
                }
                else if (value is int i)
                {
                    return Comparison.Compare(i, Count);
                }
                return false;

            case ComboConditionType.Float:
                if (_prop == null) return false;
                if (_prop.GetValue(rotation) is float fl)
                {
                    return Comparison.Compare(fl, Value);
                }
                return false;

            case ComboConditionType.Last:
                try
                {
                    if (_method?.Invoke(rotation, [IsAdjust, new IAction?[] { _action }]) is bool boo)
                    {
                        return boo;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
        }

        return false;
    }
}

internal enum ComboConditionType : byte
{
    [Description("Boolean")]
    Bool,

    [Description("Byte")]
    Integer,

    [Description("Float")]
    Float,

    [Description("Last")]
    Last,
}
