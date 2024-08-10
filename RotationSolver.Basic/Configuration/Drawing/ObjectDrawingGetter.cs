using Dalamud.Interface.Colors;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Drawing;

internal class ObjectSelectorAttribute<T> : ChoicesAttribute where T : struct
{
    protected override Pair[] GetChoices()
    {
        var fields = typeof(T).GetRuntimeFields();

        return
        [
            .. fields.Select(f => new Pair(((string)f.GetValue(null)!), f.Name)),
        ];
    }
}

[Description("Object Static Drawing")]
internal class ObjectDrawingGetter : ObjectGetterBase
{
    [ObjectSelector<StaticOmen>, UI("Path")]
    public string Path { get; set; } = StaticOmen.Circle;

    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("Position")]
    public Position Position { get; set; } = new();

    [Range(0, 0, ConfigUnitType.Degree)]
    [UI("Rotation")]
    public float Rotation { get; set; }

    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("Size 1")]
    public float Size1 { get; set; } = 1;

    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("Size 2")]
    public float Size2 { get; set; } = 1;

    [UI("Color")]
    public Vector4 Color { get; set; } = ImGuiColors.DalamudRed;

    protected override OmenData[] GetObjectDrawing(IGameObject obj)
    {
        if (string.IsNullOrEmpty(Path)) return [];

        var targets = TargetGet(obj);

        if (targets.Length > 0)
        {
            return [.. targets.Select(t => new OmenData(OmenDataType.Static, Path, new(obj)
            {
                Position = Position,
                Rotation = Rotation,
                Target = t,
            }, new(Size1, Size2), Color))];
        }
        else
        {
            return [new OmenData(OmenDataType.Static, Path, new(obj)
            {
                Position = Position,
                Rotation = Rotation,
            }, new(Size1, Size2), Color)];
        }
    }
}