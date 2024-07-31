using XIVConfigUI.Attributes;
using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class MonkRotation
{
    /// <summary/>
    [UI("Blue Chakra", Description = "I Love blue!")]
    [RotationConfig(CombatType.PvE)]
    public bool BlueChakra { get; set; }

    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Strength;

    #region Job Gauge
    /// <inheritdoc cref="Nadi.SOLAR"/>
    public static bool HasSolar => Nadi.HasFlag(Nadi.SOLAR);

    /// <inheritdoc cref="Nadi.LUNAR"/>
    public static bool HasLunar => Nadi.HasFlag(Nadi.LUNAR);

    /// <summary>
    /// 
    /// </summary>
    public static bool HasThreeBeast => BeastChakra.Count(i => i != Dalamud.Game.ClientState.JobGauge.Enums.BeastChakra.NONE) == 3;

    /// <summary>
    /// 
    /// </summary>
    public static bool HasThreeSameBeast
    {
        get
        {
            BeastChakra last = Dalamud.Game.ClientState.JobGauge.Enums.BeastChakra.NONE;
            foreach (var beast in BeastChakra)
            {
                if (beast == Dalamud.Game.ClientState.JobGauge.Enums.BeastChakra.NONE) return false;
                if (last == Dalamud.Game.ClientState.JobGauge.Enums.BeastChakra.NONE)
                {
                    last = beast;
                }
                else
                {
                    if (last != beast) return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static bool HasThreeDistinctBeast
    {
        get
        {
            if (!BeastChakra.Any(i => i == Dalamud.Game.ClientState.JobGauge.Enums.BeastChakra.OPOOPO)) return false;
            if (!BeastChakra.Any(i => i == Dalamud.Game.ClientState.JobGauge.Enums.BeastChakra.RAPTOR)) return false;
            if (!BeastChakra.Any(i => i == Dalamud.Game.ClientState.JobGauge.Enums.BeastChakra.COEURL)) return false;
            return true;
        }
    }
    #endregion

    #region Opo-opo Form
    static partial void ModifyDragonKickPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => OpoOpoFury == 0;
    }
    #endregion

    #region Raptor Form
    private static void RaptorForm(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.RaptorForm, StatusID.PerfectBalance, StatusID.FormlessFist];
    }

    static partial void ModifyTrueStrikePvE(ref ActionSetting setting)
    {
        RaptorForm(ref setting);
    }

    static partial void ModifyTwinSnakesPvE(ref ActionSetting setting)
    {
        RaptorForm(ref setting);
        setting.ActionCheck = () => RaptorFury == 0;
    }

    static partial void ModifyFourpointFuryPvE(ref ActionSetting setting)
    {
        RaptorForm(ref setting);
    }

    static partial void ModifyRisingRaptorPvE(ref ActionSetting setting)
    {
        RaptorForm(ref setting);
    }
    #endregion

    #region Coeurl Form
    private static void CoeurlForm(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.CoeurlForm, StatusID.PerfectBalance, StatusID.FormlessFist];
    }

    static partial void ModifySnapPunchPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Flank;
        CoeurlForm(ref setting);
    }

    static partial void ModifyDemolishPvE(ref ActionSetting setting)
    {
        CoeurlForm(ref setting);
        setting.EnemyPositional = EnemyPositional.Rear;
        setting.ActionCheck = () => CoeurlFury == 0;
    }

    static partial void ModifyRockbreakerPvE(ref ActionSetting setting)
    {
        CoeurlForm(ref setting);
    }

    static partial void ModifyPouncingCoeurlPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Flank;
        CoeurlForm(ref setting);
    }
    #endregion

    #region Chakra
    private static void ChakraAction(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat && Chakra >= 5;
    }
    static partial void ModifySteelPeakPvE(ref ActionSetting setting)
    {
        ChakraAction(ref setting);
    }

    static partial void ModifyHowlingFistPvE(ref ActionSetting setting)
    {
        ChakraAction(ref setting);
    }

    static partial void ModifyTheForbiddenChakraPvE(ref ActionSetting setting)
    {
        ChakraAction(ref setting);
    }

    static partial void ModifyEnlightenmentPvE(ref ActionSetting setting)
    {
        ChakraAction(ref setting);
    }
    #endregion

    #region Masterfull Blitz
    static partial void ModifyMasterfulBlitzPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => false;
    }

    static partial void ModifyTornadoKickPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () =>
        {
            if (Nadi != (Nadi.LUNAR | Nadi.SOLAR)) return false;
            return HasThreeBeast;
        };
    }

    static partial void ModifyPhantomRushPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () =>
        {
            if (Nadi != (Nadi.LUNAR | Nadi.SOLAR)) return false;
            return HasThreeBeast;
        };
    }

    static partial void ModifyFlintStrikePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasThreeDistinctBeast;
    }

    static partial void ModifyRisingPhoenixPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasThreeDistinctBeast;
    }

    static partial void ModifyElixirFieldPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasThreeSameBeast;
    }

    static partial void ModifyElixirBurstPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasThreeSameBeast;
    }

    static partial void ModifyCelestialRevolutionPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => HasThreeBeast;
    }
    #endregion

    #region Riddle And Reply

    static partial void ModifyEarthsReplyPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.EarthsRumination];
    }

    static partial void ModifyRiddleOfFirePvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyFiresReplyPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.FiresRumination];
    }

    static partial void ModifyRiddleOfWindPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyWindsReplyPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.WindsRumination];
    }
    #endregion

    #region Mediation PvE
    private static void Mediation(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Chakra < 5;
    }

    static partial void ModifyEnlightenedMeditationPvE(ref ActionSetting setting)
    {
        Mediation(ref setting);
    }

    static partial void ModifyInspiritedMeditationPvE(ref ActionSetting setting)
    {
        Mediation(ref setting);
    }

    static partial void ModifyForbiddenMeditationPvE(ref ActionSetting setting)
    {
        Mediation(ref setting);
    }

    static partial void ModifySteeledMeditationPvE(ref ActionSetting setting)
    {
        Mediation(ref setting);
    }

    /// <summary>
    /// Do the mediation
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    public bool DoMediationPvE(out IAction? act)
    {
        if (BlueChakra)
        {
            if (EnlightenedMeditationPvE.EnoughLevel)
            {
                if (EnlightenedMeditationPvE.CanUse(out act)) return true;
            }
            else
            {
                if (InspiritedMeditationPvE.CanUse(out act)) return true;
            }
        }
        if (ForbiddenMeditationPvE.EnoughLevel)
        {
            if (ForbiddenMeditationPvE.CanUse(out act)) return true;
        }
        else
        {
            if (SteeledMeditationPvE.CanUse(out act)) return true;
        }
        return false;
    }
    #endregion

    static partial void ModifyPerfectBalancePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat;
    }
    static partial void ModifyFormShiftPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.FormlessFist, StatusID.PerfectBalance];
    }

    static partial void ModifyMantraPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyBrotherhoodPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyThunderclapPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyThunderclapPvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    /// <inheritdoc/>
    protected override bool MoveForwardAbility(out IAction? act)
    {
        if (ThunderclapPvE.CanUse(out act)) return true;
        return base.MoveForwardAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool DefenseAreaAbility(out IAction? act)
    {
        if (FeintPvE.CanUse(out act)) return true;
        return base.DefenseAreaAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool HealAreaAbility(out IAction? act)
    {
        if (MantraPvE.CanUse(out act)) return true;
        if (EarthsReplyPvE.CanUse(out act)) return true;
        return base.HealAreaAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool DefenseSingleAbility(out IAction? act)
    {
        if (RiddleOfEarthPvEReplace.CanUse(out act, usedUp: true)) return true;
        return base.DefenseSingleAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (MeteodrivePvP.CanUse(out act, skipAoeCheck: true)) return true;
        return base.LimitBreakPvPGCD(out act);
    }
}
