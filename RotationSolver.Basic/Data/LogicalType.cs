namespace RotationSolver.Basic.Data;

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
