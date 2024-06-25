
namespace RotationSolver.Basic.Configuration.Condition;

internal class CombatDataIntegerChoicesAttribute : IntegerChoicesAttribute
{
    protected override Type? FindType() => typeof(CombatData);
}
internal class CombatDataBoolChoicesAttribute : BoolChoicesAttribute
{
    protected override Type? FindType() => typeof(CombatData);
}

internal class CombatDataFloatChoicesAttribute : FloatChoicesAttribute
{
    protected override Type? FindType() => typeof(CombatData);
}

internal class CombatDataEnumerableChoicesAttribute : EnumerableChoicesAttribute
{
    protected override Type? FindType() => typeof(CombatData);
}

[Description("Combat Data Condition")]
internal class CombatDataCondition : PropertyConditionBase
{
    [CombatDataIntegerChoices]
    public override string IntegerName { get => base.IntegerName; set => base.IntegerName = value; }

    [CombatDataBoolChoices]
    public override string BoolName { get => base.BoolName; set => base.BoolName = value; }

    [CombatDataFloatChoices]
    public override string FloatName { get => base.FloatName; set => base.FloatName = value; }

    [CombatDataEnumerableChoices]
    public override string EnumName { get => base.EnumName; set => base.EnumName = value; }

    public override Comparison Comparison { get => base.Comparison; set => base.Comparison = value; }
    public override float Value { get => base.Value; set => base.Value = value; }
    public override int Count { get => base.Count; set => base.Count = value; }
    public byte CombatData { get; set; }
    public override bool? CheckBefore()
    {
        CheckMemberInfo(typeof(CombatData), ref _propertyName, ref _prop);
        return base.CheckBefore();
    }
}
