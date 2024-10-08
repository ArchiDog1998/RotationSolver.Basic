﻿namespace RotationSolver.Basic.Data;

internal enum UiString
{
    None,

    [Description("The condition set you will use, click to modify.")]
    ConfigWindow_ConditionSetDesc,

    [Description("The rotation you will use, click to modify.")]
    ConfigWindow_DutyRotationDesc,

    [Description("Load From folder.")]
    ActionSequencer_Load,

    [Description("Game Version")]
    ConfigWindow_Helper_GameVersion,

    [Description("Invalid Rotation! Please contact to the {0}!")]
    ConfigWindow_Rotation_InvalidRotation,

    [Description("Beta Rotation!")]
    ConfigWindow_Rotation_BetaRotation,

    [Description("Click to switch rotations")]
    ConfigWindow_Helper_SwitchRotation,

    [Description("Well, you must be a lazy player!")]
    ConfigWindow_About_Clicking100k,

    [Description("You're tiring RS out, give it a break!")]
    ConfigWindow_About_Clicking500k,

    [Description("Rotation Solver helped you by clicking actions {0:N0} times.")]
    ConfigWindow_About_ClickingCount,

    [Description("You have said hello to other RS users {0:N0} times!")]
    ConfigWindow_About_SayHelloCount,

    [Description("There are {0} RS users among the world!")]
    ConfigWindow_About_UserCount,

    [Description("Macro")]
    ConfigWindow_About_Macros,

    [Description("Compatibility")]
    ConfigWindow_About_Compatibility,

    [Description("Supporters")]
    ConfigWindow_About_Supporters,

    [Description("Links")]
    ConfigWindow_About_Links,

    [Description("Please don't relog without closing the game. Crashes may occur.")]
    ConfigWindow_About_Compatibility_Others,

    [Description("Literally, Rotation Solver helps you to choose the target and then clicks the action for you. So any plugin that interacts with these will affect its decision.\n\nHere is a list of known incompatible plugins:")]
    ConfigWindow_About_Compatibility_Description,

    [Description("Can't properly execute the behavior that RS wants to do.")]
    ConfigWindow_About_Compatibility_Mistake,

    [Description("Misleading RS to make the right decision.")]
    ConfigWindow_About_Compatibility_Mislead,

    [Description("Causes the game to crash.")]
    ConfigWindow_About_Compatibility_Crash,

    [Description("Many thanks to everyone that is supporting and enjoying the plugin.")]
    ConfigWindow_About_ThanksToSupporters,

    [Description("Open config Folder")]
    ConfigWindow_About_OpenConfigFolder,

    [Description("Description")]
    ConfigWindow_Rotation_Description,

    [Description("Configuration")]
    ConfigWindow_Rotation_Configuration,

    [Description("Rating")]
    ConfigWindow_Rotation_Rating,

    [Description("Information")]
    ConfigWindow_Rotation_Information,

    [Description("Here are some rating methods to analysis this rotation. Most of these methods need your engagement.")]
    ConfigWindow_Rotation_Rating_Description,

    [Description("This is the count of using last action checking in this rotation. First is average one, second is maximum one. The less the better.\nLast used action is not a part of information from the game, it is recorded by player or author. \nIt can't accurately describe the current state of combat, which may make this rotation not general. \nFor example, clipping the gcd, death, take some status that grated by some action off manually, etc.")]
    ConfigWindow_Rotation_Rating_CountOfLastUsing,

    [Description("This is the count of using combat time in this rotation. First is average one, second is maximum one. The less the better.\nCombat time is not a part of information from the game, it is recorded by player or author. \nIt can't accurately describe the current state of combat, which may make this rotation not general.\nFor example, engaged by others in the party, different gcd time, etc.")]
    ConfigWindow_Rotation_Rating_CountOfCombatTimeUsing,

    [Description("Status")]
    ConfigWindow_Rotation_Status,

    [Description("To customize when Rotation Solver uses specific actions automatically, click on an action's icon in the left list. Below, you may set the conditions for when that specific action is used. Each action can have a different set of conditions to override the default rotation behavior.")]
    ConfigWindow_Actions_Description,

    [Description("Show on CD window")]
    ConfigWindow_Actions_ShowOnCDWindow,

    [Description("Can be used by mistake")]
    ConfigWindow_Actions_IsInMistake,

    [Description("Minimum target time to kill at which the ability will be used.")]
    ConfigWindow_Actions_TTK,

    [Description("Custom Targeting Way")]
    ConfigWindow_Actions_CustomTargetingData,

    [Description("The list is in Target - Hostile")]
    ConfigWindow_Actions_CustomTargetingData_Description,

