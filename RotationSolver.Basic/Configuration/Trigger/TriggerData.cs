namespace RotationSolver.Basic.Configuration.Trigger;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
}
