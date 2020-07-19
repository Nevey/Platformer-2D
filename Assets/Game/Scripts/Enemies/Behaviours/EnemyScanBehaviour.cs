using System;
using Game.Characters.Movement;
using UnityEngine;

namespace Game.Enemies.Behaviours
{
    [RequireComponent(typeof(EnemyController))]
    public class EnemyScanBehaviour : MonoBehaviour
    {
        [SerializeField] private float playerScanRadius = 5f;

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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, playerScanRadius);
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, scanDirection, 1f, LayerMask.GetMask("Ground"));

            // TODO: Make this check more scalable
            if (!hit || hit && (hit.transform.tag.Equals("Trap") || hit.transform.tag.Equals("Trampoline")))
            {
                NoGroundFoundEvent?.Invoke();
            }
        }

        private void ScanForWall()
        {
            Vector2 scanPosition = transform.position;
            scanPosition.y -= 0.35f;

            Vector2 scanDirection = new Vector2(moveDirection.GetVectorNormalized().x, 0f);

            Debug.DrawRay(scanPosition, scanDirection, Color.cyan, 0.5f);

            RaycastHit2D hit = Physics2D.Raycast(scanPosition, scanDirection, 1f, LayerMask.GetMask("Ground"));

            if (hit)
            {
                WallFoundEvent?.Invoke();
            }
        }

        private void ScanForPlayer()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, playerScanRadius, transform.up, 0.01f, LayerMask.GetMask("Characters"));

            for (int i = 0; i < hits.Length; i++)
            {
                // Ignore other enemies
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
