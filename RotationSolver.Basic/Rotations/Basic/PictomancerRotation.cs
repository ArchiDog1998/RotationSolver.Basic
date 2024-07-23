using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
partial class PictomancerRotation
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Intelligence;

    #region Basic Combo
    static partial void ModifyFireInRedPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Aetherhues];
    }

    static partial void ModifyAeroInGreenPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Aetherhues];
        setting.StatusProvide = [StatusID.AetherhuesIi];
    }

    static partial void ModifyWaterInBluePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.AetherhuesIi];
    }

    static partial void ModifyFireIiInRedPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Aetherhues];
    }

    static partial void ModifyAeroIiInGreenPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Aetherhues];
        setting.StatusProvide = [StatusID.AetherhuesIi];
    }

    static partial void ModifyWaterIiInBluePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.AetherhuesIi];
    }
    #endregion
}
