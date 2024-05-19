using ECommons.DalamudServices;

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

public record struct ObjectBeginCastData(BattleChara Object)
{
    public override readonly string ToString()
    {
        var act = Svc.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Action>()?.GetRow(Object.CastActionId);

        if (act == null) return $"Begin Cast: {RecordString.ObjectStr(Object)}";

        return $"Begin Cast: {RecordString.ObjectStr(Object)} -> {RecordString.ActionStr(act)}";
    }
}

public record struct ObjectNewData(GameObject Object)
{
    public override readonly string ToString()
    {
        return $"New Object: {RecordString.ObjectStr(Object)}";
    }
}

public record struct ObjectEffectData(GameObject Object, ushort Param1, ushort Param2)
{
    public override readonly string ToString()
    {
        return $"Object Effect: {RecordString.ObjectStr(Object)}, P1 {Param1}, P2 {Param2}";
    }
}

public record struct VfxNewData(GameObject Object, string Path)
{
    public override readonly string ToString()
    {
        return $"New Vfx: {RecordString.ObjectStr(Object)}, Path {Path}";
    }
}

public record struct MapEffectData(uint Position, ushort Param1, ushort Param2)
{
    public override readonly string ToString()
    {
        return $"Map Effect: Position {Position}, P1 {Param1}, P2 {Param2}";
    }
}