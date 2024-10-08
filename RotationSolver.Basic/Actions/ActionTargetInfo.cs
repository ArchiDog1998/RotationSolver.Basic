﻿using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons.DalamudServices;
using ECommons.ExcelServices;
using ECommons.GameFunctions;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using RotationSolver.Basic.Configuration;
using RotationSolver.Basic.Configuration.Target;

namespace RotationSolver.Basic.Actions;

/// <summary>
/// The target info
/// </summary>
/// <param name="action">the input action.</param>
public struct ActionTargetInfo(IBaseAction action)
{
    /// <summary>
    /// The range of this action.
    /// </summary>
    public readonly float Range => ActionManager.GetActionRange(action.Info.ID);

    /// <summary>
    /// The effect range of this action.
    /// </summary>
    public readonly float EffectRange => (ActionID)action.Info.ID == ActionID.LiturgyOfTheBellPvE ? 20 : action.Action.EffectRange;

    /// <summary>
    /// Is this action single target.
    /// </summary>
    public readonly bool IsSingleTarget => action.Action.CastType == 1;

    /// <summary>
    /// Is this action friendly.
    /// </summary>
    public readonly bool IsTargetFriendly => action.Setting.IsFriendly;

    /// <summary>
    /// Is this action target area.
    /// </summary>

    public readonly bool IsTargetArea => action.Action.TargetArea;

    internal readonly TargetingData TargetingWay
    {
        get
        {
            if (action.Config.UseCustomTargetingData)
            {
                foreach (var item in Service.Config.TargetingWays)
                {
                    if (item.TargetName != action.Config.TargetingDataName) continue;
                    return item;
                }
            }
            return DataCenter.TargetingWay;
        }
    }

    private static bool NoAOE
    {
        get
        {
            if (!Service.Config.UseAoeAction) return true;

            if (DataCenter.IsManual)
            {
                if (!Service.Config.UseAoeWhenManual) return true;
            }

            return Service.Config.ChooseAttackMark
                && !Service.Config.CanAttackMarkAoe
                && MarkingHelper.HaveAttackChara;
        }
    }

    #region Target Finder.
    private readonly IEnumerable<IBattleChara> GetCanTargets(bool skipStatusProvideCheck, TargetType type)
    {
        var items = TargetFilter.GetObjectInRadius(DataCenter.AllTargets, Range);
        var objs = new List<IBattleChara>(items.Count());

        foreach (var obj in items)
        {
            if (type == TargetType.Heal && obj.GetHealthRatio() == 1) continue;

            if (!GeneralCheck(obj, skipStatusProvideCheck)) continue;
            if (action.Config.CantAttack(obj)) continue;

            objs.Add(obj);
        }

        var targetables = objs.Where(CanUseTo);

        if (!targetables.Any() && action.Info.IsDutyAction)
        {
            targetables = [Player.Object];
        }

        var isAuto = !DataCenter.IsManual || IsTargetFriendly;
        return targetables.Where(b => isAuto || b.EntityId == Svc.Targets.Target?.EntityId || b.EntityId == Player.Object?.EntityId)
            .Where(InViewTarget).Where(action.Setting.CanTarget);
    }

    private readonly List<IBattleChara> GetCanAffects(bool skipStatusProvideCheck, TargetType type)
    {
        if (EffectRange == 0) return [];

        var items = TargetFilter.GetObjectInRadius(action.Setting.IsFriendly
            ? DataCenter.PartyMembers
            : DataCenter.AllHostileTargets,
            Range + EffectRange);

        if (type == TargetType.Heal)
        {
            items = items.Where(i => i.GetHealthRatio() < 1);
        }

        var objs = new List<IBattleChara>(items.Count());

        foreach (var obj in items)
        {
            if (!GeneralCheck(obj, skipStatusProvideCheck)) continue;
            objs.Add(obj);
        }

        return objs;
    }

    private static bool InViewTarget(IBattleChara IGameObject)
    {
        if (Service.Config.OnlyAttackInView)
        {
            if (!Svc.GameGui.WorldToScreen(IGameObject.Position, out _)) return false;
        }
        if (Service.Config.OnlyAttackInVisionCone)
        {
            Vector3 dir = IGameObject.Position - Player.Object.Position;
            Vector2 dirVec = new(dir.Z, dir.X);
            double angle = Player.Object.GetFaceVector().AngleTo(dirVec);
            if (angle > Math.PI * Service.Config.AngleOfVisionCone / 360)
            {
                return false;
            }
        }
        return true;
    }

