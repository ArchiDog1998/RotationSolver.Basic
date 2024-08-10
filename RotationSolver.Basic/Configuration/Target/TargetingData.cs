using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

internal class TargetingDataAttribute : ListUIAttribute
{
    public TargetingDataAttribute() : base(61371)
    {
        Description = "Click to show the target among all game objects";
    }

    public override void OnClick(object obj)
    {
        if (obj is not TargetingData data) return;
        DrawerHelper.Draw(() =>
        {
            var target = data.FindTarget(DataCenter.AllTargets);
            if (target == null) return [];
            return [new OmenData(OmenDataType.Static, StaticOmen.Circle, new(target), Vector2.One, Vector4.One)];
        });
    }
}

[TargetingData, Description("Target Data")]
internal class TargetingData
{
    [JsonIgnore]
    public static bool IsAdvanced => Service.Config.AdvancedTargetSystem;

    [UI("Is In Loop", Description = "Will it loop when you use the command /rotation auto?")]
    public bool IsInLoop { get; set; } = true;

    private string _targetName = string.Empty;

    [UI("Name")]
    public string TargetName
    {
        get => string.IsNullOrEmpty(_targetName) ? TargetingType.ToString() : _targetName;
        set => _targetName = value;
    }

    [UI("Target Items", Parent = nameof(IsAdvanced))]
    public List<TargetingItem> TargetItems { get; set; } = [];

    [UI("Targeting Type")]
    public TargetingType TargetingType { get; set; } = TargetingType.Big;

    public IBattleChara? FindTarget(IEnumerable<IBattleChara> characters)
    {
        if (IsAdvanced)
        {
            foreach (var item in TargetItems)
            {
                var b = item.FindTarget(characters);
                if (b != null) return b;
            }
        }

        return TargetingType.FindTarget(characters);
    }

    public static implicit operator TargetingData(TargetingType targetType) => new() { TargetingType = targetType };
}
