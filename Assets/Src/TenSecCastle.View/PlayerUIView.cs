using System;
using System.Globalization;
using Rondo.Core;
using Rondo.Unity.Components;
using TenSecCastle.Model;
using TMPro;
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

        public struct ItemsInfo {
            public ulong Id;
            public string Param1;
            public string Param2;
        }

        private ItemsInfo[] _itemsDescription;

        void Awake() {
            _itemsDescription = new ItemsInfo[14];
            ulong index = 0;
            
        }
        
        [SerializeField] private SlotInfo[] _playerSlots;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TextMeshProUGUI _spawnText;
        [SerializeField] private Image _spawnUnitFillImage;
        [SerializeField] private Image _playerHPFillImage;
        [SerializeField] private Image _enemyHPFillImage;
        
        public IMessenger Messenger { get; set; }
        private delegate*<SlotKind, Msg> _listener;
        

       
        public void Sync(PlayerUIViewData model) {
            BindButtons(model);
            SetIcons(model);

            _moneyText.text = model.Coins.ToString();

            _spawnText.text = ((int)model.TimeToSpawn+1).ToString();

            _spawnUnitFillImage.fillAmount = 1 - model.TimeToSpawn / model.MaxTimeToSpawn;

            _playerHPFillImage.fillAmount = model.CastleHitPoints / 10f;
            
            _enemyHPFillImage.fillAmount = model.EnemyCastleHitPoints / 10f;
        }

        private void SetIcons(PlayerUIViewData model) {
            var i = 0;
            var e = model.PlayerSlots.Enumerator;
            while (e.MoveNext()) {
                var aop = Addressables.LoadAssetAsync<Sprite>($"Assets/Data/Icons/{e.Current.Item.Id}.png");
                var j = i;
                aop.Completed += op => {
                    _playerSlots[j].Img.sprite = op.Result;
                    //e.Current.Item.Id;
                };
                i++;
            }
        }

        private void BindButtons(PlayerUIViewData model) {
            if (_listener != model.OnSlotClick) {
                _listener = model.OnSlotClick;

                for (var i = 0; i < _playerSlots.Length; i++) {
                    _playerSlots[i].Btn.onClick.RemoveAllListeners();
                    var slot = _playerSlots[i].Id;

                    _playerSlots[i].Btn.onClick.AddListener(() => { Messenger.PostMessage(model.OnSlotClick(slot)); });
                }
            }
        }
    }
}