﻿
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Status Condition")]
internal class TargetingStatusCondition : TargetingConditionBase
{
    [StatusSource(StatusType.AllStatus)]
    [UI("Status")]
    public StatusID Status { get; set; } = StatusID.None;

    [Range(0, 0, ConfigUnitType.Seconds)]
    [UI("Status Time")]
    public float StatusTime { get; set; } = 0;

    protected override bool IsTrueInside(IGameObject obj)
    {
        if (obj is not IBattleChara b) return false;
        var status = b.StatusList.FirstOrDefault(s => s.StatusId == (uint)Status);
        if (status == null) return false;
        if (status.RemainingTime > StatusTime) return false;

        return true;
    }
}
