using Dalamud.Game.ClientState.Statuses;
using ECommons.Automation;
using ECommons.GameHelpers;
using RotationSolver.Basic.Configuration;
using GStatus = Lumina.Excel.GeneratedSheets.Status;
namespace RotationSolver.Basic.Helpers;

/// <summary>
/// The helper for the status.
/// </summary>
public static class StatusHelper
{
    private static GStatus[]? _badStatus = null;
    internal static GStatus[] BadStatus
        => _badStatus ??= Service.GetSheet<GStatus>()
                    .Where(s => s.StatusCategory == 2 && s.Icon != 0)
                    .ToArray();

    private static GStatus[]? _allDispelStatus = null;
    internal static GStatus[] AllDispelStatus
        => _allDispelStatus ??= Service.GetSheet<GStatus>()
                    .Where(s => s.CanDispel)
                    .ToArray();


    private static GStatus[]? _allStatus = null;
    internal static GStatus[] AllStatus
        => _allStatus ??= Service.GetSheet<GStatus>()
                    .Where(s => !s.CanDispel && !s.LockMovement && !s.IsGaze && !s.IsFcBuff
                        && !string.IsNullOrEmpty(s.Name.ToString()) && s.Icon != 0)
                    .ToArray();

    /// <summary>
    /// 
    /// </summary>
    public static StatusID[] RangePhysicalDefense { get;  } =
    [
        StatusID.Troubadour,
        StatusID.Tactician_1951,
        StatusID.Tactician_2177,
        StatusID.ShieldSamba,
    ];

    /// <summary>
    /// 
    /// </summary>
    public static StatusID[] PhysicResistancec { get; } =
    [
        StatusID.IceSpikes_1720,
    ];

    /// <summary>
    /// 
    /// </summary>
    public static StatusID[] MagicResistance { get; } =
    [
        StatusID.MagicResistance,
        StatusID.RepellingSpray_556,
        StatusID.MagitekField_2166,
    ];


    /// <summary>
    /// 
    /// </summary>
    public static StatusID[] AreaHots { get; } =
    [
        StatusID.AspectedHelios, StatusID.MedicaIi, StatusID.TrueMedicaIi
    ];

    /// <summary>
    /// 
    /// </summary>
    public static StatusID[] SingleHots { get; } =
    [
        StatusID.AspectedBenefic, StatusID.Regen, StatusID.Regen_897, StatusID.Regen_1330
    ];

    internal static StatusID[] TankStanceStatus { get; } =
    [
        StatusID.Grit, StatusID.RoyalGuard_1833, StatusID.IronWill, StatusID.Defiance, 
    ];

    internal static StatusID[] NoNeedHealingStatus { get; } =
    [
        StatusID.Holmgang_409, StatusID.LivingDead, //StatusID.WalkingDead,
        StatusID.Superbolide,
    ];

    internal static StatusID[] SwiftcastStatus { get; } =
    [
        StatusID.Swiftcast,
        StatusID.Triplecast,
        StatusID.Dualcast,
    ];

    internal static StatusID[] AstCardStatus { get; } =
    [
        StatusID.TheArrow_1884,
        StatusID.TheBalance_1882,
        StatusID.TheBole_1883,
        StatusID.TheEwer_1886,
        StatusID.TheSpear_1885,
        StatusID.TheSpire_1887,

        StatusID.Weakness,
        StatusID.BrinkOfDeath,
    ];

    internal static StatusID[] RampartStatus { get; } =
    [
        StatusID.Superbolide,
        StatusID.HallowedGround,
        StatusID.Rampart,
        StatusID.Bulwark,
        StatusID.Bloodwhetting,
        StatusID.Vengeance,
        StatusID.Sentinel,
        StatusID.ShadowWall,
        StatusID.Nebula,
        .. NoNeedHealingStatus,
    ];

    internal static StatusID[] NoPositionalStatus { get; } =
    [
        StatusID.TrueNorth,
        StatusID.RightEye,
    ];

    /// <summary>
    /// Check whether the target needs to be healing.
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public static bool NeedHealing(this IGameObject p) => p.WillStatusEndGCD(2, 0, false, NoNeedHealingStatus);

    /// <summary>
    /// Will any of <paramref name="statusIDs"/> be end after <paramref name="gcdCount"/> gcds add <paramref name="offset"/> seconds?
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="gcdCount"></param>
    /// <param name="offset"></param>
    /// <param name="isFromSelf"></param>
    /// <param name="statusIDs"></param>
    /// <returns></returns>
    public static bool WillStatusEndGCD(this IGameObject obj, uint gcdCount = 0, float offset = 0, bool isFromSelf = true, params StatusID[] statusIDs)
        => WillStatusEnd(obj, DataCenter.GCDTime(gcdCount, offset), isFromSelf, statusIDs);

