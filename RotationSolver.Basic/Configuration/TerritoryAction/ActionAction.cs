using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.TerritoryAction;

internal class ActionAction : ITerritoryAction
{
    [UI("Action ID")]
    public ActionID ID { get; set; } = ActionID.None;

    [UI("Target Type")]
    public TargetType TargetType { get; set; } = TargetType.None;

    [UI("Can Use Option")]
    public CanUseOption CanUseOption { get; set; } = CanUseOption.None;

    public void Disable()
    {
    }

    public void Enable()
    {
        var act = DataCenter.RightNowRotation?.AllBaseActions.FirstOrDefault(a => (ActionID)a.ID == ID);

        if (act == null) return;

        DataCenter.AddCommandAction(new(act, TargetType, CanUseOption), Service.Config.SpecialDuration);

#if DEBUG
        ECommons.DalamudServices.Svc.Log.Debug($"Added the action {act} to timeline.");
#endif   
    }
}
