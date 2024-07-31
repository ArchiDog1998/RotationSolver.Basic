using static RotationSolver.Basic.CombatData;

namespace RotationSolver.Basic.Rotations.Basic;

partial class AstrologianRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Mind;

    private sealed protected override IBaseAction? Raise => AscendPvE;

    //static partial void ModifyAstrodynePvE(ref ActionSetting setting)
    //{
    //    setting.ActionCheck = () => !Seals.Contains(SealType.NONE);
    //    setting.CreateConfig = () => new()
    //    {
    //        TimeToKill = 10,
    //    };
    //}

    //static partial void ModifyDrawPvE(ref ActionSetting setting)
    //{
    //    setting.ActionCheck = () => DrawnCard == CardType.NONE;
    //}

    //static partial void ModifyRedrawPvE(ref ActionSetting setting)
    //{
    //    setting.StatusNeed = [StatusID.ClarifyingDraw];
    //    setting.ActionCheck = () => DrawnCard != CardType.NONE && Seals.Contains(GetCardSeal(DrawnCard));
    //}

    static partial void ModifyMinorArcanaPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => InCombat;
    }

    static partial void ModifyDivinationPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyEarthlyStarPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyGravityPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            AoeCount = 2,
        };
    }

    static partial void ModifyTheArrowPvE(ref ActionSetting setting)
    {
        setting.StatusFromSelf = false;
        setting.TargetType = TargetType.Melee;
        //setting.ActionCheck = () => DrawnCard == CardType.ARROW;
    }

    static partial void ModifyTheBalancePvE(ref ActionSetting setting)
    {
        setting.StatusFromSelf = false;
        setting.TargetType = TargetType.Melee;
        //setting.ActionCheck = () => DrawnCard == CardType.BALANCE;
    }
    
    static partial void ModifyTheBolePvE(ref ActionSetting setting)
    {
        setting.StatusFromSelf = false;
        setting.TargetType = TargetType.Range;
        //setting.ActionCheck = () => DrawnCard == CardType.BOLE;
    }

    static partial void ModifyTheEwerPvE(ref ActionSetting setting)
    {
        setting.StatusFromSelf = false;
        setting.TargetType = TargetType.Range;
        //setting.ActionCheck = () => DrawnCard == CardType.EWER;
    }

    static partial void ModifyTheSpearPvE(ref ActionSetting setting)
    {
        setting.StatusFromSelf = false;
        setting.TargetType = TargetType.Melee;
        //setting.ActionCheck = () => DrawnCard == CardType.SPEAR;
    }

    static partial void ModifyTheSpirePvE(ref ActionSetting setting)
    {
        setting.StatusFromSelf = false;
        setting.TargetType = TargetType.Range;
        //setting.ActionCheck = () => DrawnCard == CardType.SPIRE;
    }

    static partial void ModifyLightspeedPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyNeutralSectPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new()
        {
            TimeToKill = 15,
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="act"></param>
    /// <returns></returns>
    [Obsolete]
    protected bool PlayCard(out IAction? act)
    {
        if (TheBalancePvE.CanUse(out act)) return true;
        if (TheArrowPvE.CanUse(out act)) return true;
        if (TheSpearPvE.CanUse(out act)) return true;
        if (TheBolePvE.CanUse(out act)) return true;
        if (TheEwerPvE.CanUse(out act)) return true;
        if (TheSpirePvE.CanUse(out act)) return true;

        return false;
    }


    /// <inheritdoc/>
    protected override bool LimitBreakPvPGCD(out IAction? act)
    {
        if (CelestialRiverPvP.CanUse(out act, skipAoeCheck: true)) return true;
        return base.LimitBreakPvPGCD(out act);
    }
}