    private readonly unsafe bool CanUseTo(IGameObject tar)
    {
        if (tar == null || !Player.Available) return false;
        if (tar.EntityId == 0xE000_0000) return false;
        if (tar.EntityId == 0) return false;

        var tarAddress = tar.Struct();
        if (tarAddress == null) return false;

        if ((ActionID)action.Info.ID != ActionID.AethericMimicryPvE
            && !ActionManager.CanUseActionOnTarget(action.Info.ID, tarAddress)) return false;

        return tar.CanSee();
    }

    private readonly bool GeneralCheck(IBattleChara gameObject, bool skipStatusProvideCheck)
    {
        if (!gameObject.IsTargetable) return false;

        if (gameObject.IsAlliance())
        {
            if (Service.Config.OnlyPartiesAreFriendly && !gameObject.IsParty())
            {
                return false;
            }
            
            if (gameObject is IBattleNpc npc)
            {
                switch (npc.BattleNpcKind)
                {
                    case Dalamud.Game.ClientState.Objects.Enums.BattleNpcSubKind.Pet
                    when Service.Config.DontTargetPetsAsFriendly:
                    case Dalamud.Game.ClientState.Objects.Enums.BattleNpcSubKind.Chocobo
                    when Service.Config.DontTargetChocoboAsFriendly:
                        return false;
                }
            }
        }

        if (gameObject.IsEnemy())
        {
            //Can't attack.
            if (!gameObject.IsAttackable()) return false;
        }

        return CheckStatus(gameObject, skipStatusProvideCheck) 
            && CheckTimeToKill(gameObject);
    }

    private readonly bool CheckStatus(IGameObject gameObject, bool skipStatusProvideCheck)
    {
        if (!action.Config.ShouldCheckStatus) return true;

        if (action.Setting.TargetStatusNeed != null)
        {
            if (gameObject.WillStatusEndGCD(0, 0,
                action.Setting.StatusFromSelf, action.Setting.TargetStatusNeed)) return false;
        }

        if (action.Info.TargetStatusProvide.Length > 0 && !skipStatusProvideCheck)
        {
            if (!gameObject.WillStatusEndGCD(action.Config.StatusGcdCount, 0,
                action.Setting.StatusFromSelf, action.Info.TargetStatusProvide)) return false;
        }

        if (action.Setting.TargetStatusPenalty != null)
        {
            if (!gameObject.WillStatusEndGCD(action.Config.StatusGcdCount, 0,
                action.Setting.StatusFromSelf, action.Setting.TargetStatusPenalty)) return false;
        }
        return true;
    }

    private readonly bool CheckTimeToKill(IGameObject IGameObject)
    {
        if (IGameObject is not IBattleChara b) return false;
        var time = b.GetTimeToKill();
        return float.IsNaN(time) || time >= action.Config.TimeToKill;
    }

    #endregion

    /// <summary>
    /// Take a little long time..
    /// </summary>
    /// <returns></returns>
    internal readonly TargetResult? FindTarget(bool skipAoeCheck, bool skipStatusProvideCheck)
    {
        var range = Range;
        var player = Player.Object;

        if (range == 0 && EffectRange == 0)
        {
            return new(player, [], player.Position);
        }
        var type = action.Setting.TargetType;

        var canTargets = GetCanTargets(skipStatusProvideCheck, type);
        var canAffects = GetCanAffects(skipStatusProvideCheck, type);

        if (IsTargetArea)
        {
            return FindTargetArea(canTargets, canAffects, range, player);
        }

#if DEBUG
        //skipAoeCheck = true;
#endif
        var targets = GetMostCanTargetObjects(canTargets, canAffects,
            skipAoeCheck ? 1 : action.Config.AoeCount);
        var target = FindTargetByType(targets, type, action.Config.AutoHealRatio, action.Setting.SpecialType, TargetingWay);
        if (target == null) return null;

        return new(target, [.. GetAffects(target, canAffects)], target.Position);
    }

