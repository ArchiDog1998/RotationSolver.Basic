using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Target;

[Description("Target Data")]
[ListUI(61371)]
internal class TargetingData
{
    [UI("Is In Loop", Description = "Will it loop when you use the command /rotation auto?")]
    public bool IsInLoop { get; set; } = true;

    private string _targetName = string.Empty;

    [UI("Name")]
    public string TargetName
    {
        get => string.IsNullOrEmpty(_targetName) ? TargetingType.ToString() : _targetName;
        set => _targetName = value;
    }

    [UI("Target Items")]
    public List<TargetingItem> TargetItems { get; set; } = [];

    [UI("Targeting Type")]
    public TargetingType TargetingType { get; set; } = TargetingType.Big;

    public BattleChara? FindTarget(IEnumerable<BattleChara> chara)
    {
        foreach (var item in TargetItems)
        {
            var b = item.FindTarget(chara);
            if (b != null) return b;
        }
        return TargetingType.FindTarget(chara);
    }

    public static implicit operator TargetingData(TargetingType targetType) => new() { TargetingType = targetType };
}
