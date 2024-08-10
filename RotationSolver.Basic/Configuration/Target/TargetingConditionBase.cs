using ECommons.DalamudServices;
using XIVConfigUI.Attributes;
using XIVConfigUI.ConditionConfigs;

namespace RotationSolver.Basic.Configuration.Target;

internal class TargetingConditionAttribute : ListUIAttribute
{
    public TargetingConditionAttribute() : base(0)
    {
        Description = "The icon means whether the target meets the condition.";
    }

    public override void OnClick(object obj)
    {
        if (obj is not TargetingConditionBase data) return;
        DrawerHelper.Draw(() =>
        {
            var targets = DataCenter.AllTargets.Where(t => data.IsTrue(t) ?? false);
            if (targets == null) return [];
            return [..targets.Select(targets => new OmenData(OmenDataType.Static, StaticOmen.Circle, new(targets), Vector2.One, Vector4.One))];
        });
    }
}

[TargetingCondition, Description("Target Condition")]
internal abstract class TargetingConditionBase : ICondition
{
    [JsonIgnore]
    public bool? State
    {
        get
        {
            var tar = Svc.Targets.Target;

            if (tar is null) return null;
            return IsTrue(tar);
        }
    }

    [ThreadStatic]
    private static Stack<TargetingConditionBase>? _callingStack;

    public bool? IsTrue(IGameObject obj)
    {
        _callingStack ??= new(64);

        if (_callingStack.Contains(this))
        {
            //Do something for recursion!
            return null;
        }

        _callingStack.Push(this);

        try
        {
           return  IsTrueInside(obj);
        }
        finally
        {
            _callingStack.Pop();
        }
    }

    protected abstract bool IsTrueInside(IGameObject obj);
}
