using ECommons.DalamudServices;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.TerritoryAction;

internal class MoveAction : ITerritoryAction
{
    [UI("Points")]
    public List<Position> Points { get; set; } = [];

    public void Enable()
    {
        if (!Service.Config.EnableMovement) return;

        var ipc = Svc.PluginInterface.GetIpcSubscriber<List<Vector3>, bool, object>("vnavmesh.Path.MoveTo");

        if (ipc == null)
        {
            Svc.Log.Error("Can't find the vnavmesh to move.");
            return;
        }
        ipc.InvokeAction([..Points], false);
    }

    public void Disable()
    {
        if (!Service.Config.EnableMovement) return;

        var ipc = Svc.PluginInterface.GetIpcSubscriber<object>("vnavmesh.Path.Stop");
        ipc?.InvokeAction();
    }
}
