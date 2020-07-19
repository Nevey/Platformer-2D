using Game.DI;
using Game.Gameplay.Cameras;
using Game.Gameplay.Health;
using Game.Gameplay.Player;
using Game.StateMachines;

namespace Game.Gameplay.GameplayFlow.States
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

            owner.ToNextState();
        }
    }
}
