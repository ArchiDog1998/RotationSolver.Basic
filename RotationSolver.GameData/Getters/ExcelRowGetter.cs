using Lumina.Excel;
using Microsoft.CodeAnalysis;

namespace RotationSolver.GameData.Getters;

internal abstract class ExcelRowGetter<T, TSyntax>(Lumina.GameData gameData) 
    where T : ExcelRow
    where TSyntax : SyntaxNode
{
    public List<string> AddedNames { get; } = [];

    protected Lumina.GameData _gameData = gameData;
    public Dictionary<T, string> Items { get; } = [];
    public int Count => Items.Count;

    protected abstract bool AddToList(T item);
    protected abstract TSyntax[] ToNodes(T item, string name);

    protected abstract string ToName(T item);

    public TSyntax[] GetNodes()
    {
        var items = _gameData.GetExcelSheet<T>();

        if (items == null) return [];
        AddedNames.Clear();

        var filteredItems = items.Where(AddToList);

        return [..filteredItems.SelectMany(item => 
        {
            var name = ToName(item).ToPascalCase();
            if (AddedNames.Contains(name))
            {
                name += "_" + item.RowId.ToString();
            }
            else
            {
                AddedNames.Add(name);
            }
            Items[item] = name;
            return ToNodes(item, name);
        })];
    }
}
