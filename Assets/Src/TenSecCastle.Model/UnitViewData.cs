﻿using System;
using TenSecCastle.Model;

namespace TenSecCastle.View {
    public struct UnitViewData : IEquatable<UnitViewData> {
        public Unit Unit;
        public bool Selected;

        public bool Equals(UnitViewData other) {
            return Unit.Equals(other.Unit) && Selected == other.Selected;
        }

        public override bool Equals(object obj) {
            return obj is UnitViewData other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Unit, Selected);
        }
    }
}