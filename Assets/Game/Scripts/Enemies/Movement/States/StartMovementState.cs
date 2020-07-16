using Game.Characters.Movement;
using Game.StateMachines;

namespace Game.Enemies.Movement.States
{
    public class StartMovementState : State
    {
        private EnemyController enemyController;
        private MoveDirection currentMoveDirection;

        protected override void OnEnter()
        {
            EnemyMovementStateMachine movementStateMachine = (EnemyMovementStateMachine)owner;
            enemyController = movementStateMachine.EnemyController;

            RevertMoveDirection();
            StartMovement();
        }

        protected override void OnExit()
        {
            enemyController.MovementStartedEvent -= OnMovementStarted;
        }

        private void RevertMoveDirection()
        {
            currentMoveDirection = currentMoveDirection == MoveDirection.Left ? MoveDirection.Right : MoveDirection.Left;
        }

        private void StartMovement()
        {
            enemyController.MovementStartedEvent += OnMovementStarted;
            enemyController.StartMovementDelayed(currentMoveDirection);
        }

        private void OnMovementStarted()
        {
            owner.ToNextState();
        }
    }
}
