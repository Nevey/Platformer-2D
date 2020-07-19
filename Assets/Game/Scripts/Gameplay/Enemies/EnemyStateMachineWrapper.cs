using Game.DI;
using Game.Gameplay.Enemies.Behaviours;
using Game.Gameplay.GameplayFlow;
using UnityEngine;

namespace Game.Gameplay.Enemies
{
    [RequireComponent(typeof(EnemyController))]
    public class EnemyStateMachineWrapper : DIBehaviour
    {
        [Inject] private GameOverController gameOverController;

        private EnemyController enemyController;
        private StateMachines.EnemyStateMachine enemyStateMachine = new StateMachines.EnemyStateMachine();

        protected override void Awake()
        {
            base.Awake();

            gameOverController.WinEvent += OnWin;

            enemyController = GetComponent<EnemyController>();

            enemyStateMachine.SetEnemyController(enemyController);
        }

        protected override void Start()
        {
            base.Start();

            enemyStateMachine.Start();
        }

        protected override void OnDestroy()
        {
            gameOverController.WinEvent -= OnWin;

            enemyStateMachine.Stop();

            base.OnDestroy();
        }

        private void OnWin()
        {
            enemyStateMachine.Stop();

            // TODO: Create central place or better approach where we can stop all enemy behaviours at once
            enemyController.StopMovement();
            enemyController.GetComponent<EnemyFollowBehaviour>().StopFollow();
            enemyController.GetComponent<EnemyAttackBehaviour>().StopAttack();
        }
    }
}
