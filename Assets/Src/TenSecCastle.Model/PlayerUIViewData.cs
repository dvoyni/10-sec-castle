using System;
using Rondo.Core.Lib.Containers;

namespace TenSecCastle.Model {
    public struct PlayerUIViewData : IEquatable<PlayerUIViewData> {
        
        public L<Slot> PlayerSlots;
        public Maybe<L<ulong>> SelectedUnitSlots;
        public int CastleHitPoints;
        public int Coins;
        public int EnemyCastleHitPoints;
        public float TimeToSpawn;
        public float MaxTimeToSpawn;
        public Maybe<bool> PlayerWon;


        public bool Equals(PlayerUIViewData other) {
            return PlayerSlots.Equals(other.PlayerSlots) && SelectedUnitSlots.Equals(other.SelectedUnitSlots) && CastleHitPoints == other.CastleHitPoints && Coins == other.Coins && EnemyCastleHitPoints == other.EnemyCastleHitPoints && TimeToSpawn.Equals(other.TimeToSpawn) && MaxTimeToSpawn.Equals(other.MaxTimeToSpawn) && PlayerWon.Equals(other.PlayerWon);
        }
       
    }
}