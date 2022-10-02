using System;
using Rondo.Core.Lib;
using Rondo.Core.Lib.Containers;
using TenSecCastle.Model;
using Random = Unity.Mathematics.Random;

namespace TenSecCastle.Game {
    public static class Utils {
        public static bool PlayerIsHuman(Player player) {
            return player.Kind == PlayerKind.Human;
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
    }
}