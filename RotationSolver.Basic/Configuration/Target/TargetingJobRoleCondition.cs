
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Job Role Condition")]
internal class TargetingJobRoleCondition : TargetingConditionBase
{
    [UI("Job Role")]
    public JobRole JobRole { get; set; }

    protected override bool IsTrueInside(IGameObject obj)
    {
        return obj.IsJobCategory(JobRole);
    }
}
