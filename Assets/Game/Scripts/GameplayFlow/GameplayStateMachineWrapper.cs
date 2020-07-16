using UnityEngine;

namespace Game.GameplayFlow
{
    public class GameplayStateMachineWrapper : MonoBehaviour
    {
        private GameplayStateMachine gameplayStateMachine = new GameplayStateMachine();

        private void Start()
        {
            gameplayStateMachine.Start();
        }

        private void OnDestroy()
        {
            gameplayStateMachine.Stop();
        }
    }
}
