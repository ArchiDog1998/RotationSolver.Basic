using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.TerritoryAction;

internal class ActionAction : ITerritoryAction
{
    [UI("Action ID")]
    public ActionID ID { get; set; } = ActionID.None;

    [UI("Target Type")]
    public TargetType TargetType { get; set; } = TargetType.None;

    public void Disable()
    {
    }

    public void Enable()
    {
        var act = DataCenter.RightNowRotation?.AllBaseActions.FirstOrDefault(a => (ActionID)a.ID == ID);

        if (act == null) return;

        DataCenter.AddCommandAction(act, Service.Config.SpecialDuration, TargetType);

#if DEBUG
        Svc.Log.Debug($"Added the action {act} to timeline.");
#endif   
    }
}
