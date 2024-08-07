namespace RotationSolver.Basic.Configuration;

/// <summary>
/// Record something about your using of Rotation Solver.
/// </summary>
internal class RotationSolverRecord
{
    private static readonly uint[] SupporterOnlyTerritories = [445, 584];
    private const int DutyCount = 5;

    /// <summary>
    /// How many times have rs clicked for you.
    /// </summary>
    public uint ClickingCount { get; set; } = 0;

    /// <summary>
    /// How many times have you greeted the other users.
    /// </summary>
    public uint SayingHelloCount { get; set; } = 0;

    /// <summary>
    /// The users that already said hello.
    /// </summary>
    public HashSet<string> SaidUsers { get; set; } = [];

    /// <summary>
    /// The played duties.
    /// </summary>
    public Dictionary<uint, List<DateTime>> PlayedDuties { get; set; } = [];

    internal void AddTerritoryId(uint territoryId)
    {
        if (!SupporterOnlyTerritories.Contains(territoryId)) return;

        if (!PlayedDuties.TryGetValue(territoryId, out var territories))
        {
            territories = PlayedDuties[territoryId] = [];
        }

        while(territories.Count > DutyCount)
        {
            territories.RemoveAt(0);
        }

        territories.Add(DateTime.UtcNow);
    }

    internal bool CanPlayInTerritory(uint territoryId)
    {
        if (!SupporterOnlyTerritories.Contains(territoryId)) return true;
        if (!PlayedDuties.TryGetValue(territoryId, out var territories)) return true;
        if (territories.Count == 0) return true;

        while (territories.Count > DutyCount)
        {
            territories.RemoveAt(0);
        }

        return DateTime.UtcNow - territories[0] > TimeSpan.FromDays(1);
    }
}