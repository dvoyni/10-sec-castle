using System.Collections.Generic;
using Rondo.Core.Lib.Containers;
using TenSecCastle.Model;

namespace TenSecCastle.Game {
    public class GameConfig {
public struct Info {
 public string Name;
 public string DescriptionFirst;
public string DescriptionSecond;
 }
public static Dictionary<ulong, Info> ItemsDescriptions = new Dictionary<ulong, Info>() {
{1, new Info() {Name = "Sword", DescriptionFirst = "+15 physical attack", DescriptionSecond = ""}},
{2, new Info() {Name = "Gun", DescriptionFirst = "+10 physical attack", DescriptionSecond = ""}},
{3, new Info() {Name = "Staff", DescriptionFirst = "+15 magic attack", DescriptionSecond = ""}},
{4, new Info() {Name = "Shield", DescriptionFirst = "+4 magic defense", DescriptionSecond = "+4 physical defense"}},
{5, new Info() {Name = "Plate armor", DescriptionFirst = "+5 physical defense", DescriptionSecond = ""}},
{6, new Info() {Name = "Scale armor", DescriptionFirst = "+5 magic defense", DescriptionSecond = ""}},
{7, new Info() {Name = "Leather armor", DescriptionFirst = "+2 magic defense", DescriptionSecond = "+2 physical defense"}},
{8, new Info() {Name = "Spiked armor", DescriptionFirst = "+10 physical attack", DescriptionSecond = ""}},
{9, new Info() {Name = "Mage Robe", DescriptionFirst = "+10 magic attack", DescriptionSecond = ""}},
{10, new Info() {Name = "Сuirass", DescriptionFirst = "+50 hit points", DescriptionSecond = ""}},
{11, new Info() {Name = "Ring of Power", DescriptionFirst = "+3 physical attack", DescriptionSecond = "+3 magic attack"}},
{12, new Info() {Name = "Amulet of Regeneration", DescriptionFirst = "+1 hp regeneration", DescriptionSecond = ""}},
{13, new Info() {Name = "Fortune Seal", DescriptionFirst = "+1 money for kill", DescriptionSecond = ""}},
{14, new Info() {Name = "Charm of health", DescriptionFirst = "+25 hit points", DescriptionSecond = ""}},
};
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
                        new Attribute { Kind = AttributeKind.Defense, Value = 4.00f, AttackType = AttackType.Magical, AttackRange = AttackRange.None},
                        new Attribute { Kind = AttributeKind.Defense, Value = 4.00f, AttackType = AttackType.Physical, AttackRange = AttackRange.None}
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
