using System.Collections.Generic;
using Rondo.Core.Lib.Containers;

namespace TenSecCastle.Model {
    public class GameConfig {
        public struct Info {
            public string Name;
            public string DescriptionFirst;
            public string DescriptionSecond;
        }

        public static Dictionary<ulong, Info> ItemsDescriptions = new Dictionary<ulong, Info>() {
                { 1, new Info() { Name = "Sword", DescriptionFirst = "+15 physical attack", DescriptionSecond = "" } },
                { 2, new Info() { Name = "Gun", DescriptionFirst = "+8 physical attack", DescriptionSecond = "" } },
                { 3, new Info() { Name = "Staff", DescriptionFirst = "+10 magic attack", DescriptionSecond = "" } },
                { 4, new Info() { Name = "Shield", DescriptionFirst = "+4 magic defense", DescriptionSecond = "+4 physical defense" } },
                { 5, new Info() { Name = "Plate armor", DescriptionFirst = "+6 physical defense", DescriptionSecond = "" } },
                { 6, new Info() { Name = "Scale armor", DescriptionFirst = "+8 magic defense", DescriptionSecond = "" } },
                { 7, new Info() { Name = "Leather armor", DescriptionFirst = "+4 magic defense", DescriptionSecond = "+4 physical defense" } },
                { 8, new Info() { Name = "Spiked armor", DescriptionFirst = "+15 physical attack", DescriptionSecond = "" } },
                { 9, new Info() { Name = "Mage Robe", DescriptionFirst = "+15 magic attack", DescriptionSecond = "" } },
                { 10, new Info() { Name = "Сuirass", DescriptionFirst = "+40 hit points", DescriptionSecond = "" } },
                { 11, new Info() { Name = "Ring of Power", DescriptionFirst = "+5 physical attack", DescriptionSecond = "+5 magic attack" } },
                { 12, new Info() { Name = "Amulet of Regeneration", DescriptionFirst = "+10 hp regeneration", DescriptionSecond = "" } },
                { 13, new Info() { Name = "Fortune Seal", DescriptionFirst = "+1 money for kill", DescriptionSecond = "" } },
                { 14, new Info() { Name = "Charm of health", DescriptionFirst = "+15 hit points", DescriptionSecond = "" } },
        };

        public static L<Item> Items {
            get {
                var list = new L<Item>();

                list += new Item {
                        Id = 1,
                        SlotKind = SlotKind.Weapon,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.Weapon, Value = 15.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.Melee }
                        ),
                };

                list += new Item {
                        Id = 2,
                        SlotKind = SlotKind.Weapon,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.Weapon, Value = 8.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.Ranged }
                        ),
                };

                list += new Item {
                        Id = 3,
                        SlotKind = SlotKind.Weapon,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.Weapon, Value = 10.00f, AttackType = AttackType.Magical, AttackRange = AttackRange.Ranged }
                        ),
                };

                list += new Item {
                        Id = 4,
                        SlotKind = SlotKind.Weapon,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.Defense, Value = 4.00f, AttackType = AttackType.Magical, AttackRange = AttackRange.None },
                            new Attribute { Kind = AttributeKind.Defense, Value = 4.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.None }
                        ),
                };

                list += new Item {
                        Id = 5,
                        SlotKind = SlotKind.Armor,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.Defense, Value = 6.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.None }
                        ),
                };

                list += new Item {
                        Id = 6,
                        SlotKind = SlotKind.Armor,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.Defense, Value = 8.00f, AttackType = AttackType.Magical, AttackRange = AttackRange.None }
                        ),
                };

                list += new Item {
                        Id = 7,
                        SlotKind = SlotKind.Armor,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.Defense, Value = 4.00f, AttackType = AttackType.Magical, AttackRange = AttackRange.None },
                            new Attribute { Kind = AttributeKind.Defense, Value = 4.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.None }
                        ),
                };

                list += new Item {
                        Id = 8,
                        SlotKind = SlotKind.Armor,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.Attack, Value = 15.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.None }
                        ),
                };

                list += new Item {
                        Id = 9,
                        SlotKind = SlotKind.Armor,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.Attack, Value = 15.00f, AttackType = AttackType.Magical, AttackRange = AttackRange.None }
                        ),
                };

                list += new Item {
                        Id = 10,
                        SlotKind = SlotKind.Armor,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.HitPoints, Value = 40.00f, AttackType = AttackType.None, AttackRange = AttackRange.None }
                        ),
                };

                list += new Item {
                        Id = 11,
                        SlotKind = SlotKind.Jewelry,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.Attack, Value = 5.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.None },
                            new Attribute { Kind = AttributeKind.Attack, Value = 5.00f, AttackType = AttackType.Magical, AttackRange = AttackRange.None }
                        ),
                };

                list += new Item {
                        Id = 12,
                        SlotKind = SlotKind.Jewelry,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.HitPointRegen, Value = 10.00f, AttackType = AttackType.None, AttackRange = AttackRange.None }
                        ),
                };

                list += new Item {
                        Id = 13,
                        SlotKind = SlotKind.Jewelry,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.Income, Value = 1.00f, AttackType = AttackType.None, AttackRange = AttackRange.None }
                        ),
                };

                list += new Item {
                        Id = 14,
                        SlotKind = SlotKind.Jewelry,
                        Attributes = new(
                            new Attribute { Kind = AttributeKind.HitPoints, Value = 15.00f, AttackType = AttackType.None, AttackRange = AttackRange.None }
                        ),
                };

                return list;
            }
        }

        public static Unit BasicUnit => new() {
                MaxHitPoints = 125.00f,
                HpRegen = 0.00f,
                PhysicsAttack = 40.00f,
                PhysicsDefense = 1.00f,
                MagicAttack = 50.00f,
                MagicDefense = 2.00f,
                MoveSpeed = 1.00f, //cells per second
                AttackSpeed = 1.00f, //attacks per second
                Income = 1, //income per kill
        };
    }
}