using Dalamud.Interface.Colors;
using ECommons.DalamudServices;
using RotationSolver.Basic.Configuration.Target;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Drawing;

[Description("Static Drawing")]
internal class StaticDrawingGetter : BaseDrawingGetter
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

    [UI("On Object")]
    public bool PlaceOnObject { get; set; } = false;

    [UI("Target")]
    public TargetingConditionSet Target { get; set; } = new();

    [UI("Text")]
    public TextDrawing Text { get; set; } = new();
    public override OmenData[] GetDrawing()
    {
        if (string.IsNullOrEmpty(Path)) return [];

        var scale = new Vector2(Size1, Size2);
        if (PlaceOnObject)
        {
            List<OmenData> drawable = [];
            foreach (var obj in Svc.Objects.Where(t => Target.IsTrue(t) ?? false))
            {
                drawable.Add(new OmenData( OmenDataType.Static, Path, new()
                {
                    Position = Position + obj.Position,
                    Rotation = Rotation + obj.Rotation,
                }, scale, Color));
                var text = Text.GetText(Position);
                if(text != null)
                {
                    drawable.Add(new(text));
                }
            }
            return [.. drawable];
        }
        else
        {
            var item = new OmenData(OmenDataType.Static, Path, new()
            {
                Position = Position,
                Rotation = Rotation,
            }, scale, Color);

            var text = Text.GetText(Position);
            return text == null ? [item] : [item, new(text)];
        }
    }
}