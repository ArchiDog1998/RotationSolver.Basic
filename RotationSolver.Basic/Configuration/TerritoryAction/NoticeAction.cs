using ECommons.DalamudServices;
using RotationSolver.Helpers;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Configuration.TerritoryAction;
internal class NoticeAction : ITerritoryAction
{
    [UI("Notice")]
    public string Notice { get; set; } = "";

    [UI("Say out")]
    public bool Sayout { get; set; } = true;

    public void Disable()
    {
    }

    public void Enable()
    {
        Svc.Toasts.ShowQuest(Notice);
        if (Sayout)
        {
            SpeechHelper.Speak(Notice);
        }
    }
}
