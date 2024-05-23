using Lumina.Excel.GeneratedSheets;
namespace RotationSolver.GameData;
internal static class DiscordGenerator
{
    public static void CreateCode(Lumina.GameData gameData, DirectoryInfo dirInfo)
    {
        var dir = dirInfo.Parent!.Parent!.FullName + "\\RotationSolver.DiscordBot\\RotationSolver.DiscordBot\\Resource.cs";

        var dict = new Dictionary<string, uint>();
        foreach (var item in gameData.GetExcelSheet<ContentFinderCondition>()!
            .Where(i => !string.IsNullOrEmpty(i.Name)))
        {
            dict[item.Name] = item.Image;
        }

        var items = dict.Select(i => $"        (\"{i.Key}\", {i.Value}),");

        var code = $$"""
         namespace RotationSolver.DiscordBot;

         internal static class Resource
         {
             public static readonly List<(string, uint)> DutyAndImage =
             [
         {{string.Join("\n", items)}}
             ];
         }
         """;

        File.WriteAllText(dir, code);

    }
}
