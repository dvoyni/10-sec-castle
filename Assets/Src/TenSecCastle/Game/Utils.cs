using System;
using Rondo.Core.Lib;
using Rondo.Core.Lib.Containers;
using TenSecCastle.Model;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace TenSecCastle.Game {
    public static class Utils {
        public static bool PlayerIsHuman(Player player) {
            return player.Kind == PlayerKind.Human;
        }

        public static bool PlayerIsAI(Player player) {
            return player.Kind == PlayerKind.AI;
        }

        public static unsafe Maybe<Item> FirstItemWithSlot(L<Item> items, SlotKind slot) {
            static bool WithSlot(Item a, SlotKind* slot) {
                return a.SlotKind == *slot;
            }

            return items.First(Cf.New<Item, SlotKind, bool>(&WithSlot, slot));
        }

        public static unsafe L<Item> ShuffleItems(L<Item> items) {
            static int RandComp(Item a, Item b, Random* r) {
                return r->NextInt(2) == 0 ? -1 : 1;
            }

            return items.SortWith(
                Cf.New<Item, Item, Random, int>(&RandComp, new Random((uint)DateTime.Now.ToFileTime()))
            );
        }

        public static GameModel NewModel {
            get {
                var model = new GameModel {
                        Items = GameConfig.Items,
                        BasicUnit = GameConfig.BasicUnit,
                        FieldSize = new int2(2, 14),
                        Interval = 10,
                        BaseIncome = 1,
                        MoveAxis = new int2(0, 1),
                        Timeout = 5,
                };
                model.Players = new(
                    new Player {
                            Id = 1,
                            Kind = PlayerKind.Human,
                            Slots = GameLogic.ShuffleSlots(model),
                            SpawnDirection = new int2(0, 1),
                            SpawnPoints = new L<int2>(
                                new int2(-1, 0),
                                new(model.FieldSize.x, 0)
                            ),
                            CastleHitPoints = 20,
                            Coins = 3
                    },
                    new Player {
                            Id = 2,
                            Kind = PlayerKind.AI,
                            Slots = GameLogic.ShuffleSlots(model),
                            SpawnDirection = new int2(0, -1),
                            SpawnPoints = new L<int2>(
                                new int2(-1, model.FieldSize.y - 1),
                                new(model.FieldSize.x, model.FieldSize.y - 1)
                            ),
                            CastleHitPoints = 20,
                            Coins = 3
                    }
                );
                return model;
            }
        }
    }
}