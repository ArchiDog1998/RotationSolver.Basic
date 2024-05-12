namespace RotationSolver.Basic.Record;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public record struct ObjectBeginCastData(GameObject? Object)
{
    public override readonly string ToString()
    {
        return base.ToString() ?? string.Empty;
    }
}

public record struct ObjectNewData(GameObject? Object)
{
    public override readonly string ToString()
    {
        return base.ToString() ?? string.Empty;
    }
}

public record struct ObjectEffectData(GameObject? Object, ushort Param1, ushort Param2)
{
    public override readonly string ToString()
    {
        return base.ToString() ?? string.Empty;
    }
}

public record struct VfxNewData(GameObject? Object, string Path)
{
    public override readonly string ToString()
    {
        return base.ToString() ?? string.Empty;
    }
}

public record struct MapEffectData(uint Position, ushort Param1, ushort Param2)
{
    public override readonly string ToString()
    {
        return base.ToString() ?? string.Empty;
    }
}