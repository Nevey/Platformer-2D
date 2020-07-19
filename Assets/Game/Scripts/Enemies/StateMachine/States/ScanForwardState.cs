using Game.Enemies.Behaviours;
using Game.StateMachines;
using UnityEngine;

namespace Game.Enemies.StateMachines.States
{
    public class ScanForwardState : State
    {
        private EnemyController enemyController;
        private EnemyScanBehaviour enemyScanBehaviour;
        private EnemyTargetBehaviour enemyTargetBehaviour;

        protected override void OnEnter()
        {
            EnemyStateMachine movementStateMachine = (EnemyStateMachine)owner;
            enemyController = movementStateMachine.EnemyController;

            enemyScanBehaviour = enemyController.GetComponent<EnemyScanBehaviour>();
            enemyTargetBehaviour = enemyController.GetComponent<EnemyTargetBehaviour>();

            enemyScanBehaviour.NoGroundFoundEvent += StopMovementAndToNextState;
            enemyScanBehaviour.WallFoundEvent += StopMovementAndToNextState;
            enemyScanBehaviour.PlayerFoundEvent += OnPlayerFound;
        }

        protected override void OnExit()
        {
            enemyScanBehaviour.NoGroundFoundEvent -= StopMovementAndToNextState;
            enemyScanBehaviour.WallFoundEvent -= StopMovementAndToNextState;
            enemyScanBehaviour.PlayerFoundEvent -= OnPlayerFound;
        }

        private void StopMovementAndToNextState()
        {
            enemyController.StopMovement();
            owner.ToNextState();
        }

        private void OnPlayerFound(Transform target)
        {
            enemyTargetBehaviour.SetTarget(target);
            StopMovementAndToNextState();
        }
    }
}
