using System;
using Rondo.Core.Lib.Containers;
using Unity.Mathematics;

namespace TenSecCastle.Model {
    public struct GameModel {
        public GameModel(GameModel other) {
            this = other;
        }

        public L<Item> Items;
        public Unit BasicUnit;
        public int2 FieldSize;
        public float Interval;
        public int BaseIncome;
        public int2 MoveAxis;

        public L<Player> Players;
        public L<Unit> Units;
        public float Timeout;
        public ulong LastUnitId;
    }

    public struct Player {
        public ulong Id;
        public PlayerKind Kind;
        public L<Slot> Slots;
        public int Coins;
        public int2 SpawnDirection;
        public L<int2> SpawnPoints;
        public int CurrentSpawnPoint;
    }

    public enum PlayerKind {
        Human,
        AI,
    }

    public struct Slot {
        public Item Item;
    }

    public struct Unit : IEquatable<Unit> {
        public ulong ArmorId;
        public ulong WeaponId;
        public ulong JewelryId;

        public float MaxHitPoints;
        public float HpRegen;
        public float PhysicsAttack;
        public float PhysicsDefense;
        public float MagicAttack;
        public float MagicDefense;
        public int KillIncome;
        public AttackType AttackType;
        public AttackRange AttackRange;
        public float AttackSpeed;
        public float MoveSpeed;
        public int Income;

        public ulong Id;
        public ulong Owner;
        public float HitPoints;
        public UnitState State;
        public int2 Cell;
        public int2 Direction;
        public float StateTime;
        public float StateProgress;
        public ulong TargetUnitId;

        public bool Equals(Unit other) {
            return ArmorId == other.ArmorId && WeaponId == other.WeaponId && JewelryId == other.JewelryId && MaxHitPoints == other.MaxHitPoints && HpRegen == other.HpRegen && PhysicsAttack == other.PhysicsAttack && PhysicsDefense == other.PhysicsDefense && MagicAttack == other.MagicAttack && MagicDefense == other.MagicDefense && AttackType == other.AttackType && AttackRange == other.AttackRange && AttackSpeed.Equals(other.AttackSpeed) && MoveSpeed.Equals(other.MoveSpeed) && Owner == other.Owner && HitPoints == other.HitPoints && State == other.State && Cell.Equals(other.Cell) && Direction.Equals(other.Direction) && StateTime.Equals(other.StateTime) && KillIncome.Equals(other.KillIncome);
        }
    }

    public enum UnitState {
        Idle,
        Attacking,
        Moving,
        Dieing,
    }

    public struct Item {
        public ulong Id;
        public SlotKind SlotKind;
        public L<Attribute> Attributes;
    }

    public enum SlotKind {
        Armor,
        Weapon,
        Jewelry,
    }

    public struct Attribute {
        public AttributeKind Kind;
        public float Value;
        public AttackType AttackType;
        public AttackRange AttackRange;
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