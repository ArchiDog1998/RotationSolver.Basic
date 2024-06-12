namespace RotationSolver.Basic.Helpers;
internal class GeneralHelper
{
    public static T Copy<T>(T obj)
    {
        return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj))!;
    }
}
