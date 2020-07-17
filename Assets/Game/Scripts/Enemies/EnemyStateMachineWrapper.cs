using UnityEngine;

namespace Game.Enemies
{
    [RequireComponent(typeof(EnemyController))]
    public class EnemyStateMachineWrapper : MonoBehaviour
    {
        private EnemyController enemyController;

        private StateMachines.EnemyStateMachine enemyStateMachine = new StateMachines.EnemyStateMachine();

        private void Awake()
        {
            enemyController = GetComponent<EnemyController>();

            enemyStateMachine.SetEnemyController(enemyController);
        }

        private void Start()
        {
            enemyStateMachine.Start();
        }

        private void OnDestroy()
        {
            enemyStateMachine.Stop();
        }
    }
}
