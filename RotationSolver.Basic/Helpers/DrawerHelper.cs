namespace RotationSolver.Basic.Helpers;
internal static class DrawerHelper
{
    private static IDisposable[]? _previewItems = null;

    public static void Draw(Func<IDisposable[]> itemsGetter)
    {
        if (_previewItems == null)
        {
            _previewItems = itemsGetter();
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
