namespace RotationSolver.Basic.Data;

internal static class LogicalTypeExtension
{
    public static bool IsTrue<T>(this LogicalType logicalType, List<T?> items, Func<T, bool?>prediction)
    {
        var myItems = items.OfType<T>();

        if(!myItems.Any()) return false;

        return logicalType switch
        {
            LogicalType.All => myItems.All(c => prediction(c) ?? false),
            LogicalType.Any => myItems.Any(c => prediction(c) ?? false),
            LogicalType.NotAll => !myItems.All(c => prediction(c) ?? false),
            LogicalType.NotAny => !myItems.Any(c => prediction(c) ?? false),
            _ => false,
        };
    }
}

internal enum LogicalType : byte
{
    [Description("&&")]
    All,

    [Description(" | | ")]
    Any,

    [Description("!&&")]
    NotAll,

    [Description("! | | ")]
    NotAny,
}
