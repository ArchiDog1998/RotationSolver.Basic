﻿using ECommons.GameHelpers;
using XIVConfigUI.Attributes;
using XIVConfigUI.ConditionConfigs;

namespace RotationSolver.Basic.Configuration.Condition;

[Description("Condition")]
[ListUI(0)]
internal abstract class DelayConditionBase : ICondition
{
    [UI("Delay")]
    public Vector2 Delay { get; set; } = default;

    [UI("Offset")]
    public float DelayOffset { get; set; } = 0;

    RandomDelay _delay = default;
    OffsetDelay _offsetDelay = default;

    [ThreadStatic]
    private static Stack<DelayConditionBase>? _callingStack;

    [JsonIgnore]
    public bool? State => IsTrue();

    public bool? IsTrue()
    {
        var rotation = DataCenter.RightNowRotation;

        if (rotation == null) return false;

        _callingStack ??= new(64);

        if (_callingStack.Contains(this))
        {
            //Do something for recursion!
            return null;
        }

        if (_delay.GetRange == null)
        {
            _delay = new(() => Delay);
        }

        if (_offsetDelay.GetDelay == null)
        {
            _offsetDelay = new(() => DelayOffset);
        }

        _callingStack.Push(this);
        var value = CheckBefore(rotation) && IsTrueInside(rotation);

        var result = _delay.Delay(_offsetDelay.Delay(value));
        _callingStack.Pop();

        return result;
    }

    protected abstract bool IsTrueInside(ICustomRotation rotation);

    public virtual bool CheckBefore(ICustomRotation rotation)
    {
        return Player.Available;
    }

    internal static bool CheckBaseAction(ICustomRotation rotation, ActionID id, ref IBaseAction? action)
    {
        if (id != ActionID.None && (action == null || (ActionID)action.ID != id))
        {
            action = rotation.AllBaseActions.FirstOrDefault(a => (ActionID)a.ID == id);
        }
        if (action == null) return false;
        return true;
    }

    internal static bool CheckMemberInfo<T>(ICustomRotation? rotation, ref string name, ref T? value) where T : MemberInfo
    {
        if (rotation == null) return false;

        if (!string.IsNullOrEmpty(name) && (value == null || value.Name != name))
        {
            var memberName = name;
            if (typeof(T).IsAssignableFrom(typeof(PropertyInfo)))
            {
                value = (T?)GetAllMembers(rotation.GetType(), RuntimeReflectionExtensions.GetRuntimeProperties).FirstOrDefault(m => m.Name == memberName);
            }
            else if (typeof(T).IsAssignableFrom(typeof(MethodInfo)))
            {
                value = (T?)GetAllMembers(rotation.GetType(), RuntimeReflectionExtensions.GetRuntimeMethods).FirstOrDefault(m => m.Name == memberName);
            }
        }
        return true;
    }

    private static IEnumerable<MemberInfo> GetAllMembers(Type? type, Func<Type, IEnumerable<MemberInfo>> getFunc)
    {
        if (type == null || getFunc == null) return [];

        var methods = getFunc(type);
        return methods.Union(GetAllMembers(type.BaseType, getFunc));
    }
}