    /// <summary>
    /// Will any of <paramref name="statusIDs"/> be end after <paramref name="time"/> seconds?
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="time"></param>
    /// <param name="isFromSelf"></param>
    /// <param name="statusIDs"></param>
    /// <returns></returns>
    public static bool WillStatusEnd(this IGameObject obj, float time, bool isFromSelf = true, params StatusID[] statusIDs)
    {
        if (DataCenter.HasApplyStatus(obj.EntityId, statusIDs)) return false;
        var remain = obj.StatusTime(isFromSelf, statusIDs);
        if (remain < 0 && obj.HasStatus(isFromSelf, statusIDs)) return false;
        return remain <= time;
    }

    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="isFromSelf"></param>
    /// <param name="statusIDs"></param>
    /// <returns></returns>
    public static float StatusTime(this IGameObject obj, bool isFromSelf, params StatusID[] statusIDs)
    {
        try
        {
            if (DataCenter.HasApplyStatus(obj.EntityId, statusIDs)) return float.MaxValue;
            var times = obj.StatusTimes(isFromSelf, statusIDs);
            if (times == null || !times.Any()) return 0;
            return Math.Max(0, times.Min() - DataCenter.WeaponRemain);
        }
        catch
        {
            return 0;
        }
    }

    internal static IEnumerable<float> StatusTimes(this IGameObject obj, bool isFromSelf, params StatusID[] statusIDs)
    {
        return obj.GetStatus(isFromSelf, statusIDs).Select(status => status.RemainingTime == 0 ? float.MaxValue : status.RemainingTime);
    }

    /// <summary>
    /// Get the stack of the status.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="isFromSelf"></param>
    /// <param name="statusIDs"></param>
    /// <returns></returns>
    public static byte StatusStack(this IGameObject obj, bool isFromSelf, params StatusID[] statusIDs)
    {
        if (DataCenter.HasApplyStatus(obj.EntityId, statusIDs)) return byte.MaxValue;
        var stacks = obj.StatusStacks(isFromSelf, statusIDs);
        if (stacks == null || !stacks.Any()) return 0;
        return stacks.Min();
    }

    private static IEnumerable<byte> StatusStacks(this IGameObject obj, bool isFromSelf, params StatusID[] statusIDs)
    {
        return obj.GetStatus(isFromSelf, statusIDs).Select(status => status.StackCount == 0 ? byte.MaxValue : status.StackCount);
    }

    /// <summary>
    /// Has one status right now.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="isFromSelf"></param>
    /// <param name="statusIDs"></param>
    /// <returns></returns>
    public static bool HasStatus(this IGameObject obj, bool isFromSelf, params StatusID[] statusIDs)
    {
        if (DataCenter.HasApplyStatus(obj.EntityId, statusIDs)) return true;
        return obj.GetStatus(isFromSelf, statusIDs).Any();
    }

    /// <summary>
    /// Take the status Off.
    /// </summary>
    /// <param name="status"></param>
    public static void StatusOff(StatusID status)
    {
        if (!Player.Object?.HasStatus(false, status) ?? true) return;
        Chat.Instance.SendMessage($"/statusoff {GetStatusName(status)}");
    }

    internal static string GetStatusName(StatusID id)
    {
        return Service.GetSheet<Lumina.Excel.GeneratedSheets.Status>().GetRow((uint)id)!.Name.ToString();
    }

    private static IEnumerable<Status> GetStatus(this IGameObject obj, bool isFromSelf, params StatusID[] statusIDs)
    {
        var newEffects = statusIDs.Select(a => (uint)a);
        return obj.GetAllStatus(isFromSelf).Where(status => newEffects.Contains(status.StatusId));
    }

    private static IEnumerable<Status> GetAllStatus(this IGameObject obj, bool isFromSelf)
    {
        if (obj is not IBattleChara b) return [];

        return b.StatusList.Where(status => !isFromSelf
                                              || status.SourceId == Player.Object.EntityId
                                              || status.SourceObject?.OwnerId == Player.Object.EntityId);
    }

    /// <summary>
    /// Is status Invincible.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public static bool IsInvincible(this Status status)
    {
        if (status.GameData.Icon == 15024) return true;
        return OtherConfiguration.InvincibleStatus.Any(id => (uint)id == status.StatusId);
    }

    /// <summary>
    /// Is the status the priority one.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public static bool IsPriority(this Status status)
    {
        return OtherConfiguration.PriorityStatus.Any(id => (uint)id == status.StatusId);
    }

    /// <summary>
    /// Is status needs to be dispel immediately.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public static bool IsDangerous(this Status status)
    {
        if (!status.CanDispel()) return false;
        if (status.StackCount > 2) return true;
        if (status.RemainingTime > 20) return true;
        return OtherConfiguration.DangerousStatus.Any(id => id == status.StatusId);
    }

    /// <summary>
    /// Can the status be dispel.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public static bool CanDispel(this Status status)
    {
        return status.GameData.CanDispel && status.RemainingTime > 1 + DataCenter.WeaponRemain;
    }
}
