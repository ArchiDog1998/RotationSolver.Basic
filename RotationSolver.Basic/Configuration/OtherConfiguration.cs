﻿using ECommons.DalamudServices;
using XIVConfigUI;

namespace RotationSolver.Basic.Configuration;

internal class OtherConfiguration
{
    public static HashSet<uint> HostileCastingArea = [];
    public static HashSet<uint> HostileCastingTank = [];
    public static HashSet<uint> HostileCastingKnockback = [];

    public static SortedList<uint, float> AnimationLockTime = [];

    public static HashSet<uint> DangerousStatus = [];
    public static HashSet<uint> PriorityStatus = [];
    public static HashSet<uint> InvincibleStatus = [];
    public static HashSet<uint> NoCastingStatus = [];

    public static RotationSolverRecord RotationSolverRecord = new();

    public static Dictionary<uint, StatusID[]> StatusProvide = [];
    public static Dictionary<uint, StatusID[]> TargetStatusProvide = [];

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
        return JsonHelper.DeserializeObject<TerritoryConfig>(str)!;
    }

    private static void DownloadTerritoryPrivate(uint id)
    {
        try
        {
            using var client = new HttpClient();
            var str = client.GetStringAsync($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{XIVConfigUIMain.RepoName}/main/Resources/TerritoryConfigs/{id}.json").Result;

            _territoryConfigs[id] = FromTxt(str);
        }
#if DEBUG
        catch (Exception ex)
        {
            Svc.Log.Error(ex, $"Failed to download the timeline {id}.");
#else
        catch
        { 
#endif
            _territoryConfigs[id] = new();
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

        Task.Run(() => InitOne(ref StatusProvide, nameof(StatusProvide)));
        Task.Run(() => InitOne(ref TargetStatusProvide, nameof(TargetStatusProvide)));
        Task.Run(() => InitOne(ref DangerousStatus, nameof(DangerousStatus)));
        Task.Run(() => InitOne(ref PriorityStatus, nameof(PriorityStatus)));
        Task.Run(() => InitOne(ref InvincibleStatus, nameof(InvincibleStatus)));
        Task.Run(() => InitOne(ref AnimationLockTime, nameof(AnimationLockTime)));
        Task.Run(() => InitOne(ref HostileCastingArea, nameof(HostileCastingArea)));
        Task.Run(() => InitOne(ref HostileCastingTank, nameof(HostileCastingTank)));
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
            await SaveAnimationLockTime();
            await SaveHostileCastingArea();
            await SaveHostileCastingTank();
            await SaveRotationSolverRecord();
            await SaveNoCastingStatus();
            await SaveHostileCastingKnockback();
            await SaveTerritoryConfigs();
            await SaveStatusProvide();
            await SaveTargetStatusProvide();
        });
    }

    private static Task SaveStatusProvide()
    {
        return Task.Run(() => Save(StatusProvide, nameof(StatusProvide)));
    }

    private static Task SaveTargetStatusProvide()
    {
        return Task.Run(() => Save(TargetStatusProvide, nameof(TargetStatusProvide)));
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
        return Task.Run(() => Save(RotationSolverRecord, nameof(RotationSolverRecord), false));
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

    private static void Save<T>(T value, string name, bool download = true)
        => SavePath(value, GetFilePath(name), download);

    private static void SavePath<T>(T value, string path, bool download = true)
    {
        try
        {
            var str = JsonHelper.SerializeObject(value);

            if (!download)
            {
                str = Cryptor.Crypt(str);
            }

            File.WriteAllText(path, str);
        }
        catch (Exception ex)
        {
            Svc.Log.Warning(ex, $"Failed to save the file to {path}");
        }
    }

    private static void InitOne<T>(ref T value, string name, bool download = true) where T : new()
    {
        var path = GetFilePath(name);
        if (File.Exists(path))
        {
            try
            {
                var str = File.ReadAllText(path);

                if (download)
                {
                    value = JsonHelper.DeserializeObject<T>(str) ?? new();
                }
                else
                {
                    str = Cryptor.Decrypt(str);
                    value = JsonHelper.DeserializeObject<T>(str) ?? new();
                }
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
                value = JsonHelper.DeserializeObject<T>(str) ?? new();
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