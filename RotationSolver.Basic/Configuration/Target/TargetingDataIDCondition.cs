using ECommons.DalamudServices;
using System.Text.RegularExpressions;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

internal class DataIdAttribute : ListUIAttribute
{
    public DataIdAttribute() : base(0)
    {
        var desc = typeof(TargetingConditionBase).GetCustomAttribute<ListUIAttribute>()?.Description ?? string.Empty;
        Description = desc + "\nClick to set the target's Data Id";
    }
    public override void OnClick(object obj)
    {
        if (obj is not TargetingDataIDCondition dataIdCondition) return;
        var tar = Svc.Targets.Target;
        if (tar == null) return;
        dataIdCondition.DataID = tar.DataId.ToString("X");
    }
}

[DataId, Description("Data ID Condition")]
internal class TargetingDataIDCondition : TargetingConditionBase
{
    [UI("Data ID")]
    public string DataID { get; set; } = string.Empty;

    protected override bool IsTrueInside(IGameObject obj)
    {
        if (string.IsNullOrEmpty(DataID)) return false;

        return new Regex(DataID).IsMatch(obj.DataId.ToString("X"));
    }
}
