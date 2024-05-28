using ECommons.DalamudServices;
using ECommons.Hooks.ActionEffectTypes;
using RotationSolver.Basic.Configuration.Trigger;

namespace RotationSolver.Basic.Record;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public interface IRecordData
{
    TriggerData ToTriggerData();
}

public record struct ObjectBeginCastData(BattleChara Object) : IRecordData
{
    public override readonly string ToString() => ToTriggerData().ToString();

    public readonly TriggerData ToTriggerData()
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
    public override readonly string ToString() => ToTriggerData().ToString();

    public readonly TriggerData ToTriggerData()
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
    public override readonly string ToString() => ToTriggerData().ToString();

    public readonly TriggerData ToTriggerData()
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
    public override readonly string ToString() => ToTriggerData().ToString();

    public readonly TriggerData ToTriggerData()
    {
        return new()
        {
            Type = TriggerDataType.VfxNew,
            ObjectDataId = Object.DataId,
            Path = Path,
        };
    }
}

public readonly record struct MapEffectData(uint Position, ushort Param1, ushort Param2) : IRecordData
{
    public override string ToString() => ToTriggerData().ToString();

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

public readonly record struct ActionEffectSetData(ActionEffectSet Set) : IRecordData
{
    public uint ObjectId => Set.Source.DataId;
    public uint ActionId => Set.Action.RowId;
    public override string ToString() => ToTriggerData().ToString();

    public TriggerData ToTriggerData()
    {
        return new()
        {
            Type = TriggerDataType.ActionEffect,
            ObjectDataId = Set.Source.DataId,
            PositionOrActionId = Set.Action.RowId,
        };
    }
}