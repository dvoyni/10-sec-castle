using Rondo.Core.Lib.Containers;

namespace TenSecCastle.Game {
    public class GameConfig {
        public static L<Item> Items {
            get {
                var list = new L<Item>();

                while (true) {
                    list += new Item {
                            Id = 1,
                            Slot = Slot.Armor,
                            Attributes = new(
                                new Attribute { Kind = AttributeKind.Defense, Value = 10, AttackType = AttackType.Physical },
                                new Attribute { Kind = AttributeKind.Defense, Value = 5, AttackType = AttackType.Magical }
                            ),
                    };
                    break;
                }

                return list;
            }
        }
    }
}