using ECommons.DalamudServices;
using RotationSolver.Basic.Configuration.Target;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Drawing;
internal abstract class ObjectGetterBase : BaseDrawingGetter
{
    [UI("Manual Target")]
    public bool ManualTarget { get; set; } = true;

    [UI("Object")]
    public TargetingConditionSet Object { get; set; } = new();

    [UI("Target", Parent = nameof(ManualTarget))]
    public TargetingConditionSet Target { get; set; } = new();

    [UI("Object Text")]
    public TextDrawing ObjectText { get; set; } = new();

    [UI("Target Text")]
    public TextDrawing TargetText { get; set; } = new();

    public override IDisposable[] GetDrawing()
    {
        var objs = Svc.Objects.Where(t => Object.IsTrue(t) ?? false);
        return [..objs.SelectMany(GetTextDrawing),
            ..objs.SelectMany(GetObjectDrawing)];
    }

    protected IGameObject[] TargetGet(IGameObject obj)
    {
        if (ManualTarget)
        {
            return [.. Svc.Objects.Where(t => Target.IsTrue(t) ?? false)];
        }
        else
        {
            var tar = obj.TargetObject;
            if (tar == null) return [];
            return [tar];
        }
    }
    private IDisposable[] GetTextDrawing(IGameObject obj)
    {
        return [..TargetGet(obj).Select(TargetText.GetText)
            .Append(ObjectText.GetText(obj))
            .OfType<IDisposable>()];
    }

    protected abstract IDisposable[] GetObjectDrawing(IGameObject obj);
}
