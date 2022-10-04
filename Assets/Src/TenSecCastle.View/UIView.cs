using System;
using System.Collections.Generic;
using System.Globalization;
using Rondo.Core;
using Rondo.Unity.Components;
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
            public Animator Animator;
            public ulong CurrentId;
        }

        [Serializable]
        public struct SlotWindowInfo {
            public SlotKind Id;
            public Image Img;
            public TextMeshProUGUI Name;
            public TextMeshProUGUI DiscFirst;
            public TextMeshProUGUI DiscSecond;
            public ulong CurrentId;
        }

        private Dictionary<ulong, Sprite> _spriteCache = new();

        private void Awake() {
            _infoWindowCloseButton.onClick.AddListener(
                () => { Messenger.PostMessage(new Msg(Screen.Game) { GameMsg = new GameMsg(MsgKind.UnitClicked) }); }
            );

            _winWindowRestartButton.onClick.AddListener(
                () => { Messenger.PostMessage(new Msg(Screen.Game) { GameMsg = new GameMsg(MsgKind.Restart) }); }
            );

            _loseWindowRestartButton.onClick.AddListener(
                () => { Messenger.PostMessage(new Msg(Screen.Game) { GameMsg = new GameMsg(MsgKind.Restart) }); }
            );

            _winWindowExitButton.onClick.AddListener(Application.Quit);
            _loseWindowExitButton.onClick.AddListener(Application.Quit);
        }

        [SerializeField] private SlotInfo[] _playerSlots;
        [SerializeField] private SlotWindowInfo[] _unitSlots;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private TextMeshProUGUI _spawnText;
        [SerializeField] private Image _spawnUnitFillImage;
        [SerializeField] private Image _playerHPFillImage;
        [SerializeField] private Image _enemyHPFillImage;
        [SerializeField] private GameObject _infoWindow;
        [SerializeField] private GameObject _tooltipWindow;
        [SerializeField] private GameObject _winWindow;
        [SerializeField] private Button _winWindowRestartButton;
        [SerializeField] private Button _winWindowExitButton;
        [SerializeField] private GameObject _loseWindow;
        [SerializeField] private Button _loseWindowRestartButton;
        [SerializeField] private Button _loseWindowExitButton;
        [SerializeField] private Button _infoWindowCloseButton;

        public IMessenger Messenger { get; set; }
        private delegate*<SlotKind, Msg> _listener;
        private int _coinsValue;
        private double _timeToSpawnValue;
        private int _castleHitPointsValue;
        private int _enemyCastleHitPointsValue;

        public void Sync(PlayerUIViewData model) {
            for (int i = 0; i < _playerSlots.Length; i++) {
                _playerSlots[i].Btn.interactable = model.Coins > 0;
            }

            BindButtons(model);
            SetIcons(model);

            if (_coinsValue != model.Coins) {
                _coinsValue = model.Coins;
                _moneyText.text = model.Coins.ToString(CultureInfo.InvariantCulture);
            }

            if (_timeToSpawnValue != model.TimeToSpawn) {
                _timeToSpawnValue = model.TimeToSpawn;
                _spawnText.text = ((int)model.TimeToSpawn + 1).ToString(CultureInfo.InvariantCulture);
                _spawnUnitFillImage.fillAmount = 1 - model.TimeToSpawn / model.MaxTimeToSpawn;
            }

            if (_castleHitPointsValue != model.CastleHitPoints) {
                _castleHitPointsValue = model.CastleHitPoints;
                _playerHPFillImage.fillAmount = model.CastleHitPoints / 20f;
            }

            if (_enemyCastleHitPointsValue != model.EnemyCastleHitPoints) {
                _enemyCastleHitPointsValue = model.EnemyCastleHitPoints;
                _enemyHPFillImage.fillAmount = model.EnemyCastleHitPoints / 20f;
            }

            SetInfoWindow(model);
            var winActive = false;
            var loseActive = false;

            if (model.PlayerWon.Test(out var playerWon)) {
                if (playerWon) {
                    winActive = true;
                }
                else {
                    loseActive = true;
                }
            }

            if (_winWindow.activeSelf != winActive) {
                _winWindow.SetActive(winActive);
            }

            if (_loseWindow.activeSelf != loseActive) {
                _loseWindow.SetActive(loseActive);
            }

            if (_tooltipWindow == model.HideRerollTooltop) {
                _tooltipWindow.SetActive(!model.HideRerollTooltop);
            }
        }

        private void SetInfoWindow(PlayerUIViewData model) {
            var active = model.SelectedUnitSlots.Test(out var selectedUnitSlots);
            if (active) {
                var index = 0;
                var slots = selectedUnitSlots.Enumerator;
                while (slots.MoveNext()) {
                    var itemId = slots.Current;
                    var uiSlot = _unitSlots[index++];
                    if (uiSlot.CurrentId != itemId) {
                        uiSlot.CurrentId = itemId;
                        uiSlot.DiscFirst.text = GameConfig.ItemsDescriptions[itemId].DescriptionFirst;
                        uiSlot.DiscSecond.text = GameConfig.ItemsDescriptions[itemId].DescriptionSecond;
                        uiSlot.Name.text = GameConfig.ItemsDescriptions[itemId].Name;

                        if (_spriteCache.TryGetValue(itemId, out var sprite)) {
                            uiSlot.Img.sprite = sprite;
                        }
                        else {
                            Addressables
                                    .LoadAssetAsync<Sprite>($"Assets/Data/Icons/{itemId}.png")
                                    .Completed += op => {
                                uiSlot.Img.sprite = op.Result;
                                _spriteCache[itemId] = op.Result;
                            };
                        }
                    }
                }
            }
            if (_infoWindow.activeSelf != active) {
                _infoWindow.SetActive(active);
            }
        }

        private void SetIcons(PlayerUIViewData model) {
            var index = 0;
            var playerSlots = model.PlayerSlots.Enumerator;
            while (playerSlots.MoveNext()) {
                var itemId = playerSlots.Current.Item.Id;
                var uiSlot = _playerSlots[index++];
                if ((itemId != uiSlot.CurrentId) && (playerSlots.Current.SwapProgress >= 1)) {
                    uiSlot.CurrentId = itemId;

                    uiSlot.DiscFirst.text = GameConfig.ItemsDescriptions[itemId].DescriptionFirst;
                    uiSlot.DiscSecond.text = GameConfig.ItemsDescriptions[itemId].DescriptionSecond;

                    if (_spriteCache.TryGetValue(itemId, out var sprite)) {
                        uiSlot.Img.sprite = sprite;
                    }
                    else {
                        Addressables
                                .LoadAssetAsync<Sprite>($"Assets/Data/Icons/{itemId}.png")
                                .Completed += op => {
                            uiSlot.Img.sprite = op.Result;
                            _spriteCache[itemId] = op.Result;
                        };
                    }
                }
            }
        }

        private void BindButtons(PlayerUIViewData model) {
            if (_listener != model.OnSlotClick) {
                _listener = model.OnSlotClick;
                for (var i = 0; i < _playerSlots.Length; i++) {
                    var slot = _playerSlots[i];
                    slot.Btn.onClick.RemoveAllListeners();

                    slot.Btn.onClick.AddListener(() => {
                        Messenger.PostMessage(model.OnSlotClick(slot.Id));
                        slot.Animator.Play("Swap");
                    });
                }
            }
        }
    }
}