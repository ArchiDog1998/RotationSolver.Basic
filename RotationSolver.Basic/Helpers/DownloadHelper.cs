using ECommons.DalamudServices;
using ECommons.GameHelpers;
using XIVConfigUI;

namespace RotationSolver.Basic.Helpers;

internal static class DownloadHelper
{
    public static string[] LinkLibraries { get; private set; } = [];
    public static string[] UsersHash { get; private set; } = [];
    public static string[] Supporters { get; private set; } = [];
    private static string[] SupportersHash { get; set; } = [];
    internal static string[] ContributorsHash { get; private set; } = [];

    public static bool IsSupporter
    {
        get
        {
            if (!Player.Available) return false;

            var hash = Player.Object.EncryptString();

            return SupportersHash.Contains(hash)
                || ContributorsHash.Contains(hash);
        }
    }
    public static IncompatiblePlugin[] IncompatiblePlugins { get; private set; } = [];

    public static async Task DownloadAsync()
    {
        LinkLibraries = await DownloadOneAsync<string[]>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{XIVConfigUIMain.RepoName}/main/Resources/downloadList.json") ?? [];

        IncompatiblePlugins = await DownloadOneAsync<IncompatiblePlugin[]>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{XIVConfigUIMain.RepoName}/main/Resources/IncompatiblePlugins.json") ?? [];

        ContributorsHash = await DownloadOneAsync<string[]>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{XIVConfigUIMain.RepoName}/main/Resources/ContributorsHash.json") ?? [];

        UsersHash = await DownloadOneAsync<string[]>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{GithubRecourcesHelper.RepoName}/main/UsersHash.json") ?? [];

        Supporters = await DownloadOneAsync<string[]>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{XIVConfigUIMain.RepoName}/main/Resources/Supporters.json") ?? [];

        SupportersHash = await DownloadOneAsync<string[]>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{XIVConfigUIMain.RepoName}/main/Resources/SupportersHash.json") ?? [];
    }

    static readonly Dictionary<Type, Dictionary<string, byte>> loadedData = [];
    static bool _isLoading;
    public static Dictionary<string, byte> GetRating(Type rotationType, out string rate)
    {
        var data = GetRating(rotationType);

        rate = "??";

        if (data.Count > 0)
        {
            rate = data.Sum(i => Math.Min(Math.Max((byte)1, i.Value), (byte)10) / (double)data.Count).ToString("F1");
        }

        return data;
    }

    private static Dictionary<string, byte> GetRating(Type rotationType)
    {
        if (loadedData.TryGetValue(rotationType, out var data)) return data;
        if (_isLoading) return [];

        _isLoading = true;
        UpdateRating(rotationType);
        return [];
    }

    public static void ModifyMyRate(Type rotationType, byte rate)
    {
        if (!Player.Available) return;
        if (!loadedData.TryGetValue(rotationType, out var data)) return;

        data[Player.Object.EncryptString()] = rate;
    }

    private static async void UpdateRating(Type rotationType)
    {
        loadedData[rotationType] = await DownloadOneAsync<Dictionary<string, byte>>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{GithubRecourcesHelper.RepoName}/main/Rating/{rotationType.FullName ?? rotationType.Name}.json")
            ?? [];
        _isLoading = false;
    }

    private static async Task<T?> DownloadOneAsync<T>(string url)
    {
        using var client = new HttpClient();
        try
        {
            var str = await client.GetStringAsync(url);
            return JsonHelper.DeserializeObject<T>(str);
        }
        catch (Exception ex)
        {
            Svc.Log.Information(ex, $"Failed to download list: {url}.");
            return default;
        }
    }
}
