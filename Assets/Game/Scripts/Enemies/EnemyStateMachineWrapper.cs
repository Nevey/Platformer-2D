using Game.Enemies.Attacking;
using Game.Enemies.Movement;
using Game.Health;
using UnityEngine;

namespace Game.Enemies
{
    [RequireComponent(typeof(EnemyController))]
    [RequireComponent(typeof(HealthComponent))]
    public class EnemyStateMachineWrapper : MonoBehaviour
    {
        private EnemyController enemyController;
        private HealthComponent healthComponent;

        private EnemyMovementStateMachine enemyMovementStateMachine = new EnemyMovementStateMachine();
        private EnemyAttackStateMachine enemyAttackStateMachine = new EnemyAttackStateMachine();

        private void Awake()
        {
            enemyController = GetComponent<EnemyController>();
            healthComponent = GetComponent<HealthComponent>();

            enemyMovementStateMachine.SetEnemyController(enemyController);
            enemyMovementStateMachine.StoppedEvent += OnMovementStopped;

            enemyAttackStateMachine.SetEnemyController(enemyController);
            enemyAttackStateMachine.StoppedEvent += OnAttackStopped;
        }

        private void Start()
        {
            enemyMovementStateMachine.Start();
        }

        private void OnDestroy()
        {
            enemyMovementStateMachine.StoppedEvent -= OnMovementStopped;
            enemyAttackStateMachine.StoppedEvent -= OnAttackStopped;

            enemyMovementStateMachine.Stop();
            enemyAttackStateMachine.Stop();
        }

        private void OnMovementStopped()
        {
            // We only stop to attack, or when we're killed
            enemyAttackStateMachine.Start();
        }

        private void OnAttackStopped()
        {
            // We only stop to resume movement, or when we're killed
            enemyMovementStateMachine.Start();
        }
    }
}
