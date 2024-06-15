using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Target Condition")]
[ListUI(63922)]
internal interface ITargetingCondition
{
    bool IsTrue(BattleChara chara);
}
