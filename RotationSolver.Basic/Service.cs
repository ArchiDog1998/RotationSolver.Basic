using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using ECommons.DalamudServices;
using FFXIVClientStructs.Attributes;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel;
using RotationSolver.Basic.Configuration;
using RotationSolver.Basic.Configuration.Timeline.TimelineCondition;
using RotationSolver.Basic.Configuration.Timeline;
using RotationSolver.Basic.Record;
using RotationSolver.Basic.Configuration.Drawing;
using RotationSolver.Basic.Configuration.TerritoryAction;
using RotationSolver.Basic.Configuration.Trigger;
using ECommons.GameHelpers;
using XIVConfigUI;

namespace RotationSolver.Basic;

internal class Service : IDisposable
{
    public const string COMMAND = "/rotation";

    public static string InvalidUseString
    {
        get
        {
            if (DownloadHelper.IsSupporter) return string.Empty;

            if (DataCenter.IsInHighEndDuty)
            {
                return UiString.CantUseInHighEnd.Local();
            }
            if (DataCenter.TerritoryContentType is TerritoryContentType.DeepDungeons)
            {
                return UiString.CantUseInDeepDungeons.Local();
            }
            else if (DataCenter.TerritoryContentType is TerritoryContentType.Eureka)
            {
                return UiString.CantUseInEureka.Local();
            }
            else if (DataCenter.TerritoryContentType is (TerritoryContentType)29)
            {
                return UiString.CantUseInBozja.Local();
            }
            else if (!OtherConfiguration.RotationSolverRecord.CanPlayInTerritory(Svc.ClientState.TerritoryType))
            {
                return string.Format(UiString.CantFarmThisDuty.Local(), DataCenter.ContentFinderName);
            }

            if (Config.IWannaBeSaidHello) return string.Empty;

            var uiName = Config.GetType().GetRuntimeProperty(nameof(Configs.IWannaBeSaidHello))?.LocalUIName() ?? string.Empty;

            if (DataCenter.IsPvP)
            {
                return string.Format(UiString.CantUseInPvP.Local(), uiName);
            }
            if (Player.Object.Level >= 90)
            {
                return string.Format(UiString.CantUseAtTopLevel.Local(), uiName);
            }

            return string.Empty;
        }
    }

    // From https://github.com/UnknownX7/Cammy/blob/5c92ef3b1b0f8fdfd8cb690cc0825316721642a1/Game.cs#L31
    [Signature("F3 0F 10 05 ?? ?? ?? ?? 0F 2E C6 0F 8A", ScanType = ScanType.StaticAddress, Fallibility = Fallibility.Infallible, Offset = 4)]
    static IntPtr forceDisableMovementPtr = IntPtr.Zero;
    private static unsafe ref int ForceDisableMovement => ref *(int*)forceDisableMovementPtr;

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
                new TerritoryActionConverter(), new BaseDrawingGetterConverter(), 
                new ITimelineConditionConverter(), new BaseTimelineItemConverter(),
                new BaseTriggerItemConverter())
                ?? new Configs();
        }
        catch (Exception ex)
        {
            Svc.Log.Warning(ex, "Failed to load config");
            Config = new Configs();
        }

        Svc.ClientState.Login += ClientState_Login;
        Svc.DutyState.DutyCompleted += DutyState_DutyCompleted;
        ClientState_Login();
        Recorder.Init();
    }

    private void DutyState_DutyCompleted(object? sender, ushort e)
    {
        OtherConfiguration.RotationSolverRecord.AddTerritoryId(Svc.ClientState.TerritoryType);
    }

    private void ClientState_Login()
    {
        var count = OtherConfiguration.RotationSolverRecord.ClickingCount;
        if (count != 0)
        {
            GithubRecourcesHelper.UploadYourHash(count > 2000 && Config.IWannaBeSaidHello);
        }
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
        Svc.DutyState.DutyCompleted -= DutyState_DutyCompleted;
        Recorder.Dispose();
    }
}
