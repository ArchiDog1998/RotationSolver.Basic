﻿namespace RotationSolver.Basic.Data;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public enum WhyActionCantUse : byte
{
    [Description("It can be used!")]
    None = 0,

    [Description("This action was disabled by the user or not on the slot.")]
    Disabled,

    [Description("This action was disabled by the action sequencer!")]
    DisabledSequencer,

    [Description("The level is not enough!")]
    NotEnoughLevel,

    [Description("Not enough MP.")]
    NoMp,

    [Description("There are no statuses that the action need!")]
    NoStatusNeed,

    [Description("There are the statuses that this action provide!")]
    HasTheStatus,

    [Description("There are the penalty statuses of this action!")]
    HasThePenaltyStatus,

    [Description("Not enough Enough Limit Level!")]
    LimitBreakPvP,

    [Description("The combo doesn't meet.")]
    Combo,

    [Description("The job for this action doesn't meet.")]
    JobMeet,

    [Description("For now, the command asked to not casting anything.")]
    NoCasting,

    [Description("It is knocking back now, can't cast.")]
    KnockingBack,

    [Description("The player is moving, can't cast.")]
    Moving,

    [Description("The Action Check doesn't allow this to be used.")]
    ActionCheck,

    [Description("The Rotation Developer's Rotation Check doesn't allow this to be used.")]
    RotationCheck,

    [Description("The action status doesn't allow this to be used.")]
    BadStatus,

    [Description("The action needs some resources!")]
    ActionResources,

    [Description("This 0GCD should be used as late as possible")]
    OnLast,

    [Description("If it use this 0GCD now, it'll clipping!")]
    Clipping,

    [Description("Keep at least one charge, because the developer asked to not empty!")]
    NotEmpty,

    [Description("This gcd won't get one charge on the next gcd.")]
    NoChargesGCD,

    [Description("This 0gcd won't get one charge on the next 0gcd.")]
    NoCharges0GCD,

    [Description("Don't Use range actions after moving!")]
    NoRangeActionsAfterMovingForMelee,

    [Description("The TTK is too low for this action.")]
    TTK,

    [Description("The time to the untargetable it too low")]
    TimeToUntargetable,

    [Description("Can't find the correct Target.")]
    Target,

    [Description("This action isn't highlighted.")]
    Highlight,

    [Description("It is changed to another action!")]
    MaxLevel,
}
