using Dalamud.Utility.Signatures;
using ECommons.DalamudServices;
using ECommons.GameHelpers;
using FFXIVClientStructs.Attributes;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Excel;
using RotationSolver.Basic.Configuration;
using RotationSolver.Basic.Record;
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
            if (DataCenter.TerritoryContentType is TerritoryContentType.DeepDungeons
                || DataCenter.Territory?.PlaceName?.Row is 2775)
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

        try
        {
            Config = JsonConvert.DeserializeObject<Configs>(
                File.ReadAllText(Svc.PluginInterface.ConfigFile.FullName),
                GeneralJsonConverter.Instance)
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
        UpdateYourHash();
    }

    public static bool UpdateYourHash()
    {
        var count = OtherConfiguration.RotationSolverRecord.ClickingCount;
        if (count == 0) return false;

        var upload = count > 2000 && Config.IWannaBeSaidHello;

        try
        {
            GithubRecourcesHelper.UploadYourHash(upload);
        }
        catch
        {

        }
        return upload;
    }

    public static ActionID GetAdjustedActionId(ActionID id)
        => (ActionID)GetAdjustedActionId((uint)id);

    public static unsafe uint GetAdjustedActionId(uint id)
        =>  ActionManager.Instance()->GetAdjustedActionId(id);

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

#if DEBUG
#else
        if (UpdateYourHash())
        {
            var uiName = Config.GetType().GetRuntimeProperty(nameof(Configs.IWannaBeSaidHello))?.LocalUIName() ?? string.Empty;
            var warning = string.Format(UiString.DeleteWarning.Local(), uiName);
            warning.ShowWarning(1);
        }
#endif

        Svc.ClientState.Login -= ClientState_Login;
        Svc.DutyState.DutyCompleted -= DutyState_DutyCompleted;
        Recorder.Dispose();
    }
}
