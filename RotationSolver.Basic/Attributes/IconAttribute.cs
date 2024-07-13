namespace RotationSolver.Basic.Attributes;

[AttributeUsage(AttributeTargets.Field)]
internal class IconAttribute(uint icon) : Attribute
{
    internal uint Icon => icon;
}
