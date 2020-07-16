﻿using Game.Cameras;
using Game.DI;
using Game.Health;
using Game.Player;
using Game.StateMachines;

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

            owner.ToNextState();
        }
    }
}
