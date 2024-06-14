using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration;

[ListUI(60502)]
internal class TargetingData
{
    [UI("Is In Loop")]
    public bool IsInLoop { get; set; } = true;

    private string _targetName = string.Empty;

    [UI("Name")]
    public string TargetName 
    { 
        get => string.IsNullOrEmpty(_targetName) ? TargetingType.ToString() : _targetName;
        set => _targetName = value;
    }

    [UI("Targeting Type")]
    public TargetingType TargetingType { get; set; } = TargetingType.Big;

    public BattleChara? FindTarget(IEnumerable<BattleChara> chara)
    {
        return TargetingType switch
        {
            TargetingType.Close => chara.MinBy(p => p.DistanceToPlayer()),
            TargetingType.Small => chara.MinBy(p => p.HitboxRadius),
            TargetingType.HighHP => chara.MaxBy(p => p.CurrentHp),
            TargetingType.LowHP => chara.MinBy(p => p.CurrentHp),
            TargetingType.HighMaxHP => chara.MaxBy(p => p.MaxHp),
            TargetingType.LowMaxHP => chara.MinBy(p => p.MaxHp),
            _ => chara.MaxBy(p => p.HitboxRadius),
        };
    }

    public static implicit operator TargetingData(TargetingType targetType) => new() { TargetingType = targetType };
}