    [Description("Minimum time until the target becomes untargetable that the ability will be used at.")]
    ConfigWindow_Actions_TTU,

    [Description("Minimum number of targets that are needed for this ability to be used.")]
    ConfigWindow_Actions_AoeCount,

    [Description("Should this action check if it was not already used by another player.")]
    ConfigWindow_Actions_CheckStatus,

    [Description("How many gcds are needed to add the status.")]
    ConfigWindow_Actions_GcdCount,

    [Description("The HP ratio to auto heal")]
    ConfigWindow_Actions_HealRatio,

    [Description("Forced Conditions have a higher priority. If Forced Conditions are met, Disabled Conditions will be ignored.\nNo Target Conditions have a higher priority. If No Target Conditions are met, Priority Target Conditions will be ignored.")]
    ConfigWindow_Actions_ConditionDescription,

    [Description("Forced Conditions")]
    ConfigWindow_Actions_ForcedConditionSet,

    [Description("Conditions for forced automatic use of action.")]
    ConfigWindow_Actions_ForcedConditionSet_Description,

    [Description("Disabled Condition")]
    ConfigWindow_Actions_DisabledConditionSet,

    [Description("Conditions for automatic use of action being disabled.")]
    ConfigWindow_Actions_DisabledConditionSet_Description,

    [Description("Priority Target Conditions")]
    ConfigWindow_Actions_PriorityTargeting,

    [Description("Conditions for the target that you want to prioritize.")]
    ConfigWindow_Actions_PriorityTargeting_Description,

    [Description("No Target Conditions")]
    ConfigWindow_Actions_CantTargeting,

    [Description("Conditions for the target that you don't want to target.")]
    ConfigWindow_Actions_CantTargeting_Description,

    [Description("Download Rotations")]
    ConfigWindow_Rotations_Download,

    [Description("Links of the rotations online")]
    ConfigWindow_Rotations_Links,

    [Description("Settings")]
    ConfigWindow_Rotations_Settings,

    [Description("Loaded")]
    ConfigWindow_Rotations_Loaded,

    [Description("GitHub")]
    ConfigWindow_Rotations_GitHub,

    [Description("Libraries")]
    ConfigWindow_Rotations_Libraries,

    [Description("User Name")]
    ConfigWindow_Rotations_UserName,

    [Description("Repository")]
    ConfigWindow_Rotations_Repository,

    [Description("File Name")]
    ConfigWindow_Rotations_FileName,

    [Description("The folder contains rotation libs or the download url about rotation lib.")]
    ConfigWindow_Rotations_Library,

    [Description("In this window, you can set the parameters that can be customized using lists.")]
    ConfigWindow_List_Description,

    [Description("Statuses")]
    ConfigWindow_List_Statuses,

    [Description("Actions")]
    ConfigWindow_List_Actions,

    [Description("Map specific settings")]
    ConfigWindow_List_Territories,

    [Description("Action Groups")]
    ConfigWindow_List_ActionGroups,

    [Description("The Name of it")]
    ConfigWindow_List_ActionGroups_Name,

    [Description("Is this group enabled. True to turn all actions on.")]
    ConfigWindow_List_ActionGroups_Enable,

    [Description("Show this action group in the action group window.")]
    ConfigWindow_List_ActionGroups_Show,

    [Description("Status name or id")]
    ConfigWindow_List_StatusNameOrId,

    [Description("Invulnerability")]
    ConfigWindow_List_Invincibility,

    [Description("Priority")]
    ConfigWindow_List_Priority,

    [Description("Dispellable debuffs")]
    ConfigWindow_List_DangerousStatus,

    [Description("No Casting debuffs")]
    ConfigWindow_List_NoCastingStatus,

    [Description("Ignores target if it has one of these statuses")]
    ConfigWindow_List_InvincibilityDesc,

    [Description("Attacks the target first if it has one of these statuses")]
    ConfigWindow_List_PriorityDesc,

    [Description("Dispellable debuffs list")]
    ConfigWindow_List_DangerousStatusDesc,

    [Description("No Casting debuffs List")]
    ConfigWindow_List_NoCastingStatusDesc,

    [Description("Open the timeline link")]
    Timeline_OpenLink,

    [Description("Add Status")]
    ConfigWindow_List_AddStatus,

    [Description("Action name or id")]
    ConfigWindow_List_ActionNameOrId,

    [Description("Tank Buster")]
    ConfigWindow_List_HostileCastingTank,

    [Description("AoE")]
    ConfigWindow_List_HostileCastingArea,

