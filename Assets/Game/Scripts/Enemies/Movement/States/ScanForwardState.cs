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

            enemyController.NoGroundFoundEvent += OnNoGroundFound;
        }

        protected override void OnExit()
        {
            enemyController.NoGroundFoundEvent -= OnNoGroundFound;
        }

        private void OnNoGroundFound()
        {
            enemyController.StopMovement();

            owner.ToNextState();
        }
    }
}
