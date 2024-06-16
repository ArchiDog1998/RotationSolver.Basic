using ECommons.DalamudServices;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.TerritoryAction;
internal class PathfindAction : ITerritoryAction
{
    [UI("Destination")]
    public Position Destination { get; set; } = new();

    public void Disable()
    {
        if (!Service.Config.EnableMovement) return;

        var ipc = Svc.PluginInterface.GetIpcSubscriber<object>("vnavmesh.Path.Stop");
        ipc?.InvokeAction();
    }

    public void Enable()
    {
        if (!Service.Config.EnableMovement) return;

        var ipc = Svc.PluginInterface.GetIpcSubscriber<Vector3, bool, object>("vnavmesh.SimpleMove.PathfindAndMoveTo");

        if (ipc == null)
        {
            Svc.Log.Error("Can't find the vnavmesh to path finding.");
            return;
        }
        ipc.InvokeAction(Destination, false);
    }
}
