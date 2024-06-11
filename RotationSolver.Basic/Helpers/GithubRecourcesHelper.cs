using ECommons.DalamudServices;
using ECommons.GameHelpers;
using ECommons.ImGuiMethods;
using Octokit;
using RotationSolver.Basic.Configuration;
using XIVConfigUI;

namespace RotationSolver.Basic.Helpers;
internal static class GithubRecourcesHelper
{
    private static readonly GitHubClient GitHubClient = new(new ProductHeaderValue("ophion"))
    {
        Credentials = new("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")
    };

    internal const string RepoName = "RotationSolver.Resources";

    internal delegate bool ModifyValueDelegate<T>(ref T value, out string commit);

    private static async Task ModifyFile<T>(string path, ModifyValueDelegate<T?> modifyFile)
    {
        bool shouldCreate = true;
        string sha = string.Empty;
        T? value = default;
        try
        {
            IReadOnlyList<RepositoryContent> content;

            content = await GitHubClient.Repository.Content.GetAllContents(XIVConfigUIMain.UserName, RepoName, path);

            shouldCreate = content.Count == 0;

            if (!shouldCreate)
            {
                sha = content[0].Sha;
                try
                {
                    value = JsonConvert.DeserializeObject<T>(content[0].Content);
                }
                catch
                {
                    value = default;
                }
            }
        }
        catch (NotFoundException ex)
        {
            Svc.Log.Info(ex, $"Failed to find the file {path}.");
        }
        catch (Exception ex)
        {
            Svc.Log.Error(ex, $"Failed to find the download {path}.");
            return;
        }

        if (!modifyFile(ref value, out var commit))
        {
            return;
        }

        try
        {
            if (shouldCreate)
            {
                await GitHubClient.Repository.Content.CreateFile(XIVConfigUIMain.UserName, RepoName, path, new(commit, JsonConvert.SerializeObject(value, Formatting.Indented)));
            }
            else
            {
                await GitHubClient.Repository.Content.UpdateFile(XIVConfigUIMain.UserName, RepoName, path, new(commit, JsonConvert.SerializeObject(value, Formatting.Indented), sha));
            }
        }
        catch(Exception ex)
        {
            Svc.Log.Error(ex, $"Failed to upload the file to {path}");
        }
    }

    internal static async void UploadYourHash(bool add)
    {
        if (add)
        {
            await ModifyFile<HashSet<string>>("UsersHash.json", AddHash);
        }
        else
        {
            await ModifyFile<HashSet<string>>("UsersHash.json", RemoveHash);
        }

        static bool AddHash(ref HashSet<string>? value, out string commit)
        {
            commit = "Added one Hash.";
            value ??= [];

            var player = Player.Object;
            if (player == null) return false;

            var hash = player.EncryptString();

            if (value.Contains(hash)) return false;

            Notify.Success(string.Format(UiString.AddedYourHash.Local(), 
                typeof(Configs).GetRuntimeProperty(nameof(Configs.IWannaBeSaidHello))?.LocalUIName() ?? nameof(Configs.IWannaBeSaidHello)));
            value.Add(hash);
            return true;
        }

        static bool RemoveHash(ref HashSet<string>? value, out string commit)
        {
            commit = "Removed one Hash.";
            value ??= [];

            var player = Player.Object;
            if (player == null) return false;

            var hash = player.EncryptString();

            if (!value.Contains(hash)) return false;

            Notify.Success(UiString.RemovedYourHash.Local());
            value.Remove(hash);
            return true;
        }
    }

    internal static async void ModifyYourRate(Type rotationType, byte rate)
    {
        await ModifyFile<Dictionary<string, byte>>($"Rating/{rotationType.FullName ?? rotationType.Name}.json", ModifyRate);

        bool ModifyRate(ref Dictionary<string, byte>? value, out string commit)
        {
            commit = $"Modify the rate to {rate}.";
            value ??= [];

            var player = Player.Object;
            if (player == null) return false;

            var hash = player.EncryptString();

            if (value.TryGetValue(hash, out var v) && v == rate) return false;

            value[hash] = rate;
            return true;
        }
    }
}
