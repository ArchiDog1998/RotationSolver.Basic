namespace RotationSolver.Basic.Data;

internal enum Comparison : byte
{
    [Description(">")]
    Bigger,

    [Description("<")]
    Smaller,

    [Description("=")]
    Equal,

    [Description(">=")]
    BiggerOrEqual,

    [Description("<=")]
    SmallerOrEqual,
}
