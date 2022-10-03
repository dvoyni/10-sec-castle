using System;
using System.Linq;
using Rondo.Core;
using Rondo.Unity.Components;
using TenSecCastle.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TenSecCastle.View {
    public class UnitView : MonoBehaviour, IDataDrivenComponent<UnitViewData> {
        [Serializable]
        private struct ItemBinding {
            public ulong Id;
            public GameObject Obj;
        }
        
        [Serializable]
        private struct ArmorSetBinding {
            public ulong Id;
            public GameObject[] Obj;
        }
        
        [SerializeField] private GameObject _arrow;
        
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private ItemBinding[] Weapons;
        [SerializeField] private ItemBinding[] Auras;
        [SerializeField] private ArmorSetBinding[] Items;

        private string _currentAnimationClip;
        private float _currentProgress;

        private UnitState _currentState;
        private float _currentUnitHP;
        private float _currentAnimationProgress;

        private float _deadAnimationTime;
        private ulong _currentWeaponId;
        private ulong _currentArmorId;
        private ulong _currentAuraId;

        public IMessenger Messenger { private get; set; }

        private void Awake() {
            Init();
        }

        public void Sync(UnitViewData unitViewData) {
            var unit = unitViewData.Unit;
            UpdateAnimationState(unit);
            UpdateAnimationProgress(unit);
            PlayAnimation();

            UpdateWeapon(unit);
            UpdateArmor(unit);
            UpdateAuras(unit);
            _arrow.SetActive(unitViewData.SelectedUnitId);
            
        }

        private void UpdateAuras(Unit unit) {
            if (unit.JewelryId != _currentAuraId) {
                _currentAuraId = unit.ArmorId;

                for (int i = 0; i < Auras.Length; i++) {
                    Auras[i].Obj.SetActive(_currentAuraId == Auras[i].Id);
                }
            }
        }

        private void UpdateArmor(Unit unit) {
            if (unit.ArmorId != _currentArmorId) {
                _currentArmorId = unit.ArmorId;

                for (int i = 0; i < Items.Length; i++) {
                    for (int j = 0; j < Items[i].Obj.Length; j++) {
                        Items[i].Obj[j].SetActive(_currentArmorId == Items[i].Id);
                    }
                }
            }
        }

        private void UpdateWeapon(Unit unit) {
            if (unit.WeaponId != _currentWeaponId) {
                _currentWeaponId = unit.WeaponId;

                for (int i = 0; i < Weapons.Length; i++) {
                    Weapons[i].Obj.SetActive(_currentWeaponId == Weapons[i].Id);
                }
            }
        }

        private void PlayAnimation() {
            if (!Equals(string.IsNullOrEmpty(_currentAnimationClip))) {
                _playerAnimator.speed = 0f;
                _playerAnimator.Play(_currentAnimationClip, 0, _currentProgress);
            }
        }

        private void UpdateAnimationProgress(Unit unit) {
            _currentProgress = _currentState != UnitState.Dieing
                    ? unit.StateProgress
                    : Math.Clamp(unit.StateTime, 0, _deadAnimationTime);
        }

        private void UpdateAnimationState(Unit unit) {
            
            if (unit.State != _currentState) {
                _currentState = unit.State;
                _currentAnimationClip = GetAnimationClipName(_currentState,unit.WeaponId);
            }
        }

        private void Init() {
            var animController = _playerAnimator.runtimeAnimatorController;
            var animaName = GetAnimationClipName(UnitState.Dieing);
            var clip = animController.animationClips.First(a => a.name == animaName);
            _deadAnimationTime = clip.length;

            if (clip == null)
                throw new Exception("Can't find animation");
        }

        private string GetAnimationClipName(UnitState unitState, ulong? weaponId=null) {
            switch (unitState) {
                case UnitState.Moving:
                    return "Male_Sword_Walk";
                case UnitState.Idle:
                    return "Male Sword Stance";
                case UnitState.Attacking:
                    return weaponId switch {
                        1 => $"Male Attack 1",
                        2 => $"shotgun_fire",
                        _ => $"Male Attack 2"
                    };
                case UnitState.Dieing:
                    return "Male Sword Die";
            }

            throw new Exception("Wrong state");
        }
    }
}