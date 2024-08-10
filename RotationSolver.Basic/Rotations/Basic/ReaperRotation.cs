﻿using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class ReaperRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Strength;

    /// <summary>
    /// Has <see cref="StatusID.Enshrouded"/>
    /// </summary>
    public static bool HasEnshrouded => Player.HasStatus(true, StatusID.Enshrouded);

    /// <summary>
    /// Has <see cref="StatusID.SoulReaver"/>
    /// </summary>
    public static bool HasSoulReaver => Player.HasStatus(true, StatusID.SoulReaver);

    /// <summary>
    /// Has <see cref="StatusID.Executioner"/>
    /// </summary>
    public static bool HasExecutioner => Player.HasStatus(true, StatusID.Executioner);

    /// <summary>
    /// Has <see cref="StatusID.IdealHost"/>
    /// </summary>
    public static bool HasIdealHost => Player.HasStatus(true, StatusID.IdealHost);

    /// <summary>
    /// Has <see cref="StatusID.Oblatio"/>
    /// </summary>
    public static bool HasOblatio => Player.HasStatus(true, StatusID.Oblatio);

    #region Death
    static partial void ModifyShadowOfDeathPvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.DeathsDesign];
    }

    static partial void ModifyWhorlOfDeathPvE(ref ActionSetting setting)
    {
        setting.TargetStatusProvide = [StatusID.DeathsDesign];
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    /// <summary>
    /// Use
    /// <see cref="WhorlOfDeathPvE"/>, <see cref="ShadowOfDeathPvE"/>,
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    public bool UseDeathActions(out IAction? act)
    {
        if (WhorlOfDeathPvE.CanUse(out act)) return true;
        if (ShadowOfDeathPvE.CanUse(out act)) return true;
        return false;
    }
    #endregion

    #region G Thing
    #region Soul Reaver
    static partial void ModifyGibbetPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Flank;
        setting.StatusNeed = [StatusID.SoulReaver];
    }

    static partial void ModifyGallowsPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Rear;
        setting.StatusNeed = [StatusID.SoulReaver];
    }

    static partial void ModifyGuillotinePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.SoulReaver];
    }
    #endregion

    #region Executioner
    static partial void ModifyExecutionersGibbetPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Flank;
        setting.StatusNeed = [StatusID.Executioner];
    }

    static partial void ModifyExecutionersGallowsPvE(ref ActionSetting setting)
    {
        setting.EnemyPositional = EnemyPositional.Rear;
        setting.StatusNeed = [StatusID.Executioner];
    }

    static partial void ModifyExecutionersGuillotinePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Executioner];
    }
    #endregion

    /// <summary>
    /// Use 
    /// <see cref="ExecutionersGuillotinePvE"/>, <see cref="GuillotinePvE"/>,
    /// <see cref="ExecutionersGibbetPvE"/>, <see cref="GibbetPvE"/>,
    /// <see cref="ExecutionersGallowsPvE"/>, <see cref="GallowsPvE"/>,
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    public bool UseGThings(out IAction? act)
    {
        if (ExecutionersGuillotinePvE.CanUse(out act)) return true;
        if (GuillotinePvE.CanUse(out act)) return true;

        if (!Player.WillStatusEndGCD(0, 0, true, StatusID.EnhancedGibbet))
        {
            if (ExecutionersGibbetPvE.CanUse(out act)) return true;
            if (GibbetPvE.CanUse(out act)) return true;
        }

        if (ExecutionersGallowsPvE.CanUse(out act)) return true;
        if (GallowsPvE.CanUse(out act)) return true;

        return false;
    }
    #endregion


    #region Soul Getter
    static partial void ModifySoulSlicePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Soul <= 50;
    }

    static partial void ModifySoulScythePvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Soul <= 50;
    }
    #endregion

    #region Soul User
    private static void SoulUser(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.SoulReaver, StatusID.Executioner];
        setting.ActionCheck = () => Soul >= 50;
    }

    static partial void ModifyBloodStalkPvE(ref ActionSetting setting)
    {
        SoulUser(ref setting);
    }

    static partial void ModifyGrimSwathePvE(ref ActionSetting setting)
    {
        SoulUser(ref setting);
    }

    static partial void ModifyGluttonyPvE(ref ActionSetting setting)
    {
        SoulUser(ref setting);
    }

    static partial void ModifyEnshroudPvE(ref ActionSetting setting)
    {
        SoulUser(ref setting);
    }
    #endregion

    #region Enshrouded
    static partial void ModifySacrificiumPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Enshrouded];
        setting.ActionCheck = () => Player.HasStatus(true, StatusID.Oblatio);
    }


    static partial void ModifyLemuresSlicePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Enshrouded];
        setting.ActionCheck = () => VoidShroud >= 2;
    }

    static partial void ModifyLemuresScythePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Enshrouded];
        setting.ActionCheck = () => VoidShroud >= 2;
    }

    static partial void ModifyCommunioPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Enshrouded];
        setting.ActionCheck = () => LemureShroud == 1;
    }

    static partial void ModifyVoidReapingPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Enshrouded];
        setting.ActionCheck = () => LemureShroud >= 1;
    }

    static partial void ModifyCrossReapingPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Enshrouded];
        setting.ActionCheck = () => LemureShroud >= 1;
    }

    static partial void ModifyGrimReapingPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Enshrouded];
        setting.ActionCheck = () => LemureShroud >= 1;
    }
    
    /// <summary>
    /// Use <see cref="LemureShroud"/>
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    public bool UseEnshroudedGCD(out IAction? act)
    {
        if (CommunioPvE.CanUse(out act, skipAoeCheck: true)) return true;

        if (GrimReapingPvE.CanUse(out act)) return true;
        if (!Player.WillStatusEndGCD(0, 0, true, StatusID.EnhancedCrossReaping))
        {
            if (CrossReapingPvE.CanUse(out act)) return true;
        }
        else
        {
            if (VoidReapingPvE.CanUse(out act)) return true;
        }
        return false;
    }

    /// <summary>
    /// Use <see cref="VoidShroud"/>
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    public bool UseEnshroudedAbility(out IAction? act)
    {
        if (SacrificiumPvE.CanUse(out act, skipAoeCheck: true)) return true;
        if (LemuresScythePvE.CanUse(out act)) return true;
        if (LemuresSlicePvE.CanUse(out act)) return true;
        return false;
    }
    #endregion

    #region Range
    static partial void ModifyHarpePvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MeleeRange;
    }

    static partial void ModifySoulsowPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Soulsow];
        setting.ActionCheck = () => !InCombat;
    }

    static partial void ModifyHarvestMoonPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Soulsow];
    }
    #endregion

    #region Move
    static partial void ModifyHellsIngressPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
        setting.StatusProvide = [StatusID.EnhancedHarpe];
    }

    static partial void ModifyHellsEgressPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.EnhancedHarpe];
    }
    #endregion

    static partial void ModifyPerfectioPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.PerfectioParata];
    }

    static partial void ModifyArcaneCirclePvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyPlentifulHarvestPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.ImmortalSacrifice];
        setting.StatusProvide = [StatusID.BloodsownCircle_2972];
    }

    /// <inheritdoc/>
    protected override bool MoveForwardAbility(out IAction? act)
    {
        if (HellsIngressPvE.CanUse(out act)) return true;
        return base.MoveForwardAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool DefenseAreaAbility(out IAction? act)
    {
        if (!HasSoulReaver && !HasEnshrouded && !HasExecutioner && FeintPvE.CanUse(out act)) return true;
        return base.DefenseAreaAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool DefenseSingleAbility(out IAction? act)
    {
        if (!HasSoulReaver && !HasEnshrouded && !HasExecutioner && ArcaneCrestPvE.CanUse(out act)) return true;
        return base.DefenseSingleAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool MoveBackAbility(out IAction? act)
    {
        if (HellsEgressPvE.CanUse(out act)) return true;
        return base.MoveBackAbility(out act);
    }

    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (TenebraeLemurumPvP.CanUse(out act)) return true;
        return base.LimitBreakPvPGCD(out act);
    }
}