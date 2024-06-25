using ECommons.DalamudServices;
using RotationSolver.Basic.Record;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

internal class VfxAttribute : ListUIAttribute
{
    public VfxAttribute() : base(0)
    {
        var desc = typeof(TargetingConditionBase).GetCustomAttribute<ListUIAttribute>()?.Description ?? string.Empty;
        Description = desc + "\nClick to get the closest vfx from the target.";
    }
    public override void OnClick(object obj)
    {
        if (obj is not TargetingVfxCondition vfxCondition) return;

        var data = Recorder.Data.Select(i => i.Item2).OfType<VfxNewData>().LastOrDefault(d =>
        {
            var tar = Svc.Targets.Target;
            if (tar == null) return true;
            return tar.EntityId == d.Object.EntityId;
        });
        if (string.IsNullOrEmpty(data.Path)) return;

        vfxCondition.VfxPath = data.Path;
    }
}

[Vfx, Description("Vfx Condition")]
internal class TargetingVfxCondition : TargetingConditionBase
{
    [UI("Vfx Path")]
    public string VfxPath { get; set; } = string.Empty;

    [Range(0, 0, ConfigUnitType.Seconds)]
    [UI("Time duration")]
    public Vector2 TimeDuration { get; set; } = new(0, 2);

    protected override bool IsTrueInside(GameObject obj)
    {
        return Recorder.GetData<VfxNewData>(TimeDuration).Any(effect =>
        {
            if (effect.Object?.EntityId != obj.EntityId) return false;
            if (effect.Path != VfxPath) return false;

            return true;
        });
    }
}
