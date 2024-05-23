using ECommons.DalamudServices;
using RotationSolver.Basic.Configuration.Drawing;
using RotationSolver.Basic.Configuration.TerritoryAction;
using RotationSolver.Basic.Configuration.Timeline;
using RotationSolver.Basic.Configuration.Timeline.TimelineCondition;
using RotationSolver.Basic.Configuration.Trigger;
using XIVConfigUI;

namespace RotationSolver.Basic.Configuration;

internal class OtherConfiguration
{
    public static HashSet<uint> HostileCastingArea = [];
    public static HashSet<uint> HostileCastingTank = [];
    public static HashSet<uint> HostileCastingKnockback = [];

    public static SortedList<uint, float> AnimationLockTime = [];

    public static Dictionary<uint, string[]> NoHostileNames = [];
    public static Dictionary<uint, string[]> NoProvokeNames = [];
    public static Dictionary<uint, Vector3[]> BeneficialPositions = [];

    public static HashSet<uint> DangerousStatus = [];
    public static HashSet<uint> PriorityStatus = [];
    public static HashSet<uint> InvincibleStatus = [];
    public static HashSet<uint> NoCastingStatus = [];

    public static RotationSolverRecord RotationSolverRecord = new();

    #region Territory Config
    private static readonly Dictionary<uint, TerritoryConfig> _territoryConfigs = [];
    private static readonly List<uint> _downloadingList = [];
    private static readonly TerritoryConfig Empty = new ();
    public static TerritoryConfig TerritoryConfig
    {
        get
        {
            if (Svc.ClientState == null)
            {
                if (_territoryConfigs.TryGetValue(0, out var v)) return v;
                else return _territoryConfigs[0] = new();
            }

            var id = Svc.ClientState.TerritoryType;

            if (_territoryConfigs.TryGetValue(id, out var value)) return value;

            LoadConfig(id);
            return Empty;
        }
    }

    public static TerritoryConfig GetTerritoryConfigById(uint id)
    {
        if (_territoryConfigs.TryGetValue(id, out var value)) return value;

        LoadConfig(id);
        return Empty;
    }

    public static void SetTerritoryConfigById(uint id, string text, bool isTimeline)
    {
        var newConfig = FromTxt(text);

        if (!_territoryConfigs.TryGetValue(id, out var config)) config = new();

        if (isTimeline)
        {
            config.JobConfig.Timeline = newConfig.JobConfig.Timeline;
            config.Config.Timeline = newConfig.Config.Timeline;
        }
        else
        {
            config.JobConfig.Trigger = newConfig.JobConfig.Trigger;
            config.Config.Trigger = newConfig.Config.Trigger;
        }
    }

    private static void LoadConfig(uint id)
    {
        var path = GetFilePath("TerritoryConfigs\\" + id);
        if (File.Exists(path))
        {
            try
            {
                var str = File.ReadAllText(path);
                _territoryConfigs[id] = FromTxt(str);
            }
            catch(Exception ex)
            {
                Svc.Log.Warning(ex, "Failed to load the territory config");
                _territoryConfigs[id] = new();
            }
            return;
        }

        if (_downloadingList.Contains(id)) return;
        _downloadingList.Add(id);

        Task.Run(() =>
        {
            DownloadTerritoryPrivate(id);
        });
    }

    private static TerritoryConfig FromTxt(string str)
    {
        return JsonConvert.DeserializeObject<TerritoryConfig>(str,
                    new TerritoryActionConverter(), new BaseDrawingGetterConverter(), 
                    new ITimelineConditionConverter(), new BaseTimelineItemConverter(),
                    new BaseTriggerItemConverter())!;
    }

