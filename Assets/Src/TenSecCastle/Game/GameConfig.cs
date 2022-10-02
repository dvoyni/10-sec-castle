using Rondo.Core.Lib.Containers;
using TenSecCastle.Model;

namespace TenSecCastle.Game {
    public class GameConfig {
        public static L<Item> Items {
            get {
                var list = new L<Item>();

                list += new Item {
                    Id = 1,
                    SlotKind = SlotKind.Weapon,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.Weapon, Value = 15.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.Melee}
                    ),
                };

                list += new Item {
                    Id = 2,
                    SlotKind = SlotKind.Weapon,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.Weapon, Value = 10.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.Ranged}
                    ),
                };

                list += new Item {
                    Id = 3,
                    SlotKind = SlotKind.Weapon,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.Weapon, Value = 15.00f, AttackType = AttackType.Magical, AttackRange = AttackRange.Ranged}
                    ),
                };

                list += new Item {
                    Id = 4,
                    SlotKind = SlotKind.Weapon,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.Defense, Value = 3.50f, AttackType = AttackType.Magical, AttackRange = AttackRange.None},
                        new Attribute { Kind = AttributeKind.Defense, Value = 3.50f, AttackType = AttackType.Physical, AttackRange = AttackRange.None}
                    ),
                };

                list += new Item {
                    Id = 5,
                    SlotKind = SlotKind.Armor,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.Defense, Value = 5.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.None}
                    ),
                };

                list += new Item {
                    Id = 6,
                    SlotKind = SlotKind.Armor,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.Defense, Value = 5.00f, AttackType = AttackType.Magical, AttackRange = AttackRange.None}
                    ),
                };

                list += new Item {
                    Id = 7,
                    SlotKind = SlotKind.Armor,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.Defense, Value = 2.00f, AttackType = AttackType.Magical, AttackRange = AttackRange.None},
                        new Attribute { Kind = AttributeKind.Defense, Value = 2.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.None}
                    ),
                };

                list += new Item {
                    Id = 8,
                    SlotKind = SlotKind.Armor,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.Attack, Value = 10.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.None}
                    ),
                };

                list += new Item {
                    Id = 9,
                    SlotKind = SlotKind.Armor,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.Attack, Value = 10.00f, AttackType = AttackType.Magical, AttackRange = AttackRange.None}
                    ),
                };

                list += new Item {
                    Id = 10,
                    SlotKind = SlotKind.Armor,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.HitPoints, Value = 50.00f, AttackType = AttackType.None, AttackRange = AttackRange.None}
                    ),
                };

                list += new Item {
                    Id = 11,
                    SlotKind = SlotKind.Jewelry,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.Attack, Value = 3.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.None},
                        new Attribute { Kind = AttributeKind.Attack, Value = 3.00f, AttackType = AttackType.Magical, AttackRange = AttackRange.None}
                    ),
                };

                list += new Item {
                    Id = 12,
                    SlotKind = SlotKind.Jewelry,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.HitPointRegen, Value = 1.00f, AttackType = AttackType.None, AttackRange = AttackRange.None}
                    ),
                };

                list += new Item {
                    Id = 13,
                    SlotKind = SlotKind.Jewelry,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.Income, Value = 1.00f, AttackType = AttackType.None, AttackRange = AttackRange.None}
                    ),
                };

                list += new Item {
                    Id = 14,
                    SlotKind = SlotKind.Jewelry,
                    Attributes = new(
                        new Attribute { Kind = AttributeKind.HitPoints, Value = 25.00f, AttackType = AttackType.None, AttackRange = AttackRange.None}
                    ),
                };

                return list;
            }
        }

        public static Unit BasicUnit => new() {
            MaxHitPoints = 100.00f,
            HpRegen = 0.00f,
            PhysicsAttack = 50.00f,
            PhysicsDefense = 1.00f,
            MagicAttack = 50.00f,
            MagicDefense = 1.00f,
            MoveSpeed = 1.00f, //cells per second
            AttackSpeed = 1.00f, //attacks per second
            Income = 1, //income per kill
        };
    }
}
