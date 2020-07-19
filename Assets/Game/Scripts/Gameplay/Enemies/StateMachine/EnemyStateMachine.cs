using Game.Gameplay.Enemies.StateMachines.States;
using Game.StateMachines;

namespace Game.Gameplay.Enemies.StateMachines
{
    public class EnemyStateMachine : StateMachine
    {
        private EnemyController enemyController;

        public EnemyController EnemyController => enemyController;

        public EnemyStateMachine()
        {
            SetInitialState<StartMovementState>();

            AddTransition<StartMovementState, ScanForwardState>();
            AddTransition<ScanForwardState, AttackTargetState>();
            AddTransition<AttackTargetState, StartMovementState>();
        }

        public void SetEnemyController(EnemyController enemyController)
        {
            this.enemyController = enemyController;
        }
    }
}
