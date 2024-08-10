using RotationSolver.Basic.Configuration;

namespace RotationSolver.Basic.Data;

public enum OmenDataType : byte
{
    Static,
    Channeling,
    LockOn,
}

public readonly struct OmenData
{
    public IDisposable? Item { get; }
    public OmenDataType Type { get; }
    public string? Path { get; }
    public LocationDescription Location { get; }
    public Vector2 Scale { get; }
    public Vector4 Color { get; }
    public OmenData(IDisposable item)
    {
        Item = item;
    }

    public OmenData(
        OmenDataType type,
        string path,
        LocationDescription location,
        Vector2 scale,
        Vector4 color)
    {
        Type = type;
        Path = path;
        Location = location;
        Scale = scale;
        Color = color;
    }
}

public readonly struct LocationDescription()
{
    public IGameObject? Object { get; init; }
    public IGameObject? Target { get; init; }
    public Vector3 Position { get; init; } = default;
    public float Rotation { get; init; }

    public LocationDescription(IGameObject obj) : this()
    {
        Object = obj;
    }

    public LocationDescription(Vector3 pos) : this()
    {
        Position = pos;
    }

    internal LocationDescription(Position pos) : this((Vector3)pos)
    {
        
    }

    public static implicit operator LocationDescription(Vector3 position) => new(position);
}

/// <summary>
/// "vfx/lockon/eff/{Name}.avfx".
/// </summary>
public struct LockOnOmen
{
    /// <summary>
    /// </summary>
    public const string
        Share4 = "com_share0c", //com_share3t ?
        Share2 = "m0618trg_a0k1",
        Single = "lockon5_t0h";
}

/// <summary>
///  "vfx/channeling/eff/{Name}.avfx".
/// </summary>
public struct ChannelingOmen
{
    /// <summary>
    /// </summary>
    public const string
        ChannelingDark = "chn_dark001f",
        ChannelingLight = "chn_light01f",
        ChannelingWater = "chn_water01f",
        ChannelingWind = "chn_wind001f",
        ChannelingFire = "chn_fire001f",
        ChannelingThunder = "chn_thunder1f",
        ChannelingEarth = "chn_earth001f",
        ChannelingIce = "chn_ice001f";
}

/// <summary>
/// "vfx/omen/eff/{Name}.avfx".
/// </summary>
public struct StaticOmen
{
    /// <summary>
    /// </summary>
    public const string
        Fan = "nd/customFan",
        Circle = "nd/customCircle",
        Donut = "nd/customDount",
        Rect = "nd/rect",
        Rect2 = "nd/rect2";
}
