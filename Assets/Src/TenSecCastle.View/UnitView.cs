using System;
using Rondo.Core;
using Rondo.Unity.Components;
using UnityEngine;

namespace TenSecCastle.View {
    public struct UnitData : IEquatable<UnitData> {
        public bool Equals(UnitData other) {
            return false;
        }
    }

    public class UnitView : MonoBehaviour, IDataDrivenComponent<UnitData> {
        public IMessenger Messenger { private get; set; }

        public void Sync(UnitData unit) { }
    }
}