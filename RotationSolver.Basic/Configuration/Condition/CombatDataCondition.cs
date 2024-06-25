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

[Description("Combat Data Condition")]
internal class CombatDataCondition : PropertyConditionBase
{
    [CombatDataIntegerChoices]
    public override string IntegerName { get => base.IntegerName; set => base.IntegerName = value; }

    [CombatDataBoolChoices]
    public override string BoolName { get => base.BoolName; set => base.BoolName = value; }

    [CombatDataFloatChoices]
    public override string FloatName { get => base.FloatName; set => base.FloatName = value; }

    public override bool? CheckBefore()
    {
        CheckMemberInfo(typeof(CombatData), ref _propertyName, ref _prop);
        return base.CheckBefore();
    }
}
