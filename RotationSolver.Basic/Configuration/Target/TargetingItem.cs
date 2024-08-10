using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

internal class TargetingItemAttribute : ListUIAttribute
{
    public TargetingItemAttribute() : base(61512)
    {
        Description = "Click to show the target among all game objects";
    }

    public override void OnClick(object obj)
    {
        if (obj is not TargetingItem data) return;
        DrawerHelper.Draw(() =>
        {
            var target = data.FindTarget(DataCenter.AllTargets);
            if (target == null) return [];
            return [new OmenData(OmenDataType.Static, StaticOmen.Circle, new(target), Vector2.One, Vector4.One)];
        });
    }
}

[TargetingItem, Description("Targeting Item")]
internal class TargetingItem
{
    [UI("Target Condition")]
    public TargetingConditionSet ConditionSet { get; set; } = new();

    [UI("Targeting Type")]
    public TargetingType TargetingType { get; set; } = TargetingType.Big;

    public IBattleChara? FindTarget(IEnumerable<IBattleChara> characters)
    {
        return TargetingType.FindTarget(characters.Where(t => ConditionSet.IsTrue(t) ?? false));
    }
}
