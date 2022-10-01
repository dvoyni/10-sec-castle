using Rondo.Core.Lib.Containers;

namespace TenSecCastle.Model {
    public class GameConfig {
        public static L<Item> Items {
            get {
                var list = new L<Item>();

                while (true) {
                    list += new Item {
                            Id = 1,
                            SlotKind = SlotKind.Armor,
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

        public static Unit BasicUnit => new() {
                MaxHitPoints = 10,
                HpRegen = 1,
                PhysicsAttack = 5,
                PhysicsDefense = 3,
                MagicAttack = 4,
                MagicDefense = 1,
                AttackSpeed = 1, //attacks per second
                MoveSpeed = 2, //cells per second
        };
    }
}