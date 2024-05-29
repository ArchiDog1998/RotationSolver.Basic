using Lumina.Excel.GeneratedSheets;
namespace RotationSolver.GameData;
internal static class DiscordGenerator
{
    public static void CreateCode(Lumina.GameData gameData, DirectoryInfo dirInfo)
    {
        var dir = dirInfo.Parent!.Parent!.FullName + "\\RotationSolver.DiscordBot\\RotationSolver.DiscordBot\\Resource.cs";

        var contents = gameData.GetExcelSheet<ContentFinderCondition>()!
            .Where(i => i.Image != 0)
            .Where(i => !string.IsNullOrEmpty(i.Name));

        var dict = new Dictionary<string, ContentFinderCondition>();
        foreach (var item in contents)
        {
            dict[item.Name] = item;
        }

        var items = dict.Select(i => $"        (\"{i.Key}\", {i.Value.Image}, {i.Value.ContentMemberType.Value!.TanksPerParty}, {i.Value.ContentMemberType.Value!.HealersPerParty}, {i.Value.ContentMemberType.Value!.MeleesPerParty + i.Value.ContentMemberType.Value!.RangedPerParty}),");

        var code = $$"""
         namespace RotationSolver.DiscordBot;

         internal static class Resource
         {
             public const string DefaultDuty = "{{contents.Last().Name.RawString}}";

             public static readonly List<(string, uint, byte, byte, byte)> DutyAndImage =
             [
         {{string.Join("\n", items)}}
             ];
         }
         """;

        File.WriteAllText(dir, code);
    }
}
