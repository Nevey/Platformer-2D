using Game.Characters.Movement;
using Game.StateMachines;

namespace Game.Enemies.StateMachines.States
{
    public class StartMovementState : State
    {
        private EnemyController enemyController;
        private MoveDirection currentMoveDirection;

        protected override void OnEnter()
        {
            EnemyStateMachine movementStateMachine = (EnemyStateMachine)owner;
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
