using System;
using Rondo.Core.Extras;
using Rondo.Core.Lib;
using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using Rondo.Unity.Utils;
using TenSecCastle.Model;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace TenSecCastle.Game {
    public static unsafe class GameLogic {
        public static (GameModel, L<Cmd<Msg>>) UpdateTick(GameModel model, float dt) {
            model.Timeout -= dt;
            model = UpdateUnits(model, dt);

            if (model.Timeout <= 0) {
                model = AI(model);
                model = SpawnUnit(model);
                model.Timeout += model.Interval;
            }

            return (model, new());
        }

        private static GameModel UpdateUnits(GameModel model, float dt) {
            static Maybe<Unit> UpdateUnit(Unit unit, GameModel* model, float* dt) {
                switch (unit.State) {
                    case UnitState.Idle: {
                        if (!InBounds(unit.Cell + unit.Direction, model->FieldSize, model->MoveAxis)) {
                            Debug.Log("hit castle");
                            unit.State = UnitState.Dieing;
                            unit.StateTime = 0;
                            unit.StateProgress = 0;
                        }
                        else if (FindTarget(*model, unit).Test(out var target)) {
                            unit.State = UnitState.Attacking;
                            unit.StateTime = 0;
                            unit.StateProgress = 0;
                            unit.TargetUnitId = target.Id;
                        }
                        else if (FindCellToMove(*model, unit).Test(out var cell)) {
                            unit.State = UnitState.Moving;
                            unit.StateTime = 0;
                            unit.StateProgress = 0;
                            unit.Direction = cell - unit.Cell;
                            unit.Cell = cell;
                        }
                        break;
                    }
                    case UnitState.Attacking: {
                        unit.StateTime += *dt;
                        var len = 1f / unit.AttackSpeed;
                        unit.StateProgress = unit.StateTime / len;
                        if (unit.StateTime >= len) {
                            unit.State = UnitState.Idle;
                            unit.StateTime = 0;
                            unit.StateProgress = 0;
                        }
                        break;
                    }
                    case UnitState.Moving: {
                        var len = 1f / unit.MoveSpeed;
                        unit.StateTime += *dt;
                        unit.StateProgress = unit.StateTime / len;
                        if (unit.StateTime >= len) {
                            unit.State = UnitState.Idle;
                            unit.StateTime = 0;
                            unit.StateProgress = 0;
                        }
                        break;
                    }
                    case UnitState.Dieing:
                        unit.StateTime += *dt;
                        if (unit.StateTime > 2f) { //Death animation length
                            return Maybe<Unit>.Nothing;
                        }
                        break;
                }
                return Maybe<Unit>.Just(unit);
            }

            static bool WithId(Unit unit, ulong* id) {
                return unit.Id == *id;
            }

            model.Units = model.Units.FilterMap(Cf.New<Unit, GameModel, float, Maybe<Unit>>(&UpdateUnit, model, dt));

            for (var i = 0; i < model.Units.Length(); i++) {
                if (model.Units.At(i).Test(out var unit)) {
                    if ((unit.State == UnitState.Attacking) && (unit.StateTime == 0)) {
                        if (
                            model.Units
                                    .FindIndex(Cf.New<Unit, ulong, bool>(&WithId, unit.TargetUnitId))
                                    .Test(out var targetIndex)
                            && model.Units.At(targetIndex).Test(out var target)
                        ) {
                            target.HitPoints -= CalculateDamage(unit, target);
                            if (target.HitPoints <= 0) {
                                target.HitPoints = 0;
                                target.State = UnitState.Dieing;
                                target.StateTime = 0;
                            }
                            model.Units = model.Units.Replace(targetIndex, target);
                        }
                    }
                }
            }
            return model;
        }

        private static bool InBounds(int2 cell, int2 bounds, int2 axis) {
            if (axis.x == 1) {
                return (cell.x >= 0) && (cell.x < bounds.x);
            }
            if (axis.y == 1) {
                return (cell.y >= 0) && (cell.y < bounds.y);
            }
            Assert.Fail("Invalid move axis");
            return false;
        }

        private static bool InBounds(int2 cell, int2 bounds) {
            return (cell.x >= 0) && (cell.x < bounds.x) && (cell.y >= 0) && (cell.y < bounds.y);
        }

        private static Maybe<Unit> UnitAt(GameModel model, int2 cell) {
            static bool CellEq(Unit unit, int2* cell) {
                return unit.Cell.Equals(*cell);
            }

            return model.Units.First(Cf.New<Unit, int2, bool>(&CellEq, cell));
        }

        private static readonly int2[] _meleeCheckDelta = {
                new(-1, 0), new(0, -1), new(0, 1), new(1, 0),
                new(-1, -1), new(-1, 1), new(1, -1), new(1, 1),
        };

        private static readonly int2[] _rangedCheckDelta = {
                new(-1, 0), new(0, -1), new(0, 1), new(1, 0),
                new(-1, -1), new(-1, 1), new(1, -1), new(1, 1),

                new(-2, 0), new(2, 0), new(0, -2), new(0, 2),
                new(-2, -1), new(-2, 1), new(2, -1), new(2, 1),
                new(-1, -2), new(1, -2), new(-1, 2), new(1, 2),
                new(2, -2), new(-2, -2), new(2, 2), new(-2, 2),
        };

        private static Maybe<Unit> FindTarget(GameModel model, Unit unit) {
            int2[] deltas = null;
            switch (unit.AttackRange) {
                case AttackRange.Melee:
                    deltas = _meleeCheckDelta;
                    break;
                case AttackRange.Ranged:
                    deltas = _rangedCheckDelta;
                    break;
            }
            if (deltas != null) {
                for (var i = 0; i < deltas.Length; i++) {
                    var c = unit.Cell + deltas[i];
                    if (UnitAt(model, c).Test(out var target) && (target.Owner != unit.Owner)) {
                        return Maybe<Unit>.Just(unit);
                    }
                }
            }

            return Maybe<Unit>.Nothing; //TODO: find unit to hit
        }

        private static readonly int[] _moveCheckDelta = { 0, -1, 1 };

        private static Maybe<int2> FindCellToMove(GameModel model, Unit unit) {
            var d = unit.Direction * model.MoveAxis;
            var oc = unit.Cell + d;
            var side = new int2(model.MoveAxis.y, model.MoveAxis.x);
            for (var i = 0; i < _moveCheckDelta.Length; i++) {
                var c = oc + side * _moveCheckDelta[i];
                if (InBounds(c, model.FieldSize) && !UnitAt(model, c).Test(out _)) {
                    return Maybe<int2>.Just(c);
                }
            }

            return Maybe<int2>.Nothing;
        }

        private static float CalculateDamage(Unit attacker, Unit victim) {
            var attack = 0f;
            var defense = 0f;
            switch (attacker.AttackType) {
                case AttackType.Physical:
                    attack = attacker.PhysicsAttack;
                    defense = victim.PhysicsDefense;
                    break;
                case AttackType.Magical:
                    attack = attacker.MagicAttack;
                    defense = victim.MagicDefense;
                    break;
            }
            return attack * (1 - (float)Math.Log(defense + 1, 40));
        }

        private static GameModel AI(GameModel model) {
            //TODO:
            return model;
        }

        private static GameModel SpawnUnit(GameModel model) {
            var players = model.Players.Enumerator;
            while (players.MoveNext()) {
                var player = players.Current;
                if (
                    player.SpawnPoints.At(player.CurrentSpawnPoint).Test(out var spawnPoint)
                    && !UnitAt(model, spawnPoint).Test(out _)
                ) {
                    var unit = CreateUnit(model.BasicUnit, player.Slots);
                    unit.Id = ++model.LastUnitId;
                    unit.Cell = spawnPoint;
                    unit.Direction = player.SpawnDirection;
                    unit.Owner = player.Id;

                    model.Units += unit;
                }
            }

            static Player Reset(Player player, GameModel* model) {
                player.Slots = ShuffleSlots(*model);
                player.Coins += model->BaseIncome;
                return player;
            }

            model.Players = model.Players.Map(Cf.New<Player, GameModel, Player>(&Reset, model));

            return model;
        }

        public static L<Slot> ShuffleSlots(GameModel model) {
            static int RandComp(Item a, Item b, Random* r) {
                return r->NextInt(2) == 0 ? -1 : 1;
            }

            static bool WithSlot(Item a, SlotKind* slot) {
                return a.SlotKind == *slot;
            }

            static Slot ToSlot(Maybe<Item> maybeItem) {
                if (!maybeItem.Test(out var item)) {
                    Assert.Fail("There are not enough items available");
                }
                return new Slot { Item = item };
            }

            var items = model.Items.SortWith(
                Cf.New<Item, Item, Random, int>(&RandComp, new Random((uint)DateTime.Now.ToFileTime()))
            );

            return new L<Maybe<Item>>(
                items.First(Cf.New<Item, SlotKind, bool>(&WithSlot, SlotKind.Weapon)),
                items.First(Cf.New<Item, SlotKind, bool>(&WithSlot, SlotKind.Armor)),
                items.First(Cf.New<Item, SlotKind, bool>(&WithSlot, SlotKind.Jewelry))
            ).Map(&ToSlot);
        }

        private static Unit CreateUnit(Unit baseUnit, L<Slot> currentSlots) {
            var slots = currentSlots.Enumerator;
            baseUnit.HitPoints = baseUnit.MaxHitPoints;

            while (slots.MoveNext()) {
                var slot = slots.Current;
                var attributes = slot.Item.Attributes.Enumerator;
                while (attributes.MoveNext()) {
                    var attr = attributes.Current;
                    switch (attr.Kind) {
                        case AttributeKind.Weapon:
                            switch (attr.AttackType) {
                                case AttackType.Physical:
                                    baseUnit.PhysicsAttack += attr.Value;
                                    break;
                                case AttackType.Magical:
                                    baseUnit.MagicAttack += attr.Value;
                                    break;
                            }
                            baseUnit.AttackType = attr.AttackType;
                            baseUnit.AttackRange = attr.AttackRange;
                            break;
                        case AttributeKind.Attack:
                            switch (attr.AttackType) {
                                case AttackType.Physical:
                                    baseUnit.PhysicsAttack += attr.Value;
                                    break;
                                case AttackType.Magical:
                                    baseUnit.MagicAttack += attr.Value;
                                    break;
                            }
                            break;
                        case AttributeKind.Defense:
                            switch (attr.AttackType) {
                                case AttackType.Physical:
                                    baseUnit.PhysicsDefense += attr.Value;
                                    break;
                                case AttackType.Magical:
                                    baseUnit.MagicDefense += attr.Value;
                                    break;
                            }
                            break;
                        case AttributeKind.HitPoints:
                            baseUnit.HitPoints += attr.Value;
                            break;
                        case AttributeKind.HitPointRegen:
                            baseUnit.HpRegen += attr.Value;
                            break;
                        case AttributeKind.Income:
                            baseUnit.KillIncome += (int)attr.Value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            return baseUnit;
        }
    }
}