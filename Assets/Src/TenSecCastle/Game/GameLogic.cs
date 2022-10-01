using System;
using Rondo.Core.Lib;
using Rondo.Core.Lib.Containers;
using Rondo.Core.Lib.Platform;
using TenSecCastle.Model;
using Unity.Mathematics;

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
                        if (FindTarget(*model, unit).Test(out var target)) {
                            unit.State = UnitState.Attacking;
                            unit.StateProgress = 0;
                            unit.TargetUnitId = target.Id;
                            target.HitPoints -= CalculateDamage(unit, target);
                            if (target.HitPoints <= 0) {
                                target.HitPoints = 0;
                                target.State = UnitState.Dead;
                                target.StateProgress = 0;
                            }
                        }
                        else if (FindCellToMove(*model, unit).Test(out var cell)) {
                            unit.State = UnitState.Moving;
                            unit.StateProgress = 0;
                            unit.Direction = cell - unit.Cell;
                            unit.Cell = cell;
                        }
                        break;
                    }
                    case UnitState.Attacking:
                        unit.StateProgress += *dt;
                        if (unit.StateProgress > (1f / unit.AttackSpeed)) {
                            unit.State = UnitState.Idle;
                        }
                        break;
                    case UnitState.Moving:
                        unit.StateProgress += *dt;
                        if (unit.StateProgress > (1f / unit.MoveSpeed)) {
                            unit.State = UnitState.Idle;
                        }
                        break;
                    case UnitState.Dead:
                        unit.StateProgress += *dt;
                        if (unit.StateProgress > 2f) { //Death animation length
                            return Maybe<Unit>.Nothing;
                        }
                        break;
                }
                return Maybe<Unit>.Just(unit);
            }

            model.Units = model.Units.FilterMap(Cf.New<Unit, GameModel, float, Maybe<Unit>>(&UpdateUnit, model, dt));
            return model;
        }

        private static Maybe<Unit> UnitAt(GameModel model, int2 cell) {
            static bool CellEq(Unit unit, int2* cell) {
                return unit.Cell.Equals(*cell);
            }

            return model.Units.First(Cf.New<Unit, int2, bool>(&CellEq, cell));
        }

        private static Maybe<Unit> FindTarget(GameModel model, Unit value) {
            throw new NotImplementedException();
        }

        private static Maybe<int2> FindCellToMove(GameModel model, Unit value) {
            throw new NotImplementedException();
        }

        private static int CalculateDamage(Unit attacker, Unit victim) {
            throw new NotImplementedException();
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
                    var unit = CreateUnit(model.BasicUnit, player.Slots, ++model.LastUnitId, spawnPoint);
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

        private static L<Slot> ShuffleSlots(GameModel model) {
            throw new NotImplementedException();
        }

        private static Unit CreateUnit(Unit baseUnit, L<Slot> currentSlots, ulong id, int2 spawnPoint) {
            var slots = currentSlots.Enumerator;
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
                            baseUnit.KillIncome += attr.Value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            baseUnit.Id = id;
            baseUnit.Cell = spawnPoint;
            return baseUnit;
        }
    }
}