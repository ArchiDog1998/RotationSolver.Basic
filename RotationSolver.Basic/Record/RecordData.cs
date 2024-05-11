namespace RotationSolver.Basic.Watcher;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record ObjectNewData(GameObject Object) : RecordData;

public record ObjectEffectData(uint ObjectId, ushort Param1, ushort Param2) : RecordData;

public record MapEffectData(uint Position, ushort Param1, ushort Param2) : RecordData;

public record VfxNewData(uint ObjectId, string Path) : RecordData;

public record RecordData
{
    private readonly DateTime _time = DateTime.Now;

    /// <summary>
    /// The time duration.
    /// </summary>
    public TimeSpan TimeDuration => DateTime.Now - _time;
}
