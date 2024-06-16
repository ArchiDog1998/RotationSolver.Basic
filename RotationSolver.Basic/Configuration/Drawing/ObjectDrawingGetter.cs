using XIVConfigUI.Attributes;
using XIVDrawer.Vfx;

namespace RotationSolver.Basic.Configuration.Drawing;

[Description("Object Static Drawing")]
internal class ObjectDrawingGetter : ObjectGetterBase
{
    [OmenSelector, UI("Path")]
    public string Path { get; set; } = GroundOmenHostile.Circle.Omen();

    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("Position")]
    public Position Position { get; set; } = new();

    [Range(0, 0, ConfigUnitType.Degree)]
    [UI("Rotation")]
    public float Rotation { get; set; }

    [UI("Scale")]
    public Position Scale { get; set; } = Vector3.One;

    protected override IDisposable[] GetObjectDrawing(GameObject obj)
    {
        if (string.IsNullOrEmpty(Path)) return [];

        var targets = TargetGet(obj);

        if (targets.Length > 0)
        {
            return [.. targets.Select(t => new StaticVfx(Path, obj, Scale)
                {
                    LocationOffset = Position,
                    RotateAddition = Rotation,
                    Target = t,
                })];
        }
        else
        {
            return [new StaticVfx(Path, obj, Scale)
                {
                    LocationOffset = Position,
                    RotateAddition = Rotation,
                }];
        }
    }
}