    private static void DownloadTerritoryPrivate(uint id)
    {
        try
        {
            using var client = new HttpClient();
            var str = client.GetStringAsync($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{XIVConfigUIMain.RepoName}/main/Resources/TerritoryConfigs/{id}.json").Result;

            _territoryConfigs[id] = FromTxt(str);
        }
        catch (Exception ex)
        {
            _territoryConfigs[id] = new();
#if DEBUG
            Svc.Log.Error(ex, $"Failed to download the timeline {id}.");
#endif
            return;
        }
        _downloadingList.Remove(id);
    }
    #endregion

    public static void Init()
    {
        if (!Directory.Exists(Svc.PluginInterface.ConfigDirectory.FullName))
        {
            Directory.CreateDirectory(Svc.PluginInterface.ConfigDirectory.FullName);
        }

        Task.Run(() => InitOne(ref DangerousStatus, nameof(DangerousStatus)));
        Task.Run(() => InitOne(ref PriorityStatus, nameof(PriorityStatus)));
        Task.Run(() => InitOne(ref InvincibleStatus, nameof(InvincibleStatus)));
        Task.Run(() => InitOne(ref NoHostileNames, nameof(NoHostileNames)));
        Task.Run(() => InitOne(ref NoProvokeNames, nameof(NoProvokeNames)));
        Task.Run(() => InitOne(ref AnimationLockTime, nameof(AnimationLockTime)));
        Task.Run(() => InitOne(ref HostileCastingArea, nameof(HostileCastingArea)));
        Task.Run(() => InitOne(ref HostileCastingTank, nameof(HostileCastingTank)));
        Task.Run(() => InitOne(ref BeneficialPositions, nameof(BeneficialPositions)));
        Task.Run(() => InitOne(ref RotationSolverRecord, nameof(RotationSolverRecord), false));
        Task.Run(() => InitOne(ref NoCastingStatus, nameof(NoCastingStatus)));
        Task.Run(() => InitOne(ref HostileCastingKnockback, nameof(HostileCastingKnockback)));
    }

    public static Task Save()
    {
        return Task.Run(async () =>
        {
            await SavePriorityStatus();
            await SaveDangerousStatus();
            await SaveInvincibleStatus();
            await SaveNoHostileNames();
            await SaveAnimationLockTime();
            await SaveHostileCastingArea();
            await SaveHostileCastingTank();
            await SaveBeneficialPositions();
            await SaveRotationSolverRecord();
            await SaveNoProvokeNames();
            await SaveNoCastingStatus();
            await SaveHostileCastingKnockback();
            await SaveTerritoryConfigs();
        });
    }

    public static Task SaveTerritoryConfigs()
    {
        return Task.Run(() =>
        {
            foreach (var pair in _territoryConfigs)
            {
                SavePath(pair.Value, GetFilePath("TerritoryConfigs\\" + pair.Key));
            }
        });
    }

    private static Task SaveHostileCastingKnockback()
    {
        return Task.Run(() => Save(HostileCastingKnockback, nameof(HostileCastingKnockback)));
    }

    public static Task SaveNoCastingStatus()
    {
        return Task.Run(() => Save(NoCastingStatus, nameof(NoCastingStatus)));
    }

    public static Task SavePriorityStatus()
    {
        return Task.Run(() => Save(PriorityStatus, nameof(PriorityStatus)));
    }

    public static Task SaveRotationSolverRecord()
    {
        return Task.Run(() => Save(RotationSolverRecord, nameof(RotationSolverRecord)));
    }
    public static Task SaveNoProvokeNames()
    {
        return Task.Run(() => Save(NoProvokeNames, nameof(NoProvokeNames)));
    }

    public static Task SaveBeneficialPositions()
    {
        return Task.Run(() => Save(BeneficialPositions, nameof(BeneficialPositions)));
    }

    public static Task SaveHostileCastingArea()
    {
        return Task.Run(() => Save(HostileCastingArea, nameof(HostileCastingArea)));
    }

    public static Task SaveHostileCastingTank()
    {
        return Task.Run(() => Save(HostileCastingTank, nameof(HostileCastingTank)));
    }

    public static Task SaveDangerousStatus()
    {
        return Task.Run(() => Save(DangerousStatus, nameof(DangerousStatus)));
    }

    public static Task SaveInvincibleStatus()
    {
        return Task.Run(() => Save(InvincibleStatus, nameof(InvincibleStatus)));
    }

    public static Task SaveNoHostileNames()
    {
        return Task.Run(() => Save(NoHostileNames, nameof(NoHostileNames)));
    }

    public static Task SaveAnimationLockTime()
    {
        return Task.Run(() => Save(AnimationLockTime, nameof(AnimationLockTime)));
    }

    private static string GetFilePath(string name)
    {
        var directory = Svc.PluginInterface.ConfigDirectory.FullName;
#if DEBUG
        var dirInfo = Svc.PluginInterface.AssemblyLocation.Directory;
        dirInfo = dirInfo?.Parent!.Parent!.Parent!.Parent!;
        var dir = dirInfo.FullName + @"\Resources"; 
        if (!Directory.Exists(dir))
        {
            Svc.Log.Error("Failed to save the resources: " + dir);
        }
        else
        {
            directory = dir;
        }
#endif

        var path = directory + $"\\{name}.json";
        var d = new FileInfo(path).Directory;
        if (d != null && !d.Exists)
        {
            d.Create();
        }
        return path;
    }

    private static void Save<T>(T value, string name)
        => SavePath(value, GetFilePath(name));

    private static void SavePath<T>(T value, string path)
    {
        try
        {
            File.WriteAllText(path,
            JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.None,
            }));
        }
        catch (Exception ex)
        {
            Svc.Log.Warning(ex, $"Failed to save the file to {path}");
        }
    }

    private static void InitOne<T>(ref T value, string name, bool download = true)
    {
        var path = GetFilePath(name);
        if (File.Exists(path))
        {
            try
            {
                value = JsonConvert.DeserializeObject<T>(File.ReadAllText(path))!;
            }
            catch (Exception ex)
            {
                Svc.Log.Warning(ex, $"Failed to load {name}.");
            }
        }
        else if (download)
        {
            try
            {
                using var client = new HttpClient();
                var str = client.GetStringAsync($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{XIVConfigUIMain.RepoName}/main/Resources/{name}.json").Result;

                File.WriteAllText(path, str);
                value = JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Error,
                    Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                    {
                        args.ErrorContext.Handled = true;
                    }!
                })!;
            }
            catch
            {
                SavePath(value, path);
            }
        }
        else
        {
            SavePath(value, path);
        }
    }
}