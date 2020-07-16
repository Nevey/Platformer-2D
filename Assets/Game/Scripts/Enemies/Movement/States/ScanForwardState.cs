using System;
using Game.StateMachines;

namespace Game.Enemies.Movement.States
{
    public class ScanForwardState : State
    {
        private EnemyController enemyController;

        protected override void OnEnter()
        {
            EnemyMovementStateMachine movementStateMachine = (EnemyMovementStateMachine)owner;
            enemyController = movementStateMachine.EnemyController;

            enemyController.NoGroundFoundEvent += StopMovement;
            enemyController.WallFoundEvent += StopMovement;
        }

        protected override void OnExit()
        {
            enemyController.NoGroundFoundEvent -= StopMovement;
            enemyController.WallFoundEvent -= StopMovement;
        }

        private void StopMovement()
        {
            enemyController.StopMovement();
            owner.ToNextState();
        }
    }
}
