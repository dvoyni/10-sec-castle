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
            public Animator Animator;
        }
        
        [Serializable]
        public struct SlotWindowInfo {
            public SlotKind Id;
            public Image Img;
            public TextMeshProUGUI Name;
            public TextMeshProUGUI DiscFirst;
            public TextMeshProUGUI DiscSecond;
        }

        void Awake() {
            ulong index = 0;

            _infoWindowCloseButton.onClick.AddListener(
                () => { Messenger.PostMessage(new Msg(Screen.Game) { GameMsg = new GameMsg(MsgKind.UnitClicked) }); }
            );

           _winWindowRestartButton.onClick.AddListener(() => {
               Messenger.PostMessage(new Msg(Screen.Game) { GameMsg = new GameMsg(MsgKind.Restart) });
           });
           
           _loseWindowRestartButton.onClick.AddListener(() => {
               Messenger.PostMessage(new Msg(Screen.Game) { GameMsg = new GameMsg(MsgKind.Restart) });
           });
           
           _winWindowExitButton.onClick.AddListener(() => {
               Application.Quit();
           });
           _loseWindowExitButton.onClick.AddListener(() => {
               Application.Quit();
           });
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

        public void Sync(PlayerUIViewData model) {

            for (int i = 0; i < _playerSlots.Length; i++) {
                _playerSlots[i].Btn.interactable = model.Coins > 0;
            }
            BindButtons(model);
            SetIcons(model);

            _moneyText.text = model.Coins.ToString();

            _spawnText.text = ((int)model.TimeToSpawn + 1).ToString();

            _spawnUnitFillImage.fillAmount = 1 - model.TimeToSpawn / model.MaxTimeToSpawn;

            _playerHPFillImage.fillAmount = model.CastleHitPoints / 20f;

            _enemyHPFillImage.fillAmount = model.EnemyCastleHitPoints / 20f;

            SetInfoWindow(model);

            if (model.PlayerWon.Test(out var value)) {
                if (value)
                    _winWindow.SetActive(true);
                else
                    _loseWindow.SetActive(true);
            }
            else {
                _winWindow.SetActive(false);
                _loseWindow.SetActive(false);
            }
            
            _tooltipWindow.SetActive(!model.HideRerollTooltop);
        }

        private void SetInfoWindow(PlayerUIViewData model) {
            if (model.SelectedUnitSlots.Test(out var window)) {
                _infoWindow.SetActive(true);
                var i = 0;
                var e = window.Enumerator;
                while (e.MoveNext()) {
                    var id = e.Current;
                    var aop = Addressables.LoadAssetAsync<Sprite>($"Assets/Data/Icons/{id}.png");
                    var slot = _unitSlots[i];
                    aop.Completed += op => {
                        slot.Img.sprite = op.Result;
                        slot.DiscFirst.text = GameConfig.ItemsDescriptions[id].DescriptionFirst;
                        slot.DiscSecond.text = GameConfig.ItemsDescriptions[id].DescriptionSecond;
                        slot.Name.text = GameConfig.ItemsDescriptions[id].Name;
                    };
                    i++;
                }
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
                var currentTime = e.Current.SwapProgress;
                aop.Completed += op => {
                    if (currentTime >= 0.98f) {
                        _playerSlots[j].Img.sprite = op.Result;
                        _playerSlots[j].DiscFirst.text = GameConfig.ItemsDescriptions[currentId].DescriptionFirst;
                        _playerSlots[j].DiscSecond.text = GameConfig.ItemsDescriptions[currentId].DescriptionSecond;
                    }
                    
                };
                i++;
            }
        }

        private void BindButtons(PlayerUIViewData model) {
            if (_listener != model.OnSlotClick) {
                _listener = model.OnSlotClick;
                var coins = model.Coins;
                for (var i = 0; i < _playerSlots.Length; i++) {
                    _playerSlots[i].Btn.onClick.RemoveAllListeners();
                    var slot = _playerSlots[i].Id;
                    var j = i;
                    
                    _playerSlots[i].Btn.onClick.AddListener(() => {
                        Messenger.PostMessage(model.OnSlotClick(slot));
                        if (coins > 0) {
                            _playerSlots[j].Animator.Play("Swap");
                        }
                            
                    });
                }
            }
        }
    }
}