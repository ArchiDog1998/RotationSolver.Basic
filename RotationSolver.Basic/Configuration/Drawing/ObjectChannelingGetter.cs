using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Drawing;

[Description("Object Channeling Drawing")]

internal class ObjectChannelingGetter : ObjectGetterBase
{
    [ObjectSelector<ChannelingOmen>, UI("Path")]
    public string ChannelingPath { get; set; } = ChannelingOmen.ChannelingDark;

    protected override OmenData[] GetObjectDrawing(IGameObject obj)
    {
        if (string.IsNullOrEmpty(ChannelingPath)) return [];

        var targets = TargetGet(obj);

        if (targets.Length > 0)
        {
            return [.. targets.Select(t => new OmenData(OmenDataType.LockOn, ChannelingPath, new(obj) { Target = t }, Vector2.One, Vector4.One))];
        }
        else
        {
            return [new OmenData(OmenDataType.LockOn, ChannelingPath, new(obj), Vector2.One, Vector4.One)];
        }
    }
}
