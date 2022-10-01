using Rondo.Core;
using Rondo.Unity.Components;
using TenSecCastle.Model;
using UnityEngine;

namespace TenSecCastle.View {
    public class UnitView : MonoBehaviour, IDataDrivenComponent<Unit> {
        public IMessenger Messenger { private get; set; }

        public void Sync(Unit unit) { }
    }
}