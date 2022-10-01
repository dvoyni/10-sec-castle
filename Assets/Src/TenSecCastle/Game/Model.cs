using Rondo.Core.Lib.Containers;
using TenSecCastleConfig;
using Unity.Mathematics;

namespace TenSecCastle.Game {
    public struct Model {
        public Model(Model other) {
            this = other;
        }

        public L<Unit> Units;
    }

    public struct Unit {
        public ulong ArmorId;
        public ulong WeaponId;
        public ulong JewelryId;

        public int MaxHitPoints;
        public int HpRegen;
        public int PhysicsAttack;
        public int PhysicsDefense;
        public int MagicAttack;
        public int MagicDefense;
        public AttackType AttackType;
        public AttackRange AttackRange;
        public int AttackSpeed;
        public int MoveSpeed;

        public int HitPoints;
        public UnitState State;
        public int2 Cell;
        public int2 Direction;
        public float StateProgress;
    }

    public enum UnitState {
        Idle,
        Attacking,
        Moving,
    }
}