    [Description("Knockback")]
    ConfigWindow_List_HostileCastingKnockback,

    [Description("Use tank personal damage mitigation abilities if the target is casting any of these actions")]
    ConfigWindow_List_HostileCastingTankDesc,

    [Description("Use AoE damage mitigation abilities if the target is casting any of these actions")]
    ConfigWindow_List_HostileCastingAreaDesc,

    [Description("Use knockback prevention abilities if the target is casting any of these actions")]
    ConfigWindow_List_HostileCastingKnockbackDesc,

    [Description("Add Action")]
    ConfigWindow_List_AddAction,

    [Description("Don't target")]
    ConfigWindow_List_NoHostile,

    [Description("Don't provoke")]
    ConfigWindow_List_NoProvoke,

    [Description("Beneficial AoE locations")]
    ConfigWindow_List_BeneficialPositions,

    [Description("Enemies that will never be targeted.")]
    ConfigWindow_List_NoHostileDesc,

    [Description("The name of the enemy that you don't want to be targeted")]
    ConfigWindow_List_NoHostilesName,

    [Description("Enemies that will never be provoked.")]
    ConfigWindow_List_NoProvokeDesc,

    [Description("The name of the enemy that you don't want to be provoked")]
    ConfigWindow_List_NoProvokeName,

    [Description("Time")]
    ConfigWindow_Timeline_Time,

    [Description("Name")]
    ConfigWindow_Timeline_Name,

    [Description("Actions")]
    ConfigWindow_Timeline_Actions,

    [Description("Job Actions")]
    ConfigWindow_Timeline_JobActions,

    [Description("Log")]
    ConfigWindow_Trigger_Log,

    [Description("Trigger Data")]
    ConfigWindow_Trigger_TriggerData,

    [Description("Left-click to add the general Trigger Data.\nRight-click to add the job Trigger Data.")]
    ConfigWindow_Trigger_AddTriggerDataDesc,

    [Description("Clear Log")]
    ConfigWindow_Trigger_Clear,

    [Description("OOOps! RS clicked the wrong action ({0})!")]
    ClickingMistakeMessage,

    [Description("Ability")]
    ActionAbility,

    [Description("Friendly")]
    ActionFriendly,

    [Description("Attack")]
    ActionAttack,

    [Description("Normal Targets")]
    NormalTargets,

    [Description("Targets with HOT")]
    HotTargets,

    [Description("HP for AoE healing oGCDs")]
    HpAoe0Gcd,

    [Description("HP for AoE healing GCDs")]
    HpAoeGcd,

    [Description("HP for ST healing oGCDs")]
    HpSingle0Gcd,

    [Description("HP for ST healing GCDs")]
    HpSingleGcd,

    [Description("No Move")]
    InfoWindowNoMove,

    [Description("Move")]
    InfoWindowMove,

    [Description("Timer")]
    ConfigWindow_Basic_Timer,

    [Description("Auto Switch")]
    ConfigWindow_Basic_AutoSwitch,

    [Description("Named Conditions")]
    ConfigWindow_Basic_NamedConditions,

    [Description("Others")]
    ConfigWindow_Basic_Others,

    [Description("The ping time.\nIn RS, it means the time from sending the action request to receiving the using success message from the server.")]
    ConfigWindow_Basic_Ping,

    [Description("The Animation lock time from individual actions. Here is 0.6s for example.")]
    ConfigWindow_Basic_AnimationLockTime,

    [Description("The clicking duration, RS will try to click at this moment.")]
    ConfigWindow_Basic_ClickingDuration,

    [Description("The ideal click time.")]
    ConfigWindow_Basic_IdealClickingTime,

    [Description("The real click time.")]
    ConfigWindow_Basic_RealClickingTime,

    [Description("Auto turn off conditions")]
    ConfigWindow_Basic_SwitchCancelConditionSet,

    [Description("Auto turn manual conditions")]
    ConfigWindow_Basic_SwitchManualConditionSet,

    [Description("Auto turn auto conditions")]
    ConfigWindow_Basic_SwitchAutoConditionSet,

    [Description("Condition Name")]
    ConfigWindow_Condition_ConditionName,

    [Description("Information")]
    ConfigWindow_UI_Information,

    [Description("Overlay")]
    ConfigWindow_UI_Overlay,

    [Description("Windows")]
    ConfigWindow_UI_Windows,

    [Description("Change the way that RS automatically uses actions.")]
    ConfigWindow_Auto_Description,

    [Description("Action Usage")]
    ConfigWindow_Auto_ActionUsage,

    [Description("Which actions Rotation Solver can use.")]
    ConfigWindow_Auto_ActionUsage_Description,

