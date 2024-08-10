using XIVConfigUI.Attributes;
using XIVConfigUI.Overlay;

namespace RotationSolver.Basic.Configuration.Drawing;

internal class TextDrawing
{
    [UI("Text")]
    public string Text { get; set; } = "";

    [UI("Offset")]
    public Position PositionOffset { get; set; } = new();

    [Range(0, 0, ConfigUnitType.Pixels)]
    [UI("Padding"), UIType(UiType.Padding)]
    public Vector2 Padding { get; set; } = Vector2.One * 5;

    [Range(0, 0, ConfigUnitType.Pixels)]
    [UI("Corner")]
    public float Corner { get; set; } = 5;

    [UI("Scale")]
    public float Scale { get; set; } = 1;

    [UI("Bg Color")]
    public Vector4 BackgroundColor { get; set; } = new(0, 0, 0, 0.5f);

    [UI("Color")]
    public Vector4 Color { get; set; } = new(1, 1, 1, 1);

    public Drawing3DText? GetText(Vector3 position)
    {
        if (string.IsNullOrEmpty(Text)) return null;
        return new(Text, position + PositionOffset)
        {
            HideIfInvisible = false,
            Padding = Padding,
            BackgroundColor = ImGui.ColorConvertFloat4ToU32(BackgroundColor),
            Color = ImGui.ColorConvertFloat4ToU32(Color),
            Scale = Scale,
        };
    }

    public Drawing3DText? GetText(IGameObject obj)
    {
        var text = GetText(obj.Position);

        if (text == null) return null;

        text.Text = text.Text.Replace("{Name}", obj.Name.TextValue);
        text.UpdateEveryFrame += () =>
        {
            text.Position = obj.Position + PositionOffset;
        };
        return text;
    }
}
