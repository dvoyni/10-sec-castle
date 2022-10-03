using System;
using System.Globalization;
using Rondo.Core;
using Rondo.Unity.Components;
using TenSecCastle.Game;
using TenSecCastle.Model;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Screen = TenSecCastle.Model.Screen;

namespace TenSecCastle.View {
    public unsafe class UIView : MonoBehaviour, IDataDrivenComponent<PlayerUIViewData> {
        [Serializable]
        public struct SlotInfo {
            public SlotKind Id;
            public Image Img;
            public Button Btn;
            public TextMeshProUGUI DiscFirst;
            public TextMeshProUGUI DiscSecond;
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

            _infoWindowCloseButton.onClick.AddListener(
                () => { Messenger.PostMessage(new Msg(Screen.Game) { GameMsg = new GameMsg(MsgKind.UnitClicked) }); }
            );

            //restart:
            //Messenger.PostMessage(new Msg(Screen.Game) { GameMsg = new GameMsg(MsgKind.Restart) });
        }

        [SerializeField] private SlotInfo[] _playerSlots;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TextMeshProUGUI _spawnText;
        [SerializeField] private Image _spawnUnitFillImage;
        [SerializeField] private Image _playerHPFillImage;
        [SerializeField] private Image _enemyHPFillImage;
        [SerializeField] private GameObject _infoWindow;
        [SerializeField] private Button _infoWindowCloseButton;

        public IMessenger Messenger { get; set; }
        private delegate*<SlotKind, Msg> _listener;

        public void Sync(PlayerUIViewData model) {
            BindButtons(model);
            SetIcons(model);

            _moneyText.text = model.Coins.ToString();

            _spawnText.text = ((int)model.TimeToSpawn + 1).ToString();

            _spawnUnitFillImage.fillAmount = 1 - model.TimeToSpawn / model.MaxTimeToSpawn;

            _playerHPFillImage.fillAmount = model.CastleHitPoints / 10f;

            _enemyHPFillImage.fillAmount = model.EnemyCastleHitPoints / 10f;

            if (model.SelectedUnitSlots.Test(out var window)) {
                _infoWindow.SetActive(true);
            }
            else {
                _infoWindow.SetActive(false);
            }
        }

        private void SetIcons(PlayerUIViewData model) {
            var i = 0;
            var e = model.PlayerSlots.Enumerator;
            while (e.MoveNext()) {
                var aop = Addressables.LoadAssetAsync<Sprite>($"Assets/Data/Icons/{e.Current.Item.Id}.png");
                var j = i;
                var currentId = e.Current.Item.Id;
                aop.Completed += op => {
                    _playerSlots[j].Img.sprite = op.Result;
                    _playerSlots[j].DiscFirst.text = GameConfig.ItemsDescriptions[currentId].DescriptionFirst;
                    _playerSlots[j].DiscSecond.text = GameConfig.ItemsDescriptions[currentId].DescriptionSecond;
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