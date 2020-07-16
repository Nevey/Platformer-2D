﻿using Game.DI;
using Game.Spawning;
using Game.StateMachines;

namespace Game.GameplayFlow.States
{
    public class DeathState : State
    {
        [Inject] private CheckpointController checkpointController;

        protected override void OnEnter()
        {
            checkpointController.SpawnedEvent += OnSpawned;
            checkpointController.SpawnAtCheckpointDelayed(1f);
        }

        protected override void OnExit()
        {
            
        }

        private void OnSpawned()
        {
            checkpointController.SpawnedEvent -= OnSpawned;
            owner.ToNextState();
        }
    }
}
