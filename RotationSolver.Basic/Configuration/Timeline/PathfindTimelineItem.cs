using ECommons.DalamudServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotationSolver.Basic.Configuration.Timeline;

[Description("Path find Time line")]
internal class PathfindTimelineItem : BaseTimelineItem
{
    public Vector3 Destination { get; set; } = default;

    public override bool InPeriod(TimelineItem item)
    {
        var time = item.Time - DataCenter.RaidTimeRaw;

        if (time < 0) return false;

        if (time > Time || Time - time > 3) return false;

        if (!Condition.IsTrue(item)) return false;

        return true;
    }

    internal override void OnEnable()
    {
        base.OnEnable();

        if (!Service.Config.EnableTimelineMovement) return;

        var ipc = Svc.PluginInterface.GetIpcSubscriber<Vector3, bool, object>("vnavmesh.SimpleMove.PathfindAndMoveTo");

        if (ipc == null)
        {
            Svc.Log.Error("Can't find the vnavmesh to path finding.");
            return;
        }
        ipc.InvokeAction(Destination, false);
    }

    internal override void OnDisable()
    {
        base.OnDisable();

        if (!Service.Config.EnableTimelineMovement) return;

        var ipc = Svc.PluginInterface.GetIpcSubscriber<object>("vnavmesh.Path.Stop");
        ipc?.InvokeAction();
    }
}
