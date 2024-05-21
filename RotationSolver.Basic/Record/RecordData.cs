using ECommons.DalamudServices;
using ECommons.Hooks.ActionEffectTypes;
using RotationSolver.Basic.Configuration.Trigger;

namespace RotationSolver.Basic.Record;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
internal static class RecordString
{
    public static string ObjectStr(GameObject obj)
    {
        return $"{obj.Name} ({obj.DataId})";
    }

    public static string ActionStr(Lumina.Excel.GeneratedSheets.Action act)
    {
        return $"{act.Name} ({act.RowId})";
    }
}

public interface IRecordData
{
    TriggerData ToTriggerData();
}

public record struct ActionEffectSetData(ActionEffectSet set) : IRecordData
{
    public TriggerData ToTriggerData()
    {
        return new()
        {
            Type = TriggerDataType.ActionEffect,
            ObjectDataId =set.Source.DataId,
            PositionOrActionId = set.Action.RowId,
        };
    }
}
public record struct ObjectBeginCastData(BattleChara Object) : IRecordData
{
    public override readonly string ToString()
    {
        var act = Svc.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Action>()?.GetRow(Object.CastActionId);

        if (act == null) return $"Begin Cast: {RecordString.ObjectStr(Object)}";

        return $"Begin Cast: {RecordString.ObjectStr(Object)} -> {RecordString.ActionStr(act)}";
    }

    public TriggerData ToTriggerData()
    {
        return new()
        {
            Type = TriggerDataType.ObjectBeginCast,
            ObjectDataId = Object.DataId,
            PositionOrActionId = Object.CastActionId,
        };
    }
}

public record struct ObjectNewData(GameObject Object) : IRecordData
{
    public override readonly string ToString()
    {
        return $"New Object: {RecordString.ObjectStr(Object)}";
    }

    public TriggerData ToTriggerData()
    {
        return new()
        {
            Type = TriggerDataType.ObjectNew,
            ObjectDataId = Object.DataId,
        };
    }
}

public record struct ObjectEffectData(GameObject Object, ushort Param1, ushort Param2) : IRecordData
{
    public override readonly string ToString()
    {
        return $"Object Effect: {RecordString.ObjectStr(Object)}, P1 {Param1}, P2 {Param2}";
    }

    public TriggerData ToTriggerData()
    {
        return new()
        {
            Type = TriggerDataType.ObjectEffect,
            ObjectDataId = Object.DataId,
            Param1 = Param1,
            Param2 = Param2,
        };
    }
}

public record struct VfxNewData(GameObject Object, string Path) : IRecordData
{
    public override readonly string ToString()
    {
        return $"New Vfx: {RecordString.ObjectStr(Object)}, Path {Path}";
    }

    public TriggerData ToTriggerData()
    {
        return new()
        {
            Type = TriggerDataType.VfxNew,
            ObjectDataId = Object.DataId,
            Path = Path,
        };
    }
}

public record struct MapEffectData(uint Position, ushort Param1, ushort Param2) : IRecordData
{
    public override readonly string ToString()
    {
        return $"Map Effect: Position {Position}, P1 {Param1}, P2 {Param2}";
    }

    public TriggerData ToTriggerData()
    {
        return new()
        {
            Type = TriggerDataType.MapEffect,
            PositionOrActionId = Position,
            Param1 = Param1,
            Param2 = Param2,
        };
    }
}