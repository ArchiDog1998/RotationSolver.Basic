namespace RotationSolver.Basic.Helpers;

internal static class DrawerHelper
{
    private static IDisposable[]? _previewItems = null;

    public static void Draw(Func<OmenData[]> itemsGetter)
    {
        if (_previewItems == null && Service.ToDrawing is not null)
        {
            _previewItems = itemsGetter().Select(Service.ToDrawing).OfType<IDisposable>().ToArray();
        }
        else
        {
            ClearDrawings();
        }
    }

    public static void ClearDrawings()
    {
        if (_previewItems == null) return;

        foreach (var preview in _previewItems)
        {
            preview.Dispose();
        }
        _previewItems = null;
    }
}
