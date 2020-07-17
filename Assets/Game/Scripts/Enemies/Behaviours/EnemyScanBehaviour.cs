using System;
using Game.Characters.Movement;
using UnityEngine;

namespace Game.Enemies.Behaviours
{
    [RequireComponent(typeof(EnemyController))]
    public class EnemyScanBehaviour : MonoBehaviour
    {
        private EnemyController enemyController;
        private MoveDirection moveDirection;

        public event Action NoGroundFoundEvent;
        public event Action WallFoundEvent;
        public event Action<Transform> PlayerFoundEvent;
        public event Action PlayerLostEvent;

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
            ScanForPlayer();
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

        private void ScanForPlayer()
        {
            const float scanDistance = 10f;
            const float boxScanSize = 3f;

            Vector2 scanDirection = new Vector2(moveDirection.GetVectorNormalized().x, 0f);
            Vector2 boxSize = Vector2.one * boxScanSize;

#if UNITY_EDITOR
            Vector2 topRayPosition = transform.position;
            topRayPosition.y += boxScanSize / 2f;
            Vector2 bottomRayPosition = transform.position;
            bottomRayPosition.y -= boxScanSize / 2f;

            Debug.DrawRay(topRayPosition, scanDirection * scanDistance, Color.magenta);
            Debug.DrawRay(bottomRayPosition, scanDirection * scanDistance, Color.magenta);
#endif

            // TODO: Create a frustrum instead of using these very horizontal lines
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0f, scanDirection, scanDistance, LayerMask.GetMask("Characters"));

            // TODO: Think about instead of finding all hits, do a single hit, but make sure the enemies are on a different layer
            // TODO: And/or instead of doing a box cast, do multiple ray casts close to each other vertically
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.tag.Equals("Enemy"))
                {
                    continue;
                }

                // If the player isn't the first thing it's allowed to see, it 's vision is blocked
                if (!hits[i].transform.tag.Equals("Player"))
                {
                    break;
                }

                PlayerFoundEvent?.Invoke(hits[i].transform);

                return;
            }

            PlayerLostEvent?.Invoke();
        }
    }
}