    private readonly TargetResult? FindTargetArea(IEnumerable<IBattleChara> canTargets, IEnumerable<IBattleChara> canAffects,
        float range, IPlayerCharacter player)
    {
        if (range == 0)
        {
            return new(player, [.. GetAffects(player.Position, canAffects)], player.Position);
        }
        else if (action.Setting.TargetType is TargetType.Move)
        {
            return FindTargetAreaMove(range);
        }
        else if (action.Setting.IsFriendly)
        {
            if (!Service.Config.UseGroundBeneficialAbility) return null;
            if (!Service.Config.UseGroundBeneficialAbilityWhenMoving && DataCenter.IsMoving) return null;

            return FindTargetAreaFriend(range, canAffects, player);
        }
        else
        {
            return FindTargetAreaHostile(canTargets, canAffects, action.Config.AoeCount);
        }
    }

    private readonly TargetResult? FindTargetAreaHostile(IEnumerable<IBattleChara> canTargets, IEnumerable<IBattleChara> canAffects, int aoeCount)
    {
        var target = GetMostCanTargetObjects(canTargets, canAffects, aoeCount)
            .OrderByDescending(ObjectHelper.GetHealthRatio).FirstOrDefault();
        if (target == null) return null;
        return new(target, [..GetAffects(target, canAffects)], target.Position);
    }

    private readonly TargetResult? FindTargetAreaMove(float range)
    {
        if (Service.Config.MoveAreaActionFarthest)
        {
            Vector3 pPosition = Player.Object.Position;
            if (Service.Config.MoveTowardsScreenCenter) unsafe
                {
                    var camera = CameraManager.Instance()->CurrentCamera;
                    var tar = camera->LookAtVector - camera->Object.Position;
                    tar.Y = 0;
                    var length = ((Vector3)tar).Length();
                    if (length == 0) return null;
                    tar = tar / length * range;
                    return new(Player.Object, [], new Vector3(pPosition.X + tar.X,
                        pPosition.Y, pPosition.Z + tar.Z));
                }
            else
            {
                float rotation = Player.Object.Rotation;
                return new(Player.Object, [], new Vector3(pPosition.X + (float)Math.Sin(rotation) * range,
                    pPosition.Y, pPosition.Z + (float)Math.Cos(rotation) * range));
            }
        }
        else
        {
            var availableCharas = DataCenter.AllTargets.Where(b => b.EntityId != Player.Object.EntityId);
            var target = FindTargetByType(TargetFilter.GetObjectInRadius(availableCharas, range),
                TargetType.Move, action.Config.AutoHealRatio, action.Setting.SpecialType, TargetingWay);
            if (target == null) return null;
            return new(target, [], target.Position);
        }
    }

    private readonly TargetResult? FindTargetAreaFriend(float range, IEnumerable<IBattleChara> canAffects, IPlayerCharacter player)
    {
        var strategy = Service.Config.BeneficialAreaStrategy;
        switch (strategy)
        {
            case BeneficialAreaStrategy.OnLocations: // Find from list
            case BeneficialAreaStrategy.OnlyOnLocations: // Only the list
                var pts  = OtherConfiguration.TerritoryConfig.BeneficialPositions;

                if (pts.Count == 0)
                {
                    if (DataCenter.TerritoryContentType == TerritoryContentType.Trials ||
                        DataCenter.TerritoryContentType == TerritoryContentType.Raids
                        && DataCenter.AllianceMembers.Count(p => p is IPlayerCharacter) == 8)
                    {
                        pts = [.. pts, Vector3.Zero, new(100, 0, 100)];
                    }
                }

                if (pts.Count > 0)
                {
                    var closest = pts.MinBy(p => Vector3.Distance(player.Position, p));
                    var rotation = new Random().NextDouble() * Math.Tau;
                    var radius = new Random().NextDouble() * 1;
                    if (closest == null) break;

                    closest.X += (float)(Math.Sin(rotation) * radius);
                    closest.Z += (float)(Math.Cos(rotation) * radius);
                    if (Vector3.Distance(player.Position, closest) < player.HitboxRadius + EffectRange)
                    {
                        return new(player, [.. GetAffects(closest, canAffects)], closest);
                    }
                }

                if (strategy == BeneficialAreaStrategy.OnlyOnLocations) return null;
                break;

            case BeneficialAreaStrategy.OnTarget: // Target
                if (Svc.Targets.Target != null && Svc.Targets.Target.DistanceToPlayer() < range)
                {
                    var target = Svc.Targets.Target as IBattleChara;
                    return new(target, [.. GetAffects(target?.Position, canAffects)], target?.Position);
                }
                break;
        }

        if (Svc.Targets.Target is IBattleChara b && b.DistanceToPlayer() < range &&
            b.IsBossFromIcon() && b.HasPositional() && b.HitboxRadius <= 8)
        {
            return new(b, [.. GetAffects(b.Position, canAffects)], b.Position);
        }
        else
        {
            var effectRange = EffectRange;
            var attackT = FindTargetByType(DataCenter.AllianceMembers.GetObjectInRadius(range + effectRange),
                TargetType.BeAttacked, action.Config.AutoHealRatio, action.Setting.SpecialType, TargetingWay);

            if (attackT == null)
            {
                return new(player, [.. GetAffects(player.Position, canAffects)], player.Position);
            }
            else
            {
                var disToTankRound = Vector3.Distance(player.Position, attackT.Position) + attackT.HitboxRadius;

                if (disToTankRound < effectRange
                    || disToTankRound > 2 * effectRange - player.HitboxRadius)
                {
                    return new(player, [.. GetAffects(player.Position, canAffects)], player.Position);
                }
                else
                {
                    Vector3 directionToTank = attackT.Position - player.Position;
                    var MoveDirection = directionToTank / directionToTank.Length() * Math.Max(0, disToTankRound - effectRange);
                    return new(player, [.. GetAffects(player.Position, canAffects)], player.Position + MoveDirection);
                }
            }
        }
    }

