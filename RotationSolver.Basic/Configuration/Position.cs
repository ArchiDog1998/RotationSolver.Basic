using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Common.Component.BGCollision;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration;

internal class PositionAttribute() : ListUIAttribute(60403)
{
    public override void OnClick(object obj)
    {
        if (obj is not Position pos) return;
        DrawerHelper.Draw(() => [new OmenData(OmenDataType.Static, StaticOmen.Circle, new(pos), Vector2.One, Vector4.One)]);
    }
}

[Position, Description("Position")]
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
        var pt = point + (Vector3.UnitY * 5);
        var y = -Vector3.UnitY;

        point = FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance()->BGCollisionModule
                ->RaycastMaterialFilter(&hit, &pt, &y, 20, 1, unknown) ? hit.Point : point;

        X = point.X;
        Y = point.Y;
        Z = point.Z;
    }
}
