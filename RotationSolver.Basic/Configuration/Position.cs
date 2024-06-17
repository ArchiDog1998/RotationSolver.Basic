using ECommons.DalamudServices;
using ECommons.GameHelpers;
using XIVConfigUI.Attributes;
using XIVDrawer.Vfx;

namespace RotationSolver.Basic.Configuration;

[ListUI(60403)]
[Description("Position")]
internal class Position
{
    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("X")]
    public float X { get; set; }

    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("Y")]
    public float Y { get; set; }

    [Range(0, 0, ConfigUnitType.Yalms)]
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
        if (tar == null) return;

        X = tar.Position.X;
        Y = tar.Position.Y;
        Z = tar.Position.Z;
    }

    [UI]
    public void Draw()
    {
        DrawerHelper.Draw(() => [new StaticVfx(GroundOmenFriendly.BasicCircle.Omen(), this, 0, Vector3.One)]);
    }
}
