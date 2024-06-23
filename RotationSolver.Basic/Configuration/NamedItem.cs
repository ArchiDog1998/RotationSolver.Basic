using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration;

[ListUI(61512)]
[Description("Named Item")]
internal class NamedItem<T> where T : new()
{
    [UI]
    public string Name { get; set; } = string.Empty;

    [UI]
    public T Item { get; set; } = new();
}
