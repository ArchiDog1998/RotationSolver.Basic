using RotationSolver.Basic.Configuration.Drawing;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.TerritoryAction;
internal class DrawingAction : ITerritoryAction
{
    [UI("Drawings")]
    public List<BaseDrawingGetter> DrawingGetters { get; set; } = [];

    private IDisposable[] _drawings = [];

    public void Enable()
    {
        foreach (var item in _drawings)
        {
            item.Dispose();
        }

        if (Service.Config.ShowDrawing && Service.ToDrawing != null)
        {
            _drawings = [.. DrawingGetters.Where(i => i.Enable).SelectMany(i => i.GetDrawing().Select(Service.ToDrawing).OfType<IDisposable>())];
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
