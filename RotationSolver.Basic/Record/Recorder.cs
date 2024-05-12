using ECommons.DalamudServices;
using ECommons.Hooks.ActionEffectTypes;

namespace RotationSolver.Basic.Record;

internal static class Recorder
{
    private static readonly Dictionary<DateTime, object> _data = new(64);
    public static T[] GetData<T>(Vector2 duration) where T : struct
    {
        return GetData<T>(duration.X, duration.Y);
    }
    public static T[] GetData<T>(double timeStart, double timeEnd) where T : struct
    {
        var now = DateTime.Now;

        List<object> dataInTime = [];
        foreach (var pair in _data)
        {
            var time = (now - pair.Key).TotalSeconds;

            if (time < timeStart || time > timeEnd) continue;
            dataInTime.Add(pair.Value);
        }

        return [.. dataInTime.OfType<T>()];
    }

    public static void Init()
    {
        Svc.Framework.Update += Framework_Update;
    }

    public static void Dispose()
    {
        Svc.Framework.Update -= Framework_Update;
    }

    private static void Framework_Update(Dalamud.Plugin.Services.IFramework framework)
    {
        UpdateObjectNewData();
        UpdateCastingObjectData();
    }

    private static GameObject[] _lastObjs = [];
    private static void UpdateObjectNewData()
    {
        foreach (var obj in Svc.Objects.Except(_lastObjs))
        {
            Enqueue(new ObjectNewData(obj));
        }

        _lastObjs = [.. Svc.Objects];
    }

    private static BattleChara[] _lastCastingObjs = [];
    private static void UpdateCastingObjectData()
    {
        var castingObjects = DataCenter.AllHostileTargets.Where(b => b.IsCasting && b.AdjustedTotalCastTime > 2.5f);

        foreach (var obj in castingObjects.Except(_lastCastingObjs))
        {
            Enqueue(new ObjectBeginCastData(obj));
        }

        _lastCastingObjs = [.. castingObjects];
    }

    public static void Enqueue<T>(T data) where T : struct
    {
        if (_data.Count >= Service.Config.WatcherCount)
        {
            _data.Remove(_data.Keys.First());
        }
        _data[DateTime.Now] = data;

        try
        {
            UpdateNowDutyRotation(data);
        }
        catch (Exception e)
        {
            Svc.Log.Warning(e, "Failed to act on the duty rotation about acion on enemy.");
        }
    }

    private static void UpdateNowDutyRotation(object data)
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
        else if (data is ObjectEffectData objectEffectData)
        {
            rotation.OnObjectEffect(objectEffectData);
        }
        else if (data is ObjectBeginCastData objectBeginCastData)
        {
            rotation.OnStartCasting(objectBeginCastData);
        }
        else if (data is ActionEffectSet actionEffect)
        {
            rotation.OnActionFromEnemy(actionEffect);
        }
    }

    public static void Clear()
    {
        _data.Clear();
    }
}
