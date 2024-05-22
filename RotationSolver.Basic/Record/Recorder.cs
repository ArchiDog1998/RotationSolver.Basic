using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons.DalamudServices;
using ECommons.Hooks.ActionEffectTypes;
using ECommons.Schedulers;
using RotationSolver.Basic.Configuration;

namespace RotationSolver.Basic.Record;

internal static class Recorder
{
    public static List<(DateTime, IRecordData)> Data { get; } = new(64);
    public static T[] GetData<T>(Vector2 duration) where T : struct, IRecordData
    {
        return GetData<T>(duration.X, duration.Y);
    }
    public static T[] GetData<T>(double timeStart, double timeEnd) where T : struct, IRecordData
    {
        var now = DateTime.Now;

        List<object> dataInTime = [];
        foreach ((var createdTime, var data) in Data)
        {
            var time = (now - createdTime).TotalSeconds;

            if (time < timeStart || time > timeEnd) continue;
            dataInTime.Add(data);
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
        var objs = Svc.Objects.Where(obj => obj is not PlayerCharacter);

        foreach (var obj in objs.Except(_lastObjs))
        {
            Enqueue(new ObjectNewData(obj));
        }

        _lastObjs = [.. objs];
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

    public static void Enqueue<T>(T data) where T : struct, IRecordData
    {
        if (Data.Count >= Service.Config.WatcherCount)
        {
            Data.RemoveAt(0);
        }

        Data.Add((DateTime.Now, data));

        if (OtherConfiguration.TerritoryConfig.Trigger.TryGetValue(data.ToTriggerData(), out var items))
        {
            foreach (var item in items)
            {
                _ = new TickScheduler(item.TerritoryAction.Enable, (long)(item.StartTime * 1000));
                _ = new TickScheduler(item.TerritoryAction.Disable, (long)((item.StartTime + item.Duration) * 1000));
            }
        }

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
        else if (data is ActionEffectSet actionEffect)
        {
            rotation.OnActionFromEnemy(actionEffect);
        }
    }

    public static void Clear()
    {
        Data.Clear();
    }
}
