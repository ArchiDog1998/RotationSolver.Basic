using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Drawing;

internal class DrawingGetterAttribute : ListUIAttribute
{
    public DrawingGetterAttribute() : base(61831)
    {
        NewlineWhenInheritance = true;
        Description = "Click to show the preview drawings";
    }

    public override void OnClick(object obj)
    {
        if (obj is not BaseDrawingGetter getter) return;
        DrawerHelper.Draw(getter.GetDrawing);
    }
}

[DrawingGetter, Description("Drawing Item")]
internal abstract class BaseDrawingGetter
{
    [UI("Enable")]
    public bool Enable { get; set; } = true;

    [UI("Name")]
    public string Name { get; set; } = "Unnamed";
    public abstract IDisposable[] GetDrawing();
}
