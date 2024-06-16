using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Drawing;

[Description("Drawing Getter")]
[ListUI(61831, NewlineWhenInheritance = true)]
internal abstract class BaseDrawingGetter
{
    [UI("Enable")]
    public bool Enable { get; set; } = true;

    [UI("Name")]
    public string Name { get; set; } = "Unnamed";
    public abstract IDisposable[] GetDrawing();
}
