using ECommons.DalamudServices;
using ECommons.GameFunctions;
using RotationSolver.Basic.Configuration.Target;
using XIVConfigUI.Attributes;
using XIVDrawer;
using XIVDrawer.Vfx;
using GAction = Lumina.Excel.GeneratedSheets.Action;

namespace RotationSolver.Basic.Configuration.Drawing;

internal class OmenSelectorAttribute : ChoicesAttribute
{
    protected override Pair[] GetChoices()
    {
        var omenInfo = typeof(GroundOmenHostile).GetRuntimeFields()
        .Concat(typeof(GroundOmenNone).GetRuntimeFields())
        .Concat(typeof(GroundOmenFriendly).GetRuntimeFields());

        return 
        [
            .. omenInfo.Select(f => new Pair(((string)f.GetValue(null)!).Omen(), f.Name)),
        ];
    }
}

[Description("Action Drawing")]
internal class ActionDrawingGetter : BaseDrawingGetter
{
    [JsonIgnore]
    public TimelineItem? TimelineItem { get; set; }

    [UI("Action")]
    public ActionID ActionID { get; set; }

    [OmenSelector, UI("Path")]
    public string Path { get; set; } = "";

    [UI("X Scale")]
    public float X { get; set; }

    [UI("Y Scale")]
    public float Y { get; set; }

    [Range(0, 0, ConfigUnitType.Yalms)]
    [UI("Position Or Offset")]
    public Position Position { get; set; } = new();

    [Range(0,0, ConfigUnitType.Degree)]
    [UI("Rotation")]
    public float Rotation { get; set; }

    [UI("Target")]
    public TargetingConditionSet Target { get; set; } = new();

    public override IDisposable[] GetDrawing()
    {
        var objs = Svc.Objects.Where(t => Target.IsTrue(t) ?? false);
        if (objs.Any())
        {
            return [.. objs.Select(GetActionDrawing).OfType<IDisposable>()];
        }

        var item = GetActionDrawing(null);
        if (item == null) return [];
        return [item];
    }

    private IDisposable? GetActionDrawing(GameObject? obj)
    {
        if (ActionID == 0) return null;
        var action = Svc.Data.GetExcelSheet<GAction>()?.GetRow((uint)ActionID);
        if (action == null) return null;
        var omen = action.Omen.Value?.Path?.RawString;
        omen = string.IsNullOrEmpty(omen) ? Path : omen.Omen();

        var x = X != 0 ? X : (action.XAxisModifier > 0 ? action.XAxisModifier / 2 : action.EffectRange);
        var y = Y != 0 ? Y : action.EffectRange;
        var scale = new Vector3(x, XIVDrawerMain.HeightScale, y);

        if (action.TargetArea)
        {
            var location = Position;
            if (obj is BattleChara battle)
            {
                unsafe
                {
                    var info = battle.Struct()->GetCastInfo;
                    if (info->IsCasting != 0)
                    {
                        location = info->CastLocation;
                    }
                }
            }
            return new StaticVfx(omen, location, 0, scale);
        }
        else
        {
            if(obj != null)
            {
                return new StaticVfx(omen, obj, scale)
                {
                    RotateAddition = Rotation / 180 * MathF.PI,
                    LocationOffset = Position,
                };
            }
            else
            {
                return new StaticVfx(omen, Position, Rotation, scale);
            }
        }
    }
}
