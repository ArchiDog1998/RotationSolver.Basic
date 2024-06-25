namespace RotationSolver.Basic.Data;

internal static class ComparisonExtension
{
    public static bool Compare(this Comparison comparison, int value1, int value2)
    {
        return comparison switch
        {
            Comparison.Bigger => value1 > value2,
            Comparison.BiggerOrEqual => value1 >= value2,
            Comparison.Smaller => value1 < value2,
            Comparison.SmallerOrEqual => value1 <= value2,
            Comparison.Equal => value1 == value2,
            _ => false,
        };
    }

    public static bool Compare(this Comparison comparison, float value1, float value2)
    {
        return comparison switch
        {
            Comparison.Bigger => value1 > value2,
            Comparison.BiggerOrEqual => value1 >= value2,
            Comparison.Smaller => value1 < value2,
            Comparison.SmallerOrEqual => value1 <= value2,
            Comparison.Equal => value1 == value2,
            _ => false,
        };
    }
}

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