    [Description("Action Condition")]
    ConfigWindow_Auto_ActionCondition,

    [Description("State Condition")]
    ConfigWindow_Auto_StateCondition,

    [Description("Heal Area Forced Condition")]
    ConfigWindow_Auto_HealAreaConditionSet,

    [Description("Heal Single Forced Condition")]
    ConfigWindow_Auto_HealSingleConditionSet,

    [Description("Defense Area Forced Condition")]
    ConfigWindow_Auto_DefenseAreaConditionSet,

    [Description("Defense Single Forced Condition")]
    ConfigWindow_Auto_DefenseSingleConditionSet,

    [Description("Dispel Stance Positional Forced Condition")]
    ConfigWindow_Auto_DispelStancePositionalConditionSet,

    [Description("Raise Shirk Forced Condition")]
    ConfigWindow_Auto_RaiseShirkConditionSet,

    [Description("Move Forward Forced Condition")]
    ConfigWindow_Auto_MoveForwardConditionSet,

    [Description("Move Back Forced Condition")]
    ConfigWindow_Auto_MoveBackConditionSet,

    [Description("Anti Knockback Forced Condition")]
    ConfigWindow_Auto_AntiKnockbackConditionSet,

    [Description("Speed Forced Condition")]
    ConfigWindow_Auto_SpeedConditionSet,

    [Description("Limit Break Forced Condition")]
    ConfigWindow_Auto_LimitBreakConditionSet,

    [Description("This will change the way that Rotation Solver uses actions.")]
    ConfigWindow_Auto_ActionCondition_Description,

    [Description("Priority Target")]
    ConfigWindow_Target_Priority,

    [Description("No Target")]
    ConfigWindow_Target_Cant,

    [Description("Configuration")]
    ConfigWindow_Target_Config,

    [Description("Hostile")]
    ConfigWindow_List_Hostile,

    [Description("The logic of hostile target selection to allow flexibility in switching the logic of selecting hostile in battle.")]
    ConfigWindow_Param_HostileDesc,

    [Description("Rotation Solver focuses on the rotation itself. These are side features. If there are some other plugins can do that, these features will be deleted.")]
    ConfigWindow_Extra_Description,

    [Description("Event")]
    ConfigWindow_EventItem,

    [Description("Others")]
    ConfigWindow_Extra_Others,

    [Description("Add Events")]
    ConfigWindow_Events_AddEvent,

    [Description("In this window, you can set what macro will be triggered after using an action.")]
    ConfigWindow_Events_Description,

    [Description("Duty Start: ")]
    ConfigWindow_Events_DutyStart,

    [Description("Duty End: ")]
    ConfigWindow_Events_DutyEnd,

    [Description("Delete Event")]
    ConfigWindow_Events_RemoveEvent,

    [Description("Click to make it reverse.\nIs reversed : {0}")]
    ActionSequencer_NotDescription,

    [Description("Member Name")]
    ConfigWindow_Actions_MemberName,

    [Description("Rotation is null, please login or switch the job!")]
    ConfigWindow_Condition_RotationNullWarning,

    [Description("You'd better not use it. Because this target isn't the action's target. Try to pick it from action.")]
    ConfigWindow_Condition_TargetWarning,

    [Description("Please separately keybind damage reduction / shield cooldowns in case RS fails at a crucial moment in {0}!")]
    HighEndWarning,

    [Description("Use Forced Enable Condition")]
    ForcedEnableCondition,

    [Description("The conditions of forced to make it true.")]
    ForcedEnableConditionDesc,

    [Description("Use Forced Disable Condition")]
    ForcedDisableCondition,

    [Description("The conditions of forced to make it false.")]
    ForcedDisableConditionDesc,

    [Description("Click to execute the command")]
    ConfigWindow_Helper_RunCommand,

    [Description("Right-click to copy the command")]
    ConfigWindow_Helper_CopyCommand,

    [Description("Modify {0} to {1}")]
    CommandsChangeSettingsValue,

    [Description("Failed to find the config in this rotation, please check it.")]
    CommandsCannotFindConfig,

    [Description("Will use it within {0}s")]
    CommandsInsertAction,

    [Description("Can not find the action, please check the action name.")]
    CommandsInsertActionFailure,

    [Description("Start ")]
    SpecialCommandType_Start,

    [Description("Cancel")]
    SpecialCommandType_Cancel,

    [Description("Heal Area")]
    SpecialCommandType_HealArea,

    [Description("Heal Single")]
    SpecialCommandType_HealSingle,

