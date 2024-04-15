﻿using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using ECommons.DalamudServices;
using FFXIVClientStructs.Attributes;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel;
using RotationSolver.Basic.Configuration;
using RotationSolver.Basic.Configuration.Timeline.TimelineCondition;
using RotationSolver.Basic.Configuration.Timeline.TimelineDrawing;
using RotationSolver.Basic.Configuration.Timeline;

namespace RotationSolver.Basic;

internal class Service : IDisposable
{
    public const string COMMAND = "/rotation";

    // From https://GitHub.com/PunishXIV/Orbwalker/blame/master/Orbwalker/Memory.cs#L85-L87
    [Signature("F3 0F 10 05 ?? ?? ?? ?? 0F 2E C6 0F 8A", ScanType = ScanType.StaticAddress, Fallibility = Fallibility.Infallible)]
    static IntPtr forceDisableMovementPtr = IntPtr.Zero;
    private static unsafe ref int ForceDisableMovement => ref *(int*)(forceDisableMovementPtr + 4);

    private unsafe delegate uint AdjustedActionId(ActionManager* manager, uint actionID);
    private static Hook<AdjustedActionId>? _adjustActionIdHook;

    private unsafe delegate ulong OnCheckIsIconReplaceableDelegate(uint actionID);
    private static Hook<OnCheckIsIconReplaceableDelegate>? _checkerHook;


    static bool _canMove = true;
    internal static unsafe bool CanMove
    {
        get => ForceDisableMovement == 0;
        set
        {
            var realCanMove = value || DataCenter.NoPoslock;
            if (_canMove == realCanMove) return;
            _canMove = realCanMove;

            if (!realCanMove)
            {
                ForceDisableMovement++;
            }
            else if (ForceDisableMovement > 0)
            {
                ForceDisableMovement--;
            }
        }
    }

    public static float CountDownTime => Countdown.TimeRemaining;
    public static Configs Config { get; set; } = null!;

    public static uint NextActionID { get; set; } = 0;

    public Service()
    {
        Svc.Hook.InitializeFromAttributes(this);

        unsafe
        {
            _adjustActionIdHook = Svc.Hook.HookFromSignature<AdjustedActionId>("E8 ?? ?? ?? ?? 8B F8 3B DF", GetAdjustedActionIdDetour);
        }
        _adjustActionIdHook.Enable();

        _checkerHook = Svc.Hook.HookFromSignature<OnCheckIsIconReplaceableDelegate>("E8 ?? ?? ?? ?? 84 C0 74 4C 8B D3", IsAdjustedActionIdDetour);
        _checkerHook.Enable();

        try
        {
            Config = JsonConvert.DeserializeObject<Configs>(
                File.ReadAllText(Svc.PluginInterface.ConfigFile.FullName),
                new BaseTimelineItemConverter(), new BaseDrawingGetterConverter(), new ITimelineConditionConverter())
                ?? new Configs();
        }
        catch (Exception ex)
        {
            Svc.Log.Warning(ex, "Failed to load config");
            Config = new Configs();
        }

        Svc.ClientState.Login += ClientState_Login;
        ClientState_Login();
    }

    private void ClientState_Login()
    {
        GithubRecourcesHelper.UploadYourHash(OtherConfiguration.RotationSolverRecord.ClickingCount > 2000 && Config.IWannaBeSaidHello);
    }

    private static bool IsReplaced(uint actionID)
    {
        if (Config.ReplaceIcon)
        {
            switch (actionID)
            {
                case (uint)ActionID.SleepPvE when Config.ReplaceSleep:
                case (uint)ActionID.FootGrazePvE when Config.ReplaceFootGraze:
                case (uint)ActionID.LegGrazePvE when Config.ReplaceLegGraze:
                case (uint)ActionID.ReposePvE when Config.ReplaceRepose:
                    return true;
            }
        }
        return false;
    }

    private static ulong IsAdjustedActionIdDetour(uint actionID)
    {
        return IsReplaced(actionID) ? 1 : _checkerHook!.Original(actionID);
    }

    private static unsafe uint GetAdjustedActionIdDetour(ActionManager* manager, uint actionID)
    {
        return IsReplaced(actionID) ? NextActionID : _adjustActionIdHook!.Original(manager, actionID);
    }
    public static ActionID GetAdjustedActionId(ActionID id)
        => (ActionID)GetAdjustedActionId((uint)id);

    public static unsafe uint GetAdjustedActionId(uint id)
        => _adjustActionIdHook?.Original(ActionManager.Instance(), id)
        ?? ActionManager.Instance()->GetAdjustedActionId(id);

    public unsafe static IEnumerable<IntPtr> GetAddons<T>() where T : struct
    {
        if (typeof(T).GetCustomAttribute<Addon>() is not Addon on) return [];

        return on.AddonIdentifiers
            .Select(str => Svc.GameGui.GetAddonByName(str, 1))
            .Where(ptr => ptr != IntPtr.Zero);
    }

    public static ExcelSheet<T> GetSheet<T>() where T : ExcelRow => Svc.Data.GetExcelSheet<T>()!;

    public void Dispose()
    {
        if (!_canMove && ForceDisableMovement > 0)
        {
            ForceDisableMovement--;
        }
        _adjustActionIdHook?.Dispose();
        _checkerHook?.Dispose();

        Svc.ClientState.Login -= ClientState_Login;
    }
}
