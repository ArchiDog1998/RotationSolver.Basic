namespace RotationSolver.Basic.Attributes;

[AttributeUsage(AttributeTargets.Property)]
internal class StatusSourceAttribute(StatusType type) : Attribute
{
    public StatusType Type => type;
}
