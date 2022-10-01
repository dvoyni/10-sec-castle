using Rondo.Core;
using Rondo.Unity.Components;
using TenSecCastle.Model;
using UnityEngine;

namespace TenSecCastle.View {
    public class UnitView : MonoBehaviour, IDataDrivenComponent<Unit> {
        
        [SerializeField] private Animator _playerAnimator;
        
        public IMessenger Messenger { private get; set; }

        private UnitState _currentState;
        private float _currentUnitHP;
        public void Sync(Unit unit) {
           
            if (unit.State != _currentState) {
                _currentState = unit.State;
                switch (_currentState) {
                    case UnitState.Moving:
                        _playerAnimator.Play("Male_Sword_Walk");
                        _playerAnimator.speed = unit.MoveSpeed*0.85f;
                        break;
                    case UnitState.Idle:
                        _playerAnimator.Play("Male Sword Stance");
                        _playerAnimator.speed = 1;
                        break;
                    case UnitState.Attacking:
                        _playerAnimator.Play($"Male Attack {Random.Range(1,4)}");
                        _playerAnimator.speed = unit.AttackSpeed;
                        break;
                    case UnitState.Dieing:
                        _playerAnimator.Play($"Male Sword Die");
                        _playerAnimator.speed = 1;
                        break;
                }
            }
            
            /*if (unit.HitPoints != _currentUnitHP) {
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
    }
}