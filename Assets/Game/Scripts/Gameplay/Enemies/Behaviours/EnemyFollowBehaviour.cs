using Game.Gameplay.Characters.Movement;
using Game.Utils;
using UnityEngine;

namespace Game.Gameplay.Enemies.Behaviours
{
    [RequireComponent(typeof(EnemyController))]
    [RequireComponent(typeof(EnemyTargetBehaviour))]
    public class EnemyFollowBehaviour : MonoBehaviour
    {
        private EnemyController enemyController;
        private EnemyTargetBehaviour enemyTargetBehaviour;
        private MoveDirection? moveDirection;
        private bool isFollowingTarget;
        private bool isFollowingPaused;

        private void Awake()
        {
            enemyController = GetComponent<EnemyController>();
            enemyTargetBehaviour = GetComponent<EnemyTargetBehaviour>();
        }

        private void Update()
        {
            if (!isFollowingTarget)
            {
                return;
            }

            MoveDirection? newMoveDirection = MoveDirectionExtensions.GetMoveDirection(transform, enemyTargetBehaviour.Target, 0.2f);

            if (newMoveDirection == null || newMoveDirection == moveDirection)
            {
                return;
            }

            Log.Write(newMoveDirection.Value);

            // If following is paused, but our new move direction has changed, we can currently always assume
            // that we want to resume following our target
            if (isFollowingPaused)
            {
                ResumeFollow();
            }

            if (newMoveDirection == null)
            {
                enemyController.StopMovement();
                return;
            }

            enemyController.StartMovement(newMoveDirection.Value);

            moveDirection = newMoveDirection;
        }

        public bool StartFollow()
        {
            if (!enemyTargetBehaviour.HasTarget)
            {
                return false;
            }

            isFollowingTarget = true;

            return true;
        }

        public void StopFollow()
        {
            isFollowingTarget = false;
            isFollowingPaused = false;
        }

        public void PauseFollow()
        {
            enemyController.StopMovement();
            isFollowingPaused = true;
        }

        public void ResumeFollow()
        {
            isFollowingPaused = false;
        }
    }
}
