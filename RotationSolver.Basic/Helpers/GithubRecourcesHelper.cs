using ECommons.DalamudServices;
using ECommons.GameHelpers;
using Octokit;
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

    internal static async Task ModifyFile<T>(string path, ModifyValueDelegate<T?> modifyFile)
    {
        IReadOnlyList<RepositoryContent> content;

        try
        {
            content = await GitHubClient.Repository.Content.GetAllContents(XIVConfigUIMain.UserName, RepoName, path);
        }
        catch(Exception ex)
        {
            Svc.Log.Error(ex, $"Failed to find the file {path}.");
            return;
        }

        var shouldCreate = content.Count == 0;

        T? value;
        if (shouldCreate)
        {
            value = default;
        }
        else
        {
            try
            {
                value = JsonConvert.DeserializeObject<T>(content[0].Content);
            }
            catch
            {
                value = default;
            }
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
                await GitHubClient.Repository.Content.UpdateFile(XIVConfigUIMain.UserName, RepoName, path, new(commit, JsonConvert.SerializeObject(value, Formatting.Indented), content[0].Sha));
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

            value.Remove(hash);
            return true;
        }
    }

    internal static async void ModifyYourRate(Type rotationType, byte rate)
    {
        await ModifyFile<Dictionary<string, byte>>($"Rating/{rotationType.FullName ?? rotationType.Name}.json", ModifyRate);

        bool ModifyRate(ref Dictionary<string, byte>? value, out string commit)
        {
            commit = $"Modify the rateing to {rate}.";
            value ??= [];

            var player = Player.Object;
            if (player == null) return false;

            var hash = player.EncryptString();
            value[hash] = rate;
            return true;
        }
    }
}
