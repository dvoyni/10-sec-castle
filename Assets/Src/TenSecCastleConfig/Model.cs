using Rondo.Core.Lib.Containers;

namespace TenSecCastleConfig {
    public struct Item {
        public ulong Id;
        public Slot Slot;
        public L<Attribute> Attributes;
    }

    public enum Slot {
        Armor,
        Weapon,
        Jewelry,
    }

    public struct Attribute {
        public AttributeKind Kind;
        public int Value;
        public AttackType AttackType;
    }

    public enum AttributeKind {
        Weapon,
        Attack,
        Defense,
        HitPoints,
        HitPointRegen,
        Income,
    }

    public enum AttackRange {
        None,
        Melee,
        Ranged,
    }

    public enum AttackType {
        None,
        Physical,
        Magical,
    }
}