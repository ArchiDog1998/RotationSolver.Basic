using Dalamud.Interface.Colors;

namespace RotationSolver.Basic.Configuration;
internal class ActionGroup
{
    [JsonIgnore]
    public static IBaseAction[] AllBaseActions => [.. AllActions.OfType<IBaseAction>()];

    [JsonIgnore]
    public static IAction[] AllActions => 
        [
            ..DataCenter.RightNowRotation?.AllActions ?? [],
            ..DataCenter.RightNowDutyRotation?.AllActions ?? [],
        ];

    internal bool IsValid => ShowInWindow && !string.IsNullOrEmpty(Name);

    public string Name { get; set; } = string.Empty;

    public Vector4 Color { get; private set; } = ImGuiColors.DalamudWhite;

    public List<uint> ActionIds { get; set; } = [];

    public bool ShowInWindow { get; set; } = true;

    private bool _enable = true;
    public bool Enable 
    {
        get => _enable;
        set
        {
            if (_enable == value) return;
            _enable = value;

            foreach (var act in AllBaseActions)
            {
                if (!ActionIds.Contains(act.ID)) continue;
                act.IsEnabled = _enable;
            }
        }
    }
}