    private readonly IEnumerable<IBattleChara> GetAffects(Vector3? point, IEnumerable<IBattleChara> canAffects)
    {
        if (point == null) yield break;
        foreach (var t in canAffects)
        {
            if (Vector3.Distance(point.Value, t.Position) - t.HitboxRadius <= EffectRange)
            {
                yield return t;
            }
        }
    }

    private readonly IEnumerable<IBattleChara> GetAffects(IBattleChara tar, IEnumerable<IBattleChara> canAffects)
    {
        foreach (var t in canAffects)
        {
            if (CanGetTarget(tar, t))
            {
                yield return t;
            }
        }
    }

    #region Get Most Target
    private readonly IEnumerable<IBattleChara> GetMostCanTargetObjects(IEnumerable<IBattleChara> canTargets, IEnumerable<IBattleChara> canAffects, int aoeCount)
    {
        var condition = action.Config.IsTopPriority;

        if (IsSingleTarget || EffectRange <= 0)
        {
            var priorityCanTargets = canTargets.Where(condition).OfType<IBattleChara>();
            return priorityCanTargets.Any() ? priorityCanTargets : canTargets;
        }

        if (!action.Setting.IsFriendly && NoAOE) return [];

        var priorityCanAffects = canAffects.Where(condition).OfType<IBattleChara>();
        var hasPriorityAffects = priorityCanAffects.Any();
        var priorityAffectsCount = 0;

        List<IBattleChara> objectMax = new(canTargets.Count());

        foreach (var t in canTargets)
        {
            int count = CanGetTargetCount(t, canAffects);

            if (hasPriorityAffects)
            {
                if (count < aoeCount) continue;

                count = CanGetTargetCount(t, priorityCanAffects);

                if (count == priorityAffectsCount)
                {
                    objectMax.Add(t);
                }
                else if(count > priorityAffectsCount)
                {
                    aoeCount = count;
                    objectMax.Clear();
                    objectMax.Add(t);
                }
            }
            else
            {
                if (count == aoeCount)
                {
                    objectMax.Add(t);
                }
                else if (count > aoeCount)
                {
                    aoeCount = count;
                    objectMax.Clear();
                    objectMax.Add(t);
                }
            }
        }
        return objectMax;
    }

    private readonly int CanGetTargetCount(IGameObject target, IEnumerable<IGameObject> canAffects)
    {
        int count = 0;
        foreach (var t in canAffects)
        {
            if (target != t && !CanGetTarget(target, t)) continue;

            if (Service.Config.NoNewHostiles
                && t.TargetObject == null)
            {
                return 0;
            }

            if (action.Config.CantAttack(t))
            {
                return 0;
            }

            count++;
        }

        return count;
    }

