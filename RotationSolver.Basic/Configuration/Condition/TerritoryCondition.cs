using Lumina.Excel.GeneratedSheets;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.Condition;
[Description("Territory Condition")]
internal class TerritoryCondition : DelayConditionBase
{
    internal class DutyNameChoicesAttribute : ChoicesAttribute
    {
        protected override Pair[] GetChoices()
        {
            return [..Service.GetSheet<ContentFinderCondition>()?
                .Select(t => t?.Name?.RawString ?? string.Empty)
                .Where(s => !string.IsNullOrEmpty(s))
                .Reverse()];
        }
    }

    internal class TerritoryNameChoicesAttribute : ChoicesAttribute
    {
        protected override Pair[] GetChoices()
        {
            return [..Service.GetSheet<TerritoryType>()?
                .Select(t => t?.Name?.RawString ?? string.Empty)
                .Where(s => !string.IsNullOrEmpty(s))
                .Reverse()];
        }
    }

    internal enum TerritoryConditionType : byte
    {
        [Description("Territory Content Type")]
        TerritoryContentType,

        [Description("Territory Name")]
        TerritoryName,

        [Description("Duty Name")]
        DutyName,
    }

    [UI("Territory Type")]
    public TerritoryConditionType TerritoryType { get; set; } = TerritoryConditionType.TerritoryContentType;

    [UI("ID", (int)TerritoryConditionType.TerritoryContentType, Parent =nameof(TerritoryType))]
    public TerritoryContentType TerritoryId { get; set; } = 0;

    [DutyNameChoices, UI("Duty Name", (int)TerritoryConditionType.DutyName, Parent = nameof(TerritoryType))]
    public string DutyName { get; set; } = "Not Chosen";

    [TerritoryNameChoices, UI("Territory Name", (int)TerritoryConditionType.TerritoryName, Parent = nameof(TerritoryType))]
    public string TerritoryName { get; set; } = "Not Chosen";

    protected override bool IsTrueInside()
    {
        bool result = false;
        switch (TerritoryType)
        {
            case TerritoryConditionType.TerritoryContentType:
                result = DataCenter.TerritoryContentType == TerritoryId;
                break;

            case TerritoryConditionType.DutyName:
                result = DutyName == DataCenter.ContentFinderName;
                break;

            case TerritoryConditionType.TerritoryName:
                result = TerritoryName == DataCenter.TerritoryName;
                break;
        }
        return result;
    }
}