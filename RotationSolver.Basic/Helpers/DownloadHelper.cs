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

    static Dictionary<string, byte> _data = [];
    static Type? _loadedType;
    static bool _isLoading = false;
    public static Dictionary<string, byte> GetRating(Type rotationType)
    {
        if (_loadedType == rotationType) return _data;
        if (_isLoading) return [];

        _isLoading = true;
        UpdateRating(rotationType);
        return [];
    }

    public static void ModifyMyRate(byte rate)
    {
        if (!Player.Available) return;
        _data[Player.Object.EncryptString()] = rate;
    }

    private static async void UpdateRating(Type rotationType)
    {
        _data = await DownloadOneAsync<Dictionary<string, byte>>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{GithubRecourcesHelper.RepoName}/main/Rating/{rotationType.FullName ?? rotationType.Name}.json")
            ?? [];
        _loadedType = rotationType;
        _isLoading = false;
    }

    private static async Task<T?> DownloadOneAsync<T>(string url)
    {
        using var client = new HttpClient();
        try
        {
            var str = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(str);
        }
        catch (Exception ex)
        {
            Svc.Log.Information(ex, $"Failed to download list: {url}.");
            return default;
        }
    }
}
