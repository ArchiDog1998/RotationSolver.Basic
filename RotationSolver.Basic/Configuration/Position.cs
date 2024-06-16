using ECommons.DalamudServices;
using ECommons.GameHelpers;
using XIVConfigUI.Attributes;
using XIVDrawer.Vfx;

namespace RotationSolver.Basic.Configuration;

[ListUI(10)]
[Description("Position")]
internal class Position
{
    private static IDisposable[]? _previewItems = null;

    [UI("X")]
    public float X { get; set; }

    [UI("Y")]
    public float Y { get; set; }

    [UI("Z")]
    public float Z { get; set; }

    public static implicit operator Position(Vector3 value) => new() { X = value.X, Y = value.Y, Z = value.Z };
    public static implicit operator Vector3(Position value) => new() { X = value.X, Y = value.Y, Z = value.Z };
    public static implicit operator Position(FFXIVClientStructs.FFXIV.Common.Math.Vector3 value) => new() { X = value.X, Y = value.Y, Z = value.Z };
    public static implicit operator FFXIVClientStructs.FFXIV.Common.Math.Vector3(Position value) => new() { X = value.X, Y = value.Y, Z = value.Z };

    [UI]
    public void Pos()
    {
        var tar = Svc.Targets.Target ?? Player.Object;
        if (tar != null)
        {
            X = tar.Position.X;
            Y = tar.Position.Y; 
            Z = tar.Position.Z;
        }
    }

    [UI]
    public void Draw()
    {
        if (_previewItems == null)
        {
            _previewItems = [new StaticVfx(GroundOmenFriendly.BasicCircle.Omen(), this, 0, Vector3.One)];
        }
        else
        {
            ClearDrawings();
        }
    }

    public static void ClearDrawings()
    {
        if (_previewItems == null) return;

        foreach (var preview in _previewItems)
        {
            preview.Dispose();
        }
        _previewItems = null;
    }
}
