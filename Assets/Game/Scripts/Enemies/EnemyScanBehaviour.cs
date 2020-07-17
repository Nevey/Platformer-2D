using System;
using Game.Characters.Movement;
using UnityEngine;

namespace Game.Enemies
{
    [RequireComponent(typeof(EnemyController))]
    public class EnemyScanBehaviour : MonoBehaviour
    {
        private EnemyController enemyController;
        private MoveDirection moveDirection;

        public event Action NoGroundFoundEvent;
        public event Action WallFoundEvent;

        private void Awake()
        {
            enemyController = GetComponent<EnemyController>();
            enemyController.MoveDirectionUpdatedEvent += OnMovementDirectionUpdated;
        }

        private void OnDestroy()
        {
            enemyController.MoveDirectionUpdatedEvent -= OnMovementDirectionUpdated;
        }

        private void Update()
        {
            ScanForGround();
            ScanForWall();
        }

        private void OnMovementDirectionUpdated(MoveDirection moveDirection)
        {
            this.moveDirection = moveDirection;
        }

        private void ScanForGround()
        {
            Vector2 scanDirection = new Vector2(moveDirection.GetVectorNormalized().x, -1f);

            Debug.DrawRay(transform.position, scanDirection, Color.cyan, 0.5f);

            // Find the ground in front of my feet!
            RaycastHit2D hit = Physics2D.Raycast(transform.position, scanDirection, 1f, 1 << 8);

            // TODO: Make this check more scalable
            if (!hit || hit && (hit.transform.tag.Equals("Trap") || hit.transform.tag.Equals("Trampoline")))
            {
                NoGroundFoundEvent?.Invoke();
            }
        }

        private void ScanForWall()
        {
            Vector2 scanPosition = transform.position;
            scanPosition.y -= 0.25f;

            Vector2 scanDirection = new Vector2(moveDirection.GetVectorNormalized().x, 0f);

            Debug.DrawRay(scanPosition, scanDirection, Color.cyan, 0.5f);

            RaycastHit2D hit = Physics2D.Raycast(scanPosition, scanDirection, 1f, 1 << 8);

            if (hit)
            {
                WallFoundEvent?.Invoke();
            }
        }
    }
}
