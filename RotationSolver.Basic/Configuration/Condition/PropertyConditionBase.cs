using XIVConfigUI;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Condition;

internal abstract class PropertyChoicesAttribute : ChoicesAttribute
{
    protected sealed override bool Lazy => false;

    protected sealed override Pair[] GetChoices()
    {
        return [.. FindProperties().Select(p => new Pair(p.Name, p.Local()))];
    }

    protected abstract IEnumerable<PropertyInfo> FindProperties();
}

internal abstract class IntegerChoicesAttribute : PropertyChoicesAttribute
{
    protected override IEnumerable<PropertyInfo> FindProperties()
    {
        return FindType().GetStaticProperties<byte>().Union(GetType().GetStaticProperties<int>());
    }
    protected abstract Type? FindType();
}

internal abstract class BoolChoicesAttribute : PropertyChoicesAttribute
{
    protected override IEnumerable<PropertyInfo> FindProperties()
    {
        return FindType().GetStaticProperties<bool>();
    }
    protected abstract Type? FindType();
}

internal abstract class FloatChoicesAttribute : PropertyChoicesAttribute
{
    protected override IEnumerable<PropertyInfo> FindProperties()
    {
        return FindType().GetStaticProperties<float>();
    }
    protected abstract Type? FindType();
}

internal abstract class PropertyConditionBase : DelayConditionBase
{
    [UI("Type")]
    public PropertyConditionType PropertyCondition { get; set; } = PropertyConditionType.Float;

    internal PropertyInfo? _prop;
    [JsonProperty]
    protected string _propertyName = "Not Chosen";

    [JsonIgnore]
    [UI("Integer Property", (int)PropertyConditionType.Integer, Parent = nameof(PropertyCondition))]
    public virtual string IntegerName { get => _propertyName; set => _propertyName = value; }

    [JsonIgnore]
    [UI("Bool Property", (int)PropertyConditionType.Bool, Parent = nameof(PropertyCondition))]
    public virtual string BoolName { get => _propertyName; set => _propertyName = value; }

    [JsonIgnore]
    [UI("Float Property", (int)PropertyConditionType.Float, Parent = nameof(PropertyCondition))]
    public virtual string FloatName { get => _propertyName; set => _propertyName = value; }

    [UI("Comparison", (int)PropertyConditionType.Integer,
        (int)PropertyConditionType.Float,
        Parent = nameof(PropertyCondition))]
    public Comparison Comparison { get; set; } = Comparison.Bigger;

    [UI("Count", (int)PropertyConditionType.Integer,
    Parent = nameof(PropertyCondition))]
    public int Count { get; set; }

    [UI("Value", (int)PropertyConditionType.Float,
        Parent = nameof(PropertyCondition))]
    public float Value { get; set; }

    protected override bool IsTrueInside()
    {
        if (_prop == null) return false;

        switch (PropertyCondition)
        {
            case PropertyConditionType.Bool:
                if (_prop.GetValue(null) is bool b)
                {
                    return b;
                }
                return false;

            case PropertyConditionType.Integer:
                var value = _prop.GetValue(null);
                if (value is byte by)
                {
                    return Comparison.Compare(by, Count);
                }
                else if (value is int i)
                {
                    return Comparison.Compare(i, Count);
                }
                return false;

            case PropertyConditionType.Float:
                if (_prop.GetValue(null) is float fl)
                {
                    return Comparison.Compare(fl, Value);
                }
                return false;
        }
        return false;
    }
}

internal enum PropertyConditionType : byte
{
    [Description("Boolean")]
    Bool,

    [Description("Byte")]
    Integer,

    [Description("Float")]
    Float,
}
