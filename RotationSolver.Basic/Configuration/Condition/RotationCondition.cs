﻿namespace RotationSolver.Basic.Configuration.Condition;

internal class RotationIntegerChoicesAttribute : IntegerChoicesAttribute
{
    protected override Type? FindType()
    {
        var rotation = DataCenter.RightNowDutyRotation;
        if (rotation == null) return null;
        return rotation.GetType();
    }
}

internal class RotationBoolChoicesAttribute : BoolChoicesAttribute
{
    protected override Type? FindType()
    {
        var rotation = DataCenter.RightNowDutyRotation;
        if (rotation == null) return null;
        return rotation.GetType();
    }
}

internal class RotationFloatChoicesAttribute : FloatChoicesAttribute
{
    protected override Type? FindType()
    {
        var rotation = DataCenter.RightNowDutyRotation;
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

    public override bool? CheckBefore()
    {
        var rotation = DataCenter.RightNowRotation;
        if (rotation == null) return null;
        CheckMemberInfo(rotation, ref _propertyName, ref _prop);
        return base.CheckBefore();
    }
}