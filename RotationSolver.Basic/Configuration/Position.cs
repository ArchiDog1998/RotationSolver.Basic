using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Common.Component.BGCollision;
using XIVConfigUI.Attributes;
using XIVDrawer.Vfx;

namespace RotationSolver.Basic.Configuration;

[ListUI(60403)]
[Description("Position")]
internal class Position(float x, float y, float z)
{
    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("X")]
    public float X { get; set; } = x;

    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("Y")]
    public float Y { get; set; } = y;

    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("Z")]
    public float Z { get; set; } = z;

    public Position() : this(0, 0, 0)
    {
        
    }

    public static implicit operator Position(Vector3 value) => new() { X = value.X, Y = value.Y, Z = value.Z };
    public static implicit operator Vector3(Position value) => new() { X = value.X, Y = value.Y, Z = value.Z };
    public static implicit operator Position(FFXIVClientStructs.FFXIV.Common.Math.Vector3 value) => new() { X = value.X, Y = value.Y, Z = value.Z };
    public static implicit operator FFXIVClientStructs.FFXIV.Common.Math.Vector3(Position value) => new() { X = value.X, Y = value.Y, Z = value.Z };

    [UI]
    public unsafe void Pos()
    {
        var tar = Svc.Targets.Target ?? Player.Object;
        if (tar == null) return;

        var point = Player.Object.Position;

        int* unknown = stackalloc int[] { 0x4000, 0, 0x4000, 0 };

        RaycastHit hit = default;

        point = FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance()->BGCollisionModule
                ->RaycastMaterialFilter(&hit, point + (Vector3.UnitY * 5), -Vector3.UnitY, 20, 1, unknown) ? hit.Point : point;

        X = point.X;
        Y = point.Y;
        Z = point.Z;
    }

    [UI]
    public void Draw()
    {
        DrawerHelper.Draw(() => [new StaticVfx(GroundOmenFriendly.BasicCircle.Omen(), this, 0, Vector3.One)]);
    }
}
