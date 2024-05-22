using ECommons.DalamudServices;
using Lumina.Excel.GeneratedSheets;

namespace RotationSolver.Basic.Configuration.Trigger;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
internal static class RecordString
{
    public static string ObjectStr(uint dataId)
    {
        var npc = Service.GetSheet<BNpcName>().GetRow(dataId);
        return $"{npc?.Singular ?? "Unknown Object"} ({dataId})";
    }

    public static string ActionStr(uint actionId)
    {
        var act = Svc.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.Action>()?.GetRow(actionId);

        return $"{act?.Name ?? "Unknown Action"} ({actionId})";
    }
}

public enum TriggerDataType : byte
{
    ObjectBeginCast,
    ObjectNew,
    ObjectEffect,
    VfxNew,
    MapEffect,
    ActionEffect,
}

public readonly record struct TriggerData
{
    public TriggerDataType Type { get; init; }
    public uint ObjectDataId { get; init; }
    public uint PositionOrActionId { get; init; }
    public ushort Param1 { get; init; }
    public ushort Param2 { get; init; }
    public string Path { get; init; }

    public override string ToString()
    {
        return Type switch
        {
            TriggerDataType.ObjectBeginCast => $"Begin Cast: {RecordString.ObjectStr(ObjectDataId)} -> {RecordString.ActionStr(PositionOrActionId)}",
            TriggerDataType.ObjectNew => $"New Object: {RecordString.ObjectStr(ObjectDataId)}",
            TriggerDataType.ObjectEffect => $"Object Effect: {RecordString.ObjectStr(ObjectDataId)}, P1 {Param1}, P2 {Param2}",
            TriggerDataType.VfxNew => $"New Vfx: {RecordString.ObjectStr(ObjectDataId)}, Path {Path}",
            TriggerDataType.MapEffect => $"Map Effect: Position {PositionOrActionId}, P1 {Param1}, P2 {Param2}",
            TriggerDataType.ActionEffect => $"Action Effect: {RecordString.ObjectStr(ObjectDataId)} -> {RecordString.ActionStr(PositionOrActionId)}",
            _ => base.ToString() ?? string.Empty,
        };
    }
}
