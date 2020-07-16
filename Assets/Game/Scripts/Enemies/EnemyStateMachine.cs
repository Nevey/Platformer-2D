using Game.StateMachines;

namespace Game.Enemies
{
    public class EnemyStateMachine : StateMachine
    {
        private EnemyController enemyController;

        public EnemyController EnemyController => enemyController;

        public void SetEnemyController(EnemyController enemyController)
        {
            this.enemyController = enemyController;
        }
    }
}
