﻿using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.Basic.Helpers;

/// <summary>
/// The helper for the action id.
/// </summary>
public static class ActionIdHelper
{
    /// <summary>
    /// Is this action cooling down.
    /// </summary>
    /// <param name="actionID">the action id.</param>
    /// <returns></returns>
    public unsafe static bool IsCoolingDown(this ActionID actionID)
    {
        return actionID.GetAction().GetCoolDownGroup().Any(i => i.IsCoolingDown);
    }

    /// <summary>
    /// Is this action cooling down.
    /// </summary>
    /// <param name="cdGroup"></param>
    /// <returns></returns>
    public unsafe static bool IsCoolingDown(byte cdGroup)
    {
        var detail = GetCoolDownDetail(cdGroup);
        return detail != null && detail->IsActive != 0;
    }

    /// <summary>
    /// The cd details
    /// </summary>
    /// <param name="cdGroup"></param>
    /// <returns></returns>
    public static unsafe RecastDetail* GetCoolDownDetail(byte cdGroup) => ActionManager.Instance()->GetRecastGroupDetail(cdGroup);

    private static Action GetAction(this ActionID actionID)
    {
        return Svc.Data.GetExcelSheet<Action>()!.GetRow((uint)actionID)!;
    }

    /// <summary>
    /// The cast time.
    /// </summary>
    /// <param name="actionID"></param>
    /// <returns></returns>
    public unsafe static float GetCastTime(this ActionID actionID)
    {
        return ActionManager.GetAdjustedCastTime(ActionType.Action, (uint)actionID) / 1000f;
    }

    /// <summary>
    /// The recast time.
    /// </summary>
    /// <param name="actionID"></param>
    /// <returns></returns>
    public unsafe static float GetRecastTime(this ActionID actionID)
    {
        return ActionManager.GetAdjustedRecastTime(ActionType.Action, (uint)actionID) / 1000f;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Obsolete("Please use it as less as possible!")]
    public static ActionID AdjustId(this ActionID id)
        => (ActionID)GetAdjustedActionId((uint)id);

    private static unsafe uint GetAdjustedActionId(uint id)
        => ActionManager.Instance()->GetAdjustedActionId(id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="actionID"></param>
    /// <returns></returns>
    public unsafe static bool IsHighlight(this ActionID actionID)
    {
        return ActionManager.Instance()->IsActionHighlighted(ActionType.Action, (uint)actionID);
    }
}
