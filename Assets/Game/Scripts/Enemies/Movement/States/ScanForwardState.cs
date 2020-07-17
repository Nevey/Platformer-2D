using Game.StateMachines;

namespace Game.Enemies.Movement.States
{
    public class ScanForwardState : State
    {
        private EnemyController enemyController;
        private EnemyScanBehaviour enemyScanBehaviour;

        protected override void OnEnter()
        {
            EnemyMovementStateMachine movementStateMachine = (EnemyMovementStateMachine)owner;
            enemyController = movementStateMachine.EnemyController;

            enemyScanBehaviour = enemyController.GetComponent<EnemyScanBehaviour>();

            enemyScanBehaviour.NoGroundFoundEvent += StopMovement;
            enemyScanBehaviour.WallFoundEvent += StopMovement;
        }

        protected override void OnExit()
        {
            enemyScanBehaviour.NoGroundFoundEvent -= StopMovement;
            enemyScanBehaviour.WallFoundEvent -= StopMovement;
        }

        private void StopMovement()
        {
            enemyController.StopMovement();
            owner.ToNextState();
        }
    }
}
