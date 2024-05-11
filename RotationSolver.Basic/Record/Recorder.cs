using ECommons.DalamudServices;
using RotationSolver.Basic.Watcher;

namespace RotationSolver.Basic.Watch;
internal static class Recorder
{
    private static readonly Queue<RecordData> _data = new(64);

    /// <summary>
    /// The map effects.
    /// </summary>
    public static IEnumerable<MapEffectData> MapEffects => _data.OfType<MapEffectData>().Reverse();

    /// <summary>
    /// The object Effects.
    /// </summary>
    public static IEnumerable<ObjectEffectData> ObjectEffects => _data.OfType<ObjectEffectData>().Reverse();

    /// <summary>
    /// The vfx effects.
    /// </summary>
    public static IEnumerable<VfxNewData> VfxNewData => _data.OfType<VfxNewData>().Reverse();

    public static void Init()
    {
        Svc.Framework.Update += Framework_Update;
    }

    public static void Dispose()
    {
        Svc.Framework.Update -= Framework_Update;

    }

    private static GameObject[] _lastObjs = [];

    private static void Framework_Update(Dalamud.Plugin.Services.IFramework framework)
    {
        foreach (var obj in Svc.Objects.Except(_lastObjs))
        {
            Enqueue(new ObjectNewData(obj));
        }

        _lastObjs = [.. Svc.Objects];
    }

    public static void Enqueue(RecordData data)
    {
        if (_data.Count >= Service.Config.WatcherCount)
        {
            _data.TryDequeue(out _);
        }
        _data.Enqueue(data);

        UpdateNowDutyRotation(data);
    }

    private static void UpdateNowDutyRotation(RecordData data)
    {
        var rotation = DataCenter.RightNowDutyRotation;
        if (rotation == null) return;

        if (data is ObjectNewData objectNew)
        {
            rotation.OnNewActor(objectNew);
        }
        else if (data is VfxNewData vfxNew)
        {
            rotation.OnActorVfxNew(vfxNew);
        }
        else if (data is MapEffectData mapEffectData)
        {
            rotation.OnMapEffect(mapEffectData);
        }
        else if(data is ObjectEffectData objectEffectData)
        {
            rotation.OnObjectEffect(objectEffectData);
        }
    }

    public static void Clear()
    {
        _data.Clear();
    }
}
