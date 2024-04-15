using ECommons.DalamudServices;
using XIVConfigUI;

namespace RotationSolver.Basic.Helpers;

internal static class DownloadHelper
{
    public static string[] LinkLibraries { get; private set; } = [];
    public static string[] UsersHash { get; private set; } = [];
    public static string[] Supporters { get; private set; } = [];
    public static IncompatiblePlugin[] IncompatiblePlugins { get; private set; } = [];

    public static async Task DownloadAsync()
    {
        LinkLibraries = await DownloadOneAsync<string[]>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{XIVConfigUIMain.RepoName}/main/Resources/downloadList.json") ?? [];
        IncompatiblePlugins = await DownloadOneAsync<IncompatiblePlugin[]>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{XIVConfigUIMain.RepoName}/main/Resources/IncompatiblePlugins.json") ?? [];

        DataCenter.ContributorsHash = await DownloadOneAsync<string[]>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{XIVConfigUIMain.RepoName}/main/Resources/ContributorsHash.json") ?? [];

        UsersHash = await DownloadOneAsync<string[]>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{GithubRecourcesHelper.RepoName}/main/UsersHash.json") ?? [];

        Supporters = await DownloadOneAsync<string[]>($"https://raw.githubusercontent.com/{XIVConfigUIMain.UserName}/{XIVConfigUIMain.RepoName}/main/Resources/Supporters.json") ?? [];
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
            Svc.Log.Information(ex, "Failed to download list.");
            return default;
        }
    }
}
