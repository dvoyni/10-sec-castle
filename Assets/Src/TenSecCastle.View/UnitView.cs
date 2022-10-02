using System;
using System.Linq;
using Rondo.Core;
using Rondo.Unity.Components;
using TenSecCastle.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TenSecCastle.View {
    public class UnitView : MonoBehaviour, IDataDrivenComponent<Unit> {
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private ItemBinding[] Weapons;

        private string _currentAnimationClip;
        private float _currentProgress;

        private UnitState _currentState;
        private float _currentUnitHP;
        private float _currentAnimationProgress;
        
        private float _deadAnimationTime;
        private ulong _currentWeaponId;

        public IMessenger Messenger { private get; set; }

        private void Awake() {
            Init();
        }

      public void Sync(Unit unit) {
            UpdateAnimationState(unit);
            UpdateAnimationProgress(unit);
            PlayAnimation();

            UpdateWeapon(unit);
      }

      [Serializable]
      private struct ItemBinding {
          public ulong Id;
          public GameObject Obj;
      }
      private void UpdateWeapon() {
         
      }

      private void UpdateWeapon(Unit unit) {
          if (unit.WeaponId != _currentWeaponId) {
              _currentWeaponId = unit.WeaponId;

              for (int i = 0; i < Weapons.Length; i++) {
                  Weapons[i].Obj.SetActive(_currentWeaponId==Weapons[i].Id);
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
              _currentAnimationClip = GetAnimationClipName(_currentState);
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

        private string GetAnimationClipName(UnitState unitState) {
            switch (unitState) {
                case UnitState.Moving: return "Male_Sword_Walk";
                case UnitState.Idle: return "Male Sword Stance";
                case UnitState.Attacking: return $"Male Attack {Random.Range(1, 4)}";
                case UnitState.Dieing: return "Male Sword Die";
            }

            throw new Exception("Wrong state");
        }
    }
}