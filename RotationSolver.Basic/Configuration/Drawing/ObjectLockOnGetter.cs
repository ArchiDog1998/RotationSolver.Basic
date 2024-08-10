using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Drawing;

[Description("Object LockOn Drawing")]
internal class ObjectLockOnGetter : ObjectGetterBase
{
    [ObjectSelector<LockOnOmen>, UI("Path")]
    public string LockOnPath { get; set; } = LockOnOmen.Share4;

    protected override OmenData[] GetObjectDrawing(IGameObject obj)
    {
        if (string.IsNullOrEmpty(LockOnPath)) return [];
        return [new OmenData(OmenDataType.LockOn, LockOnPath, new(obj), Vector2.One, Vector4.One)];
    }
}
