using ECommons.DalamudServices;

namespace RotationSolver.Basic.Helpers;
internal static class ToastWarningHelper
{
    private static DateTime _lastWarningTime = DateTime.MinValue;
    public static void ShowToastWarning(this string message)
    {
        if (Service.Config.HideWarning) return;
        if (DateTime.Now - _lastWarningTime < TimeSpan.FromSeconds(3)) return;
        _lastWarningTime = DateTime.Now;
        Svc.Toasts.ShowError(message);
    }
}
