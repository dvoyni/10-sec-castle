using Rondo.Core;
using Rondo.Unity.Components;
using TenSecCastle.Model;
using UnityEngine;

namespace TenSecCastle.View {
    public class UnitView : MonoBehaviour, IDataDrivenComponent<Unit> {
        public IMessenger Messenger { private get; set; }

        private UnitState _currentState;
        public void Sync(Unit unit) {
            if (unit.State != _currentState) {
                _currentState = unit.State;
                switch (_currentState) {
                    case UnitState.Moving:
                //animation.play();
                        break;
                }
            }
            
            //animation.time = math.clamp(unit.StateTime, 0 , animation.length); 
        }
    }
}