    const double _alpha = Math.PI / 3;
    private readonly bool CanGetTarget(IGameObject target, IGameObject subTarget)
    {
        if (target == null) return false;

        var pPos = Player.Object.Position;
        Vector3 dir = target.Position - pPos;
        Vector3 tdir = subTarget.Position - pPos;

        switch (action.Action.CastType)
        {
            case 2: // Circle
                return Vector3.Distance(target.Position, subTarget.Position) - subTarget.HitboxRadius <= EffectRange;

            case 3: // Sector
                if (subTarget.DistanceToPlayer() > EffectRange) return false;
                tdir += dir / dir.Length() * target.HitboxRadius / (float)Math.Sin(_alpha);
                return Vector3.Dot(dir, tdir) / (dir.Length() * tdir.Length()) >= Math.Cos(_alpha);

            case 4: //Line
                if (subTarget.DistanceToPlayer() > EffectRange) return false;

                return Vector3.Cross(dir, tdir).Length() / dir.Length() <= 2 + target.HitboxRadius
                    && Vector3.Dot(dir, tdir) >= 0;

            case 10: //Donut
                var dis = Vector3.Distance(target.Position, subTarget.Position) - subTarget.HitboxRadius;
                return dis <= EffectRange && dis >= 8;
        }

        Svc.Log.Debug(action.Action.Name.RawString + "'s CastType is not valid! The value is " + action.Action.CastType.ToString());
        return false;
    }
    #endregion

