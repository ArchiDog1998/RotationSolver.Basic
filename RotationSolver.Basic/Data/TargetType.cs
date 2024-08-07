namespace RotationSolver.Basic.Data;

/// <summary>
/// The type of targeting.
/// </summary>
public enum TargetingType : byte
{
    /// <summary>
    /// Find the target whose hit box is biggest.
    /// </summary>
    [Description("Big")]
    Big,

    /// <summary>
    /// Find the target whose hit box is smallest.
    /// </summary>
    [Description("Small")]
    Small,

    /// <summary>
    /// Find the target whose hp is highest.
    /// </summary>
    [Description("High HP")]
    HighHP,

    /// <summary>
    /// Find the target whose hp is lowest.
    /// </summary>
    [Description("Low HP")]
    LowHP,

    /// <summary>
    /// Find the target whose max hp is highest.
    /// </summary>
    [Description("High Max HP")]
    HighMaxHP,

    /// <summary>
    /// Find the target whose max hp is lowest.
    /// </summary>
    [Description("Low Max HP")]
    LowMaxHP,

    /// <summary>
    /// The closest target.
    /// </summary>
    [Description("Closest")]
    Close,
}

internal static class TargetingTypeExtension
{
    public static IBattleChara? FindTarget(this TargetingType type, IEnumerable<IBattleChara> chara)
    {
        if (!chara.Any()) return null;

        return type switch
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
}
