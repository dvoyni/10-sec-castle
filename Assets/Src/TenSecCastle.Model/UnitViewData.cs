using System;

namespace TenSecCastle.Model {
    public struct UnitViewData : IEquatable<UnitViewData> {
        public Unit Unit;
        public bool SelectedUnitId;

        public bool Equals(UnitViewData other) {
            return Unit.Equals(other.Unit) && SelectedUnitId == other.SelectedUnitId;
        }

        public override bool Equals(object obj) {
            return obj is UnitViewData other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Unit, SelectedUnitId);
        }
    }
}