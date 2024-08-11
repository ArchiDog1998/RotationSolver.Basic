using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class PaladinRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Strength;

    /// <summary/>
    public override bool CanHealSingleSpell => DataCenter.PartyMembers.Length == 1 && base.CanHealSingleSpell;

    /// <summary/>
    public override bool CanHealAreaAbility => false;

    /// <summary>
    /// Has <see cref="StatusID.DivineMight"/>
    /// </summary>
    public static bool HasDivineMight => !Player.WillStatusEndGCD(0, 0, true, StatusID.DivineMight);

    /// <summary>
    /// Has <see cref="StatusID.FightOrFlight"/>
    /// </summary>
    public static bool HasFightOrFlight => !Player.WillStatusEndGCD(0, 0, true, StatusID.FightOrFlight);

    static partial void ModifyAtonementPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.AtonementReady];
        setting.StatusProvide = [StatusID.SupplicationReady];
    }

    static partial void ModifySupplicationPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.SupplicationReady];
        setting.StatusProvide = [StatusID.SepulchreReady];
    }

    static partial void ModifySepulchrePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.SepulchreReady];
    }

    static partial void ModifyConfiteorPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.ConfiteorReady];
    }

    static partial void ModifyBladeOfHonorPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.BladeOfHonorReady];
    }

    static partial void ModifyProminencePvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.DivineMight];
    }

    static partial void ModifyGoringBladePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.GoringBladeReady];
    }

    static partial void ModifyRoyalAuthorityPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.DivineMight];
        setting.StatusProvide = [StatusID.AtonementReady];
    }

    static partial void ModifyShieldBashPvE(ref ActionSetting setting)
    {
        setting.CanTarget = o =>
        {
            if (o is not IBattleChara b) return false;

            if (b.IsBossFromIcon() || IsMoving || b.CastActionId == 0) return false;

            if (!b.IsCastInterruptible || ActionID.InterjectPvE.IsCoolingDown()) return true;
            return false;
        };
        setting.ActionCheck = () => ActionID.LowBlowPvE.IsCoolingDown();
        setting.StatusProvide = [StatusID.Stun];
    }

    static partial void ModifyShieldLobPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MeleeRange;
        setting.TargetType = TargetType.ProvokeOrOthers;
    }

    private protected sealed override IBaseActionSet TankStance => IronWillPvEReplace;

    static partial void ModifyRequiescatPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyFightOrFlightPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifySentinelPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = StatusHelper.RampartStatus;
        setting.ActionCheck = Player.IsTargetOnSelf;
    }

    static partial void ModifyBulwarkPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = StatusHelper.RampartStatus;
        setting.ActionCheck = Player.IsTargetOnSelf;
    }

    static partial void ModifyCoverPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => OathGauge >= 50;
    }

    static partial void ModifyIntervenePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => OathGauge >= 50;
    }

    static partial void ModifyHolySheltronPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => OathGauge >= 50 && Player.IsTargetOnSelf();
    }

    static partial void ModifyIntervenePvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    /// <inheritdoc/>
    protected override bool EmergencyAbility(IAction nextGCD, out IAction? act)
    {
        if (HallowedGroundPvE.CanUse(out act)
            && Player.GetHealthRatio() <= Service.Config.HealthForDyingTanks) return true;
        return base.EmergencyAbility(nextGCD, out act);
    }

    /// <inheritdoc/>
    protected override bool MoveForwardAbility(out IAction? act)
    {
        if (IntervenePvE.CanUse(out act)) return true;
        return base.MoveForwardAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool HealSingleGCD(out IAction? act)
    {
        if (ClemencyPvE.CanUse(out act)) return true;
        return base.HealSingleGCD(out act);
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (PhalanxPvP.CanUse(out act, skipAoeCheck: true)) return true;
        return false;
    }
}