using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
partial class SageRotation
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Mind;

    private protected sealed override IBaseAction Raise => EgeiroPvE;

    /// <summary>
    /// <see cref="EukrasianDosisPvE"/> -> <see cref="EukrasianDosisIiPvE"/> -><see cref="EukrasianDosisIiiPvE"/>
    /// </summary>
    public IBaseActionSet EukrasianDosisPvEReplace { get; private set; }
    internal override void Init()
    {
        EukrasianDosisPvEReplace = new BaseActionSet(() => [EukrasianDosisIiiPvE, EukrasianDosisIiPvE, EukrasianDosisPvE], true);
        base.Init();
    }

    static partial void ModifyIcarusPvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyEukrasianDiagnosisPvE(ref ActionSetting setting)
    {
        setting.TargetType = TargetType.BeAttacked;
        setting.TargetStatusProvide = [StatusID.EukrasianDiagnosis];
    }

    static partial void ModifyEukrasianDosisPvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide =
        [
            StatusID.EukrasianDosis,
            StatusID.EukrasianDosisIi,
            StatusID.EukrasianDosisIii
        ];
    }

    static partial void ModifyDyskrasiaPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyToxikonPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Addersting > 0;
    }

    static partial void ModifyRhizomataPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Addersting < 3;
    }

    static partial void ModifyKardiaPvE(ref ActionSetting setting)
    {
        setting.TargetType = TargetType.Tank;
        setting.ActionCheck = () => !DataCenter.AllianceMembers.Any(m => m.HasStatus(true, StatusID.Kardion));
    }

    static partial void ModifyEukrasiaPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => !Eukrasia;
    }

    static partial void ModifyKeracholePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Addersgall > 0;
    }

    static partial void ModifyIxocholePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Addersgall > 0;
    }

    static partial void ModifyTaurocholePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Addersgall > 0;
    }

    static partial void ModifyDruocholePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Addersgall > 0;
    }

    static partial void ModifyPepsisPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () =>
        {
            foreach (var chara in DataCenter.PartyMembers)
            {
                if (chara.HasStatus(true, StatusID.EukrasianDiagnosis, StatusID.EukrasianPrognosis)
                && chara.GetHealthRatio() < 0.9) return true;
            }

            return false;
        };
    }

    static partial void ModifyIcarusPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    /// <inheritdoc/>
    protected override bool MoveForwardAbility(out IAction? act)
    {
        if (IcarusPvE.CanUse(out act)) return true;
        return base.MoveForwardAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (MesotesPvP.CanUse(out act)) return true;
        if (MesotesPvP_29267.CanUse(out act)) return true;
        return base.LimitBreakPvPGCD(out act);
    }
}