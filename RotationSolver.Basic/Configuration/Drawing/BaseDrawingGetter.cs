namespace RotationSolver.Basic.Configuration.Drawing; 

internal abstract class BaseDrawingGetter
{
    public bool Enable { get; set; } = true;
    public string Name { get; set; } = "Unnamed";
    public abstract IDisposable[] GetDrawing();
}
