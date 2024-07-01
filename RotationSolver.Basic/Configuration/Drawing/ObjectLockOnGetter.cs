using ECommons.DalamudServices;
using RotationSolver.Basic.Configuration.Target;
using XIVConfigUI.Attributes;
using XIVDrawer.Vfx;

namespace RotationSolver.Basic.Configuration.Drawing;

internal class ObjectSelectorAttribute : ChoicesAttribute
{
    protected override Pair[] GetChoices()
    {
        var lockOns = typeof(LockOnOmen).GetRuntimeFields();
        var channelings = typeof(ChannelingOmen).GetRuntimeFields();

        return
        [
            .. lockOns.Select(f => new Pair(((string)f.GetValue(null)!).LockOn(), f.Name)),
            .. channelings.Select(f => new Pair(((string)f.GetValue(null)!).Channeling(), f.Name)),
        ];
    }
}

[Description("Object Actor Drawing")]

internal class ObjectLockOnGetter : ObjectGetterBase
{
    [ObjectSelector, UI("Path")]
    public string Path { get; set; } = LockOnOmen.Share4.LockOn();


    protected override IDisposable[] GetObjectDrawing(IGameObject obj)
    {
        if (string.IsNullOrEmpty(Path)) return [];

        var targets = TargetGet(obj);

        if (targets.Length > 0)
        {
            return [.. targets.Select(t => new ActorVfx(Path, obj, t))];
        }
        else
        {
            return [new ActorVfx(Path, obj, obj)];
        }
    }
}
