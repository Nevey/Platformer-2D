using Game.Enemies.Behaviours;
using Game.StateMachines;

namespace Game.Enemies.StateMachines.States
{
    public class AttackTargetState : State
    {
        private EnemyController enemyController;
        private EnemyScanBehaviour enemyScanBehaviour;
        private EnemyFollowBehaviour enemyFollowBehaviour;

        protected override void OnEnter()
        {
            EnemyStateMachine movementStateMachine = (EnemyStateMachine)owner;
            enemyController = movementStateMachine.EnemyController;

            enemyScanBehaviour = enemyController.GetComponent<EnemyScanBehaviour>();
            enemyFollowBehaviour = enemyController.GetComponent<EnemyFollowBehaviour>();

            if (enemyFollowBehaviour.FollowTarget())
            {
                enemyScanBehaviour.NoGroundFoundEvent += PauseFollowTarget;
                enemyScanBehaviour.WallFoundEvent += PauseFollowTarget;
                enemyScanBehaviour.PlayerLostEvent += OnPlayerLost;
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
            owner.ToNextState();
        }

        private void PauseFollowTarget()
        {
            // Following will be resumed automatically via EnemyFollowBehaviour
            enemyFollowBehaviour.PauseFollow();
        }
    }
}
