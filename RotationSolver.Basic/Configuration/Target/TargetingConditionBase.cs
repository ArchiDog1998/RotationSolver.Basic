using ECommons.DalamudServices;
using XIVConfigUI.Attributes;
using XIVConfigUI.ConditionConfigs;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Target Condition")]
[ListUI(0, Description = "The icon means whether the target meets the condition.")]
internal abstract class TargetingConditionBase : ICondition
{
    public bool? State
    {
        get
        {
            var tar = Svc.Targets.Target;

            if (tar is null) return null;
            return IsTrue(tar);
        }
    }

    public abstract bool IsTrue(GameObject obj);
}
