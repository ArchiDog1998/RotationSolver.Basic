﻿using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Marker Condition")]
internal class TargetingMarkerCondition : TargetingConditionBase
{
    [UI]
    public HeadMarker Marker { get; set; } = HeadMarker.Attack1;

    protected override bool IsTrueInside(IGameObject obj)
    {
        return obj.EntityId == Marker.GetObjectID();
    }
}