    #region TargetFind
    private static IBattleChara? FindTargetByType(IEnumerable<IBattleChara> IGameObjects, TargetType type, float healRatio, SpecialActionType actionType, TargetingData targetingData)
    {
        if (type == TargetType.Self && IGameObjects.Contains(Player.Object)) return Player.Object;

        switch (actionType)
        {
            case SpecialActionType.MeleeRange when Service.CountDownTime == 0:
                IGameObjects = IGameObjects.Where(t => t.DistanceToPlayer() >= 3 + Service.Config.MeleeRangeOffset);
                break;

            case SpecialActionType.MovingForward:
                if (DataCenter.MergedStatus.HasFlag(AutoStatus.MoveForward) || Service.CountDownTime > 0)
                {
                    type = TargetType.Move;
                }
                if (type != TargetType.Move)
                {
                    IGameObjects = IGameObjects.Where(t => t.DistanceToPlayer() < 1);
                }
                break;
        }

        switch (type) // Filter the objects.
        {
            case TargetType.Death:
                IGameObjects = IGameObjects.Where(ObjectHelper.IsDeathToRaise);
                break;

            case TargetType.Move:
                break;

            default:
                IGameObjects = IGameObjects.Where(ObjectHelper.IsAlive);
                break;
        }

        return type switch //Find the object.
        {
            TargetType.ProvokeOrOthers => FindProvokeTarget() ?? FindHostile(),
            TargetType.Provoke => FindProvokeTarget(),
            TargetType.Dispel => FindDispelTarget(),
            TargetType.Death => FindDeathPeople(),
            TargetType.Move => FindTargetForMoving(),
            TargetType.Heal => FindHealTarget(healRatio),
            TargetType.BeAttacked => FindBeAttackedTarget(),
            TargetType.Interrupt => FindInterruptTarget(),
            TargetType.Tank => FindTankTarget(),
            TargetType.Melee => RandomMeleeTarget(IGameObjects),
            TargetType.Range => RandomRangeTarget(IGameObjects),
            TargetType.Magical => RandomMagicalTarget(IGameObjects),
            TargetType.Physical => RandomPhysicalTarget(IGameObjects),
            _ => FindHostile(),
        };

        IBattleChara? FindProvokeTarget()
        {
            if (IGameObjects.Any(o => o.EntityId == DataCenter.ProvokeTarget?.EntityId))
                return DataCenter.ProvokeTarget;
            return null;
        }

        IBattleChara? FindDeathPeople()
        {
            if (IGameObjects.Any(o => o.EntityId == DataCenter.DeathTarget?.EntityId))
                return DataCenter.DeathTarget;
            return null;
        }

        IBattleChara? FindTargetForMoving()
        {
            const float DISTANCE_TO_MOVE = 3;

            if (Service.Config.MoveTowardsScreenCenter)
            {
                return FindMoveTargetScreenCenter();
            }
            else
            {
                return FindMoveTargetFaceDirection();
            }

            IBattleChara? FindMoveTargetScreenCenter()
            {
                var pPosition = Player.Object.Position;
                if (!Svc.GameGui.WorldToScreen(pPosition, out var playerScrPos)) return null;

                var tars = IGameObjects.Where(t =>
                {
                    if (t.DistanceToPlayer() < DISTANCE_TO_MOVE) return false;

                    if (!Svc.GameGui.WorldToScreen(t.Position, out var scrPos)) return false;

                    var dir = scrPos - playerScrPos;

                    if (dir.Y > 0) return false;

                    return Math.Abs(dir.X / dir.Y) <= Math.Tan(Math.PI * Service.Config.MoveTargetAngle / 360);
                }).OrderByDescending(ObjectHelper.DistanceToPlayer);

                return tars.FirstOrDefault();
            }

            IBattleChara? FindMoveTargetFaceDirection()
            {
                Vector3 pPosition = Player.Object.Position;
                Vector2 faceVec = Player.Object.GetFaceVector();

                var tars = IGameObjects.Where(t =>
                {
                    if (t.DistanceToPlayer() < DISTANCE_TO_MOVE) return false;

                    Vector3 dir = t.Position - pPosition;
                    Vector2 dirVec = new(dir.Z, dir.X);
                    double angle = faceVec.AngleTo(dirVec);
                    return angle <= Math.PI * Service.Config.MoveTargetAngle / 360;
                }).OrderByDescending(ObjectHelper.DistanceToPlayer);

                return tars.FirstOrDefault();
            }
        }

        IBattleChara? FindHealTarget(float healRatio)
        {
            if (!IGameObjects.Any()) return null;

            if (IBaseAction.AutoHealCheck)
            {
                IGameObjects = IGameObjects.Where(o => o.GetHealthRatio() < healRatio);
            }

            var partyMembers = IGameObjects.Where(ObjectHelper.IsParty);

            return GeneralHealTarget(partyMembers)
                ?? GeneralHealTarget(IGameObjects)
                ?? partyMembers.FirstOrDefault(t => t.HasStatus(false, StatusHelper.TankStanceStatus))
                ?? partyMembers.FirstOrDefault()
                ?? IGameObjects.FirstOrDefault(t => t.HasStatus(false, StatusHelper.TankStanceStatus))
                ?? IGameObjects.FirstOrDefault();

            static IBattleChara? GeneralHealTarget(IEnumerable<IBattleChara> objs)
            {
                objs = objs.Where(StatusHelper.NeedHealing).OrderBy(ObjectHelper.GetHealthRatio);

                var healerTars = objs.GetJobCategory(JobRole.Healer);
                var tankTars = objs.GetJobCategory(JobRole.Tank);

                var healerTar = healerTars.FirstOrDefault();
                if (healerTar != null && healerTar.GetHealthRatio() < Service.Config.HealthHealerRatio)
                    return healerTar;

                var tankTar = tankTars.FirstOrDefault();
                if (tankTar != null && tankTar.GetHealthRatio() < Service.Config.HealthTankRatio)
                    return tankTar;

                var tar = objs.FirstOrDefault();
                if (tar?.GetHealthRatio() < 1) return tar;

                return null;
            }
        }

        IBattleChara? FindInterruptTarget()
        {
            if (IGameObjects.Any(o => o.EntityId == DataCenter.InterruptTarget?.EntityId))
                return DataCenter.InterruptTarget;
            return null;
        }

        IBattleChara? FindHostile()
        {
            if (IGameObjects == null || !IGameObjects.Any()) return null;

            if (Service.Config.FilterStopMark)
            {
                var cs = MarkingHelper.FilterStopBattles(IGameObjects);
                if (cs?.Any() ?? false) IGameObjects = cs;
            }

            if (DataCenter.TreasureCharas.Length > 0)
            {
                var b = IGameObjects.FirstOrDefault(b => b.EntityId == DataCenter.TreasureCharas[0]);
                if (b != null) return b;
                IGameObjects = IGameObjects.Where(b => !DataCenter.TreasureCharas.Contains(b.EntityId));
            }

            return targetingData.FindTarget(IGameObjects);
        }

        IBattleChara? FindBeAttackedTarget()
        {
            if (!IGameObjects.Any()) return null;
            var attachedT = IGameObjects.Where(ObjectHelper.IsTargetOnSelf);

            if (!DataCenter.AutoStatus.HasFlag(AutoStatus.DefenseSingle))
            {
                if (!attachedT.Any())
                {
                    attachedT = IGameObjects.Where(tank => tank.HasStatus(false, StatusHelper.TankStanceStatus));
                }

                if (!attachedT.Any())
                {
                    attachedT = IGameObjects.GetJobCategory(JobRole.Tank);
                }

                if (!attachedT.Any())
                {
                    attachedT = IGameObjects;
                }
            }

            return attachedT.OrderBy(ObjectHelper.GetHealthRatio).FirstOrDefault();
        }

        IBattleChara? FindDispelTarget()
        {
            if (IGameObjects.Any(o => o.EntityId == DataCenter.DispelTarget?.EntityId))
                return DataCenter.DispelTarget;
            return IGameObjects.FirstOrDefault(o => o is IBattleChara b && b.StatusList.Any(StatusHelper.CanDispel));
        }

        IBattleChara? FindTankTarget()
        {
            return RandomPickByJobs(IGameObjects, JobRole.Tank);
        }
    }

