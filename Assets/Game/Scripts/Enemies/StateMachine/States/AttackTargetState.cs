using Game.Enemies.Behaviours;
using Game.StateMachines;

namespace Game.Enemies.StateMachines.States
{
    public class AttackTargetState : State
    {
        private EnemyController enemyController;
        private EnemyScanBehaviour enemyScanBehaviour;
        private EnemyFollowBehaviour enemyFollowBehaviour;
        private EnemyAttackBehaviour enemyAttackBehaviour;
        private EnemyTargetBehaviour enemyTargetBehaviour;

        protected override void OnEnter()
        {
            EnemyStateMachine movementStateMachine = (EnemyStateMachine)owner;
            enemyController = movementStateMachine.EnemyController;

            enemyScanBehaviour = enemyController.GetComponent<EnemyScanBehaviour>();
            enemyFollowBehaviour = enemyController.GetComponent<EnemyFollowBehaviour>();
            enemyAttackBehaviour = enemyController.GetComponent<EnemyAttackBehaviour>();
            enemyTargetBehaviour = enemyController.GetComponent<EnemyTargetBehaviour>();

            if (enemyFollowBehaviour.StartFollow())
            {
                enemyScanBehaviour.NoGroundFoundEvent += PauseFollowTarget;
                enemyScanBehaviour.WallFoundEvent += PauseFollowTarget;
                enemyScanBehaviour.PlayerLostEvent += OnPlayerLost;

                enemyAttackBehaviour.StartAttack();
            }
            else
            {
                owner.ToNextState();
            }
        }

        protected override void OnExit()
        {
            // Even if no listener was added, this is fine to do with the current version of C#
            enemyScanBehaviour.NoGroundFoundEvent -= PauseFollowTarget;
            enemyScanBehaviour.WallFoundEvent -= PauseFollowTarget;
            enemyScanBehaviour.PlayerLostEvent -= OnPlayerLost;
        }

        private void OnPlayerLost()
        {
            enemyFollowBehaviour.StopFollow();
            enemyAttackBehaviour.StopAttack();
            enemyTargetBehaviour.ClearTarget();

            owner.ToNextState();
        }

        private void PauseFollowTarget()
        {
            // Following will be resumed automatically via EnemyFollowBehaviour
            enemyFollowBehaviour.PauseFollow();
        }
    }
}
