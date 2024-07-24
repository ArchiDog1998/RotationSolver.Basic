using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class WhiteMageRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Mind;

    private protected sealed override IBaseAction Raise => RaisePvE;

    static partial void ModifySeraphStrikePvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyMedicaIiPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.MedicaIi, StatusID.TrueMedicaIi];
    }

    static partial void ModifyRegenPvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [
            StatusID.Regen,
            StatusID.Regen_897,
            StatusID.Regen_1330,
        ];
    }

    static partial void ModifyHolyPvE(ref ActionSetting setting)
    {
        setting.IsFriendly = false;
    }

    static partial void ModifyHolyIiiPvE(ref ActionSetting setting)
    {
        setting.IsFriendly = false;
    }

    static partial void ModifyAfflatusSolacePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Lily > 0;
    }

    static partial void ModifyDivineBenisonPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.DivineBenison];
    }

    static partial void ModifyAfflatusRapturePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Lily > 0;
    }

    static partial void ModifyAeroPvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.Aero];
    }

    static partial void ModifyAeroIiPvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.AeroIi];
    }

    static partial void ModifyDiaPvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.Dia];
    }

    static partial void ModifyAfflatusMiseryPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => BloodLily == 3;
    }

    static partial void ModifyPresenceOfMindPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => !IsMoving;
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyCureIiiPvP(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.CureIiiReady];
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (AfflatusPurgationPvP.CanUse(out act, skipAoeCheck: true)) return true;
        return base.LimitBreakPvPGCD(out act);
    }
}