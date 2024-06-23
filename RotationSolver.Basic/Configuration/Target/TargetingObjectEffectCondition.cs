
using ECommons.DalamudServices;
using RotationSolver.Basic.Record;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

internal class ObjEffectAttribute : ListUIAttribute
{
    public ObjEffectAttribute() : base(0)
    {
        var desc = typeof(TargetingConditionBase).GetCustomAttribute<ListUIAttribute>()?.Description ?? string.Empty;
        Description = desc + "\nClick to get the closest object effect from the target.";
    }
    public override void OnClick(object obj)
    {
        if (obj is not TargetingObjectEffectCondition objEffectCondition) return;

        var data = Recorder.Data.Select(i => i.Item2).OfType<ObjectEffectData>().LastOrDefault(d =>
        {
            var tar = Svc.Targets.Target;
            if (tar == null) return true;
            return tar.ObjectId == d.Object.ObjectId;
        });
        if (data.Object == null) return;

        objEffectCondition.Param1 = data.Param1;
        objEffectCondition.Param2 = data.Param2;
    }
}

[ObjEffect, Description("Object Effect Condition")]
internal class TargetingObjectEffectCondition : TargetingConditionBase
{
    [UI("Param1")]
    public int Param1 { get; set; }

    [UI("Param2")]
    public int Param2 { get; set; }

    [Range(0, 0, ConfigUnitType.Seconds)]
    [UI("Time duration")]
    public Vector2 TimeDuration { get; set; } = new(0, 2);

    protected override bool IsTrueInside(GameObject obj)
    {
        return Recorder.GetData<ObjectEffectData>(TimeDuration).Any(effect =>
        {
            if (effect.Object?.ObjectId != obj.ObjectId) return false;

            if (effect.Param1 != Param1) return false;
            if (effect.Param2 != Param2) return false;

            return true;
        });
    }
}
