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

            MoveDirection? newMoveDirection = MoveDirectionExtensions.GetMoveDirection(transform, enemyTargetBehaviour.Target);

            if (newMoveDirection == null)
            {
                enemyController.StopMovement();
                return;
            }

            if (newMoveDirection == null || (newMoveDirection == moveDirection && newMoveDirection == enemyController.MoveDirection))
            {
                const float deadZone = 2.5f;
                float distance = Vector3.Distance(transform.position, enemyTargetBehaviour.Target.position);

                if (distance < deadZone)
                {
                    enemyController.StopMovement();
                    return;
                }
            }

            Log.Write(newMoveDirection.Value);

            // If following is paused, but our new move direction has changed, we can currently always assume
            // that we want to resume following our target
            if (isFollowingPaused && newMoveDirection != enemyController.MoveDirection)
            {
                ResumeFollow();
            }

            if (!isFollowingPaused)
            {
                enemyController.StartMovement(newMoveDirection.Value);
            }

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
