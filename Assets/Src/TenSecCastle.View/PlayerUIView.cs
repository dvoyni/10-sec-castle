using System;
using Rondo.Core;
using Rondo.Unity.Components;
using TenSecCastle.Model;
using UnityEngine;

namespace TenSecCastle.View {
    public class PlayerUIView :MonoBehaviour,IDataDrivenComponent<PlayerUIViewData> {
        public IMessenger Messenger { get; set; }
        public void Sync(PlayerUIViewData model) {
            //model
        }
    }
}