using ECommons.DalamudServices;

namespace RotationSolver.Basic.Configuration.Condition;

internal class RotationIntegerChoicesAttribute : IntegerChoicesAttribute
{
    protected override Type? FindType()
    {
        var rotation = DataCenter.RightNowRotation;
        if (rotation == null) return null;
        return rotation.GetType();
    }
}

internal class RotationBoolChoicesAttribute : BoolChoicesAttribute
{
    protected override Type? FindType()
    {
        var rotation = DataCenter.RightNowRotation;
        if (rotation == null) return null;
        return rotation.GetType();
    }
}

internal class RotationFloatChoicesAttribute : FloatChoicesAttribute
{
    protected override Type? FindType()
    {
        var rotation = DataCenter.RightNowRotation;
        if (rotation == null) return null;
        return rotation.GetType();
    }
}

internal class RotationEnumerableChoicesAttribute : EnumerableChoicesAttribute
{
    protected override Type? FindType()
    {
        var rotation = DataCenter.RightNowRotation;
        if (rotation == null) return null;
        return rotation.GetType();
    }
}

[Description("Rotation Condition")]
internal class RotationCondition : PropertyConditionBase
{
    [RotationIntegerChoices]
    public override string IntegerName { get => base.IntegerName; set => base.IntegerName = value; }

    [RotationBoolChoices]
    public override string BoolName { get => base.BoolName; set => base.BoolName = value; }

    [RotationFloatChoices]
    public override string FloatName { get => base.FloatName; set => base.FloatName = value; }

    [RotationEnumerableChoices]
    public override string EnumName { get => base.EnumName; set => base.EnumName = value; }
    public override Comparison Comparison { get => base.Comparison; set => base.Comparison = value; }
    public override float Value { get => base.Value; set => base.Value = value; }
    public override int Count { get => base.Count; set => base.Count = value; }
    public byte RotationData { get; set; }
    public override bool? CheckBefore()
    {
        var rotation = DataCenter.RightNowRotation;
        if (rotation == null) return null;
        CheckMemberInfo(rotation, ref _propertyName, ref _prop);
        return base.CheckBefore();
    }
}