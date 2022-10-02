using System;
using Rondo.Core;
using Rondo.Unity.Components;
using TenSecCastle.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TenSecCastle.View {
    public class UnitView : MonoBehaviour, IDataDrivenComponent<Unit> {
        [SerializeField] private Animator _playerAnimator;

        private string _currentAnimationClip;
        private float _currentProgress;

        private UnitState _currentState;
        private float _currentUnitHP;
        private float _currentAnimationProgress;

        public IMessenger Messenger { private get; set; }

        public void Sync(Unit unit) {
            if (unit.State != _currentState) {
                _currentState = unit.State;
                _currentAnimationClip = GetAnimationClipName(_currentState);
            }

            _currentProgress = unit.StateProgress;

            if (!Equals(string.IsNullOrEmpty(_currentAnimationClip))) {
                _playerAnimator.speed = 0f;
                _playerAnimator.Play(_currentAnimationClip, 0, _currentProgress);
            }

            /*if (_currentProgress != unit.StateProgress && _currentState == UnitState.Attacking) {
                if (_currentProgress > unit.StateProgress) {
                    _playerAnimator.Play($"Male Attack {Random.Range(1, 4)}");
                }
                _currentProgress = unit.StateProgress;
            }

            if (unit.HitPoints != _currentUnitHP) {
                _currentUnitHP = unit.HitPoints;
                if (_currentUnitHP < unit.MaxHitPoints * 0.25f) {
                    _playerAnimator.Play($"Male Sword Damage Heavy");
                }
                else {
                    _playerAnimator.Play($"Male Sword Damage Light");
                }
            }*/

            //animation.time = math.clamp(unit.StateTime, 0 , animation.length); 
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