    [Description("Defense Area")]
    SpecialCommandType_DefenseArea,

    [Description("Defense Single")]
    SpecialCommandType_DefenseSingle,

    [Description("Tank Stance")]
    SpecialCommandType_TankStance,

    [Description("Dispel")]
    SpecialCommandType_Dispel,

    [Description("Positional")]
    SpecialCommandType_Positional,

    [Description("Shirk")]
    SpecialCommandType_Shirk,

    [Description("Raise")]
    SpecialCommandType_Raise,

    [Description("Move Forward")]
    SpecialCommandType_MoveForward,

    [Description("Move Back")]
    SpecialCommandType_MoveBack,

    [Description("Anti Knockback")]
    SpecialCommandType_AntiKnockback,

    [Description("Burst")]
    SpecialCommandType_Burst,

    [Description("End Special")]
    SpecialCommandType_EndSpecial,

    [Description("Speed")]
    SpecialCommandType_Speed,

    [Description("Limit Break")]
    SpecialCommandType_LimitBreak,

    [Description("No Casting")]
    SpecialCommandType_NoCasting,

    [Description("Auto Target ")]
    SpecialCommandType_Smart,

    [Description("Manual Target")]
    SpecialCommandType_Manual,

    [Description("Off")]
    SpecialCommandType_Off,

    [Description("This config is job specific")]
    JobConfigTip,

    [Description("This option is unavailable while using your current job.\nRoles or jobs needed:")]
    NotInJob,

    [Description("Raid Time")]
    TimelineRaidTime,

    [Description("The abilities and custom conditions that your selected job has.")]
    Item_Actions,

    [Description("All rotations that RS has loaded.")]
    Item_Rotations,

    [Description("Some things that have to be lists.")]
    Item_List,

    [Description("Basic settings")]
    Item_Basic,

    [Description("User interface settings.")]
    Item_UI,

    [Description("General actions usage and conditions.")]
    Item_Auto,

    [Description("The targetting for either hostiles or friends.")]
    Item_Target,

    [Description("Your custom time line about the specific duty.")]
    Item_Timeline,

    [Description("Your custom trigger about the specific duty.")]
    Item_Trigger,

    [Description("Some features that shouldn't be included in RS but will help you.")]
    Item_Extra,

    [Description("There are still {0}s left to rate.")]
    Rotation_Rate,

    [Description("The pvp feature can only be used when your config \"{0}\" is on!")]
    CantUseInPvP,

    [Description("This feature at the top level is available only when your config \"{0}\" is on!")]
    CantUseAtTopLevel,

    [Description("This feature for high-end duties is available only for supporters!")]
    CantUseInHighEnd,

    [Description("The territory action feature is a supporters-only feature!")]
    CantUseTerritoryAction,

    [Description("Copied your hash to the clipboard!")]
    CopiedYourHash,

    [Description("Your hash has been added. \nIf you want to remove it, please disable \"{0}\".")]
    AddedYourHash,

    [Description("Your hash has been removed.")]
    RemovedYourHash,

    [Description("You are not a Supporter, some features will NOT be available.")]
    NotSupporterWarning,

    [Description("Thank you for supporting!")]
    ThanksForSupporting,

    [Description("The SequencerAction feature is available only for supporters!")]
    CantUseSequencerAction,

    [Description("This feature for deep dungeons is available only for supporters!")]
    CantUseInDeepDungeons,

    [Description("This feature for Eureka is available only for supporters!")]
    CantUseInEureka,

    [Description("This feature for Bozja is available only for supporters!")]
    CantUseInBozja,

    [Description("The condition boolean is available only for supporters!")]
    CantUseConditionBoolean,

    [Description("The duty {0} can only be farmed by supporters! Please do it next day!")]
    CantFarmThisDuty,

    [Description("This feature is available only for supporters! You can modify it, but it will not take effect.")]
    SupporterOnlyWarning,

    [Description("There are some limits if you turn {0} off!")]
    IWannaBeSaidHelloWarning,

    [Description("Auto heal on timeline")]
    AutoHealOnTimeline,

    [Description("Auto defense on timeline")]
    AutoDefenseOnTimeline,

    [Description("This rotation's dependency is too LOW, which may cause some errors. Please update this rotations!")]
    AssemblyLowVersion,

    [Description("This rotation's dependency is too HIGH, which may cause some errors. Please update the Rotation Solver!")]
    AssemblyHighVersion,

    [Description("If you wanna delete Rotation Solver, you'd better disable \"{0}\" first.")]
    DeleteWarning,

    [Description("There are no items!")]
    ConfigWindow_Condition_NoItemsWarning,
}
