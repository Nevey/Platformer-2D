using Game.Cameras;
using Game.Characters.Movement;
using Game.DI;
using Game.Health;
using Game.StateMachines;
using UnityEngine;

namespace Game.GameplayFlow.States
{
    public class AliveState : State
    {
        [Inject] private DeathController deathController;
        [Inject] private CameraFollowTarget cameraFollowTarget;

        protected override void OnEnter()
        {
            deathController.PlayerDiedEvent += OnPlayerDied;
        }

        protected override void OnExit()
        {
            
        }

        private void OnPlayerDied(PlayerController playerController)
        {
            deathController.PlayerDiedEvent -= OnPlayerDied;

            cameraFollowTarget.UnassignTarget();

            MonoBehaviour.Destroy(playerController.gameObject);

            owner.ToNextState();
        }
    }
}
