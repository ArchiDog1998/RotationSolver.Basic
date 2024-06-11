using Dalamud.Interface.Utility;
using XIVConfigUI;
using XIVConfigUI.SearchableConfigs;

namespace RotationSolver.Basic.Rotations;

internal class RotationSearchableConfig : SearchableConfig //TODO : better info.
{
    public override bool GeneralDefault => false;

    public override bool IsPropertyValid(PropertyInfo property)
    {
        var attr = property.GetCustomAttribute<RotationConfigAttribute>()?.Type ?? CombatType.Both;

        switch (attr)
        {
            case CombatType.PvP when !DataCenter.IsPvP:
            case CombatType.PvE when DataCenter.IsPvP:
                return false;
        }
        return true;
    }

    public override void PreNameDrawing(PropertyInfo property)
    {
        var attr = property.GetCustomAttribute<RotationConfigAttribute>()?.Type ?? CombatType.Both;

        if (ImageLoader.GetTexture(attr.GetIcon(), out var texture))
        {
            ImGui.SameLine();
            ImGui.Image(texture.ImGuiHandle, Vector2.One * 20 * ImGuiHelpers.GlobalScale);

            //TODO, tooltip drawing...
        }
    }

    public override void PropertyInvalidTooltip(PropertyInfo property)
    {
        //TODO, tooltip drawing...
    }

    public override void AfterConfigChange(Searchable item)
    {
        if (item._property.GetValue(item._obj)?.ToString() is string s)
        {
            Service.Config.RotationConfigurations[item._property.Name] = s;
        }
        base.AfterConfigChange(item);
    }
}

