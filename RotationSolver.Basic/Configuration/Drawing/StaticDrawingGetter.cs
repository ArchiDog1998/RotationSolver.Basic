using ECommons.DalamudServices;
using RotationSolver.Basic.Configuration.Target;
using XIVConfigUI.Attributes;
using XIVDrawer.Vfx;

namespace RotationSolver.Basic.Configuration.Drawing;

[Description("Static Drawing")]
internal class StaticDrawingGetter : BaseDrawingGetter
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

    [UI("On Object")]
    public bool PlaceOnObject { get; set; } = false;

    [UI("Target")]
    public TargetingConditionSet Target { get; set; } = new();

    [UI("Text")]
    public TextDrawing Text { get; set; } = new();
    public override IDisposable[] GetDrawing()
    {
        if (string.IsNullOrEmpty(Path)) return [];

        if (PlaceOnObject)
        {
            List<IDisposable> drawable = [];
            foreach (var obj in Svc.Objects.Where(t => Target.IsTrue(t) ?? false))
            {
                drawable.Add(new StaticVfx(Path, Position + obj.Position, Rotation + obj.Rotation, Scale));
                var text = Text.GetText(Position);
                if(text != null)
                {
                    drawable.Add(text);
                }
            }
            return [.. drawable];
        }
        else
        {
            var item = new StaticVfx(Path, Position, Rotation, Scale);
            var text = Text.GetText(Position);
            return text == null ? [item] : [item, text];
        }
    }
}