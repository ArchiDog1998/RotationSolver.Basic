using Dalamud.Interface.Colors;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using RotationSolver.Basic.Configuration.Target;
using XIVConfigUI.Attributes;
using GAction = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.Basic.Configuration.Drawing;

[Description("Action Drawing")]
internal class ActionDrawingGetter : BaseDrawingGetter
{
    [JsonIgnore]
    public TimelineItem? TimelineItem { get; set; }

    [UI("Action")]
    public ActionID ActionID { get; set; }

    [ObjectSelector<StaticOmen>, UI("Path")]
    public string Path { get; set; } = "";

    [UI("X Scale")]
    public float X { get; set; }

    [UI("Y Scale")]
    public float Y { get; set; }

    [UI("Color")]
    public Vector4 Color { get; set; } = ImGuiColors.DalamudRed;

    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("Position Or Offset")]
    public Position Position { get; set; } = new();

    [Range(0,0, ConfigUnitType.Degree)]
    [UI("Rotation")]
    public float Rotation { get; set; }

    [UI("Target")]
    public TargetingConditionSet Target { get; set; } = new();

    public override OmenData[] GetDrawing()
    {
        var objs = Svc.Objects.Where(t => Target.IsTrue(t) ?? false);
        if (objs.Any())
        {
            return [.. objs.Select(GetActionDrawing).OfType<OmenData>()];
        }

        var item = GetActionDrawing(null);
        if (item == null) return [];
        return [item.Value];
    }

    private OmenData? GetActionDrawing(IGameObject? obj)
    {
        if (ActionID == 0) return null;
        var action = Svc.Data.GetExcelSheet<GAction>()?.GetRow((uint)ActionID);
        if (action == null) return null;
        var omen = action.Omen.Value?.Path?.RawString;
        omen = string.IsNullOrEmpty(omen) ? Path : omen;

        var x = X != 0 ? X : (action.XAxisModifier > 0 ? action.XAxisModifier / 2 : action.EffectRange);
        var y = Y != 0 ? Y : action.EffectRange;
        var scale = new Vector2(x, y);

        if (action.TargetArea)
        {
            var location = Position;
            if (obj is IBattleChara battle)
            {
                unsafe
                {
                    var info = battle.Struct()->GetCastInfo();
                    if (info->IsCasting != 0)
                    {
                        location = info->TargetLocation;
                    }
                }
            }
            return new OmenData(OmenDataType.Static, omen, new(location), scale, Color);
        }
        else
        {
            if (obj != null)
            {
                return new OmenData(OmenDataType.Static, omen, new(obj)
                {
                    Rotation = Rotation / 180 * MathF.PI,
                    Position = Position,
                }, scale, Color);
            }
            else
            {
                return new OmenData(OmenDataType.Static, omen, new()
                {
                    Position = Position,
                    Rotation = Rotation,
                }, scale, Color);
            }
        } 
    }
}
