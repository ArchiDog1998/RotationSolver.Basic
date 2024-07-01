using FFXIVClientStructs.FFXIV.Client.Game.UI;

namespace RotationSolver.Basic.Helpers;

internal enum HeadMarker : byte
{
    Attack1,
    Attack2,
    Attack3,
    Attack4,
    Attack5,
    Bind1,
    Bind2,
    Bind3,
    Stop1,
    Stop2,
    Square,
    Circle,
    Cross,
    Triangle,
    Attack6,
    Attack7, 
    Attack8,
}

internal static class MarkingHelper
{
    internal unsafe static long GetObjectID(this HeadMarker index) => MarkingController.Instance()->Markers[(int)index].ObjectId;

    internal static bool HaveAttackChara => AttackSignTargets.Any(id => id != 0xE000_0000);

    internal static long[] AttackSignTargets => 
    [
        GetObjectID(HeadMarker.Attack1),
        GetObjectID(HeadMarker.Attack2),
        GetObjectID(HeadMarker.Attack3),
        GetObjectID(HeadMarker.Attack4),
        GetObjectID(HeadMarker.Attack5),
        GetObjectID(HeadMarker.Attack6),
        GetObjectID(HeadMarker.Attack7),
        GetObjectID(HeadMarker.Attack8),
    ];

    internal static long[] StopTargets =>
    [
        GetObjectID(HeadMarker.Stop1),
        GetObjectID(HeadMarker.Stop2),
    ];

    internal unsafe static IEnumerable<IBattleChara> FilterStopCharaes(IEnumerable<IBattleChara> charas)
    {
        var ids = StopTargets.Where(id => id != 0xE000_0000);
        return charas.Where(b => !ids.Contains(b.EntityId));
    }
}
