using Lumina.Excel.GeneratedSheets;
namespace RotationSolver.GameData;
internal static class DiscordGenerator
{
    public static void CreateCode(Lumina.GameData gameData, DirectoryInfo dirInfo)
    {
        var dir = dirInfo.Parent!.Parent!.FullName + "\\RotationSolver.DiscordBot\\RotationSolver.DiscordBot\\Resource.cs";

        var dict = new Dictionary<string, ContentFinderCondition>();
        foreach (var item in gameData.GetExcelSheet<ContentFinderCondition>()!
            .Where(i => i.Image != 0)
            .Where(i => !string.IsNullOrEmpty(i.Name)))
        {
            dict[item.Name] = item;
        }

        var items = dict.Select(i => $"        (\"{i.Key}\", {i.Value.Image}, {i.Value.Content}),");

        var code = $$"""
         namespace RotationSolver.DiscordBot;

         internal static class Resource
         {
             public static readonly List<(string, uint, ushort)> DutyAndImage =
             [
         {{string.Join("\n", items)}}
             ];
         }
         """;

        File.WriteAllText(dir, code);

    }
}