    internal static IBattleChara? RandomPhysicalTarget(IEnumerable<IBattleChara> tars)
    {
        return RandomPickByJobs(tars, Job.WAR, Job.GNB, Job.MNK, Job.SAM, Job.DRG, Job.MCH, Job.DNC)
            ?? RandomPickByJobs(tars, Job.PLD, Job.DRK, Job.NIN, Job.BRD, Job.RDM)
            ?? RandomObject(tars);
    }

    internal static IBattleChara? RandomMagicalTarget(IEnumerable<IBattleChara> tars)
    {
        return RandomPickByJobs(tars, Job.SCH, Job.AST, Job.SGE, Job.BLM, Job.SMN)
            ?? RandomPickByJobs(tars, Job.PLD, Job.DRK, Job.NIN, Job.BRD, Job.RDM)
            ?? RandomObject(tars);
    }

    internal static IBattleChara? RandomRangeTarget(IEnumerable<IBattleChara> tars)
    {
        return RandomPickByJobs(tars, JobRole.RangedMagical, JobRole.RangedPhysical, JobRole.Melee)
            ?? RandomPickByJobs(tars, JobRole.Tank, JobRole.Healer)
            ?? RandomObject(tars);
    }

    internal static IBattleChara? RandomMeleeTarget(IEnumerable<IBattleChara> tars)
    {
        return RandomPickByJobs(tars, JobRole.Melee, JobRole.RangedMagical, JobRole.RangedPhysical)
            ?? RandomPickByJobs(tars, JobRole.Tank, JobRole.Healer)
            ?? RandomObject(tars);
    }

    private static IBattleChara? RandomPickByJobs(IEnumerable<IBattleChara> tars, params JobRole[] roles)
    {
        foreach (var role in roles)
        {
            var tar = RandomPickByJobs(tars, role.ToJobs());
            if (tar != null) return tar;
        }
        return null;
    }

    private static IBattleChara? RandomPickByJobs(IEnumerable<IBattleChara> tars, params Job[] jobs)
    {
        var targets = tars.Where(t => t.IsJobs(jobs));
        if (targets.Any()) return RandomObject(targets);

        return null;
    }

    private static IBattleChara? RandomObject(IEnumerable<IBattleChara> objs)
    {
        return objs.FirstOrDefault();
        //Random ran = new(DateTime.Now.Millisecond);
        //var count = objs.Count();
        //if (count == 0) return null;
        //return objs.ElementAt(ran.Next(count));
    }

    #endregion
}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public enum TargetType : byte
{
    None,
    Interrupt,
    Provoke,
    ProvokeOrOthers,
    Death,
    Dispel,
    Move,
    BeAttacked,
    Heal,
    Tank,
    Melee,
    Range,
    Physical,
    Magical,
    Self,
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// The target result
/// </summary>
/// <param name="Target">the target.</param>
/// <param name="AffectedTargets">the targets that be affected by this action.</param>
/// <param name="Position">the position to use this action.</param>
public readonly record struct TargetResult(IBattleChara? Target, IBattleChara[] AffectedTargets, Vector3? Position);