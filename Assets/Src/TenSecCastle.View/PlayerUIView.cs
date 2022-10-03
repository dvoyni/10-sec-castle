using System;
using Rondo.Core;
using Rondo.Unity.Components;
using TenSecCastle.Model;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace TenSecCastle.View {
    public unsafe class PlayerUIView :MonoBehaviour,IDataDrivenComponent<PlayerUIViewData> {
        [Serializable]
        public struct SlotInfo {
            public SlotKind Id;
            public Image Img;
            public Button Btn;
        }
        [SerializeField] private SlotInfo[] _playerSlots;
        public IMessenger Messenger { get; set; }
        private delegate*<SlotKind, Msg> _listener;
        

       
        public void Sync(PlayerUIViewData model) {

            if (_listener != model.OnSlotClick) {
                _listener = model.OnSlotClick;
                
                for (var i = 0; i < _playerSlots.Length; i++) {
                    _playerSlots[i].Btn.onClick.RemoveAllListeners();
                    var slot = _playerSlots[i].Id;
                    
                    _playerSlots[i].Btn.onClick.AddListener(() => {
                        Messenger.PostMessage(model.OnSlotClick(slot));
                    });
                }
            }

            {
                var i = 0;
                var e = model.PlayerSlots.Enumerator;
                while (e.MoveNext()) {
                    var aop = Addressables.LoadAssetAsync<Sprite>($"Assets/Data/Icons/{e.Current.Item.Id}.png");
                    var j = i;
                    aop.Completed += op => { _playerSlots[j].Img.sprite = op.Result; };
                    i++;
                }
            }
        }
    }
}