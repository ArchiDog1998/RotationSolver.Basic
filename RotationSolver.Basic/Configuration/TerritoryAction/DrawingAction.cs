using RotationSolver.Basic.Configuration.Drawing;

namespace RotationSolver.Basic.Configuration.TerritoryAction;
internal class DrawingAction : ITerritoryAction
{
    public List<BaseDrawingGetter> DrawingGetters { get; set; } = [];

    private IDisposable[] _drawings = [];

    public void Enable()
    {
        foreach (var item in _drawings)
        {
            item.Dispose();
        }

        if (Service.Config.ShowDrawing)
        {
            _drawings = [.. DrawingGetters.Where(i => i.Enable).SelectMany(i => i.GetDrawing())];
        }
        else
        {
            _drawings = [];
        }
    }

    public void Disable()
    {
        foreach (var item in _drawings)
        {
            item.Dispose();
        }
        _drawings = [];
    }
}
