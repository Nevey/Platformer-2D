using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Gameplay.Collectibles
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private bool destroyOnCollect = true;
        [SerializeField] private float minDistanceUntilCollect = 0.1f;
        [SerializeField] private float moveTowardsAcceleration = 0.1f;
        [SerializeField] private bool scaleDuringCollect;
        [SerializeField] private Collider2D pickupTrigger;

        private Collider2D[] allColliders;
        private float moveTowardsSpeed;

        private enum CollectMode
        {
            Idle,
            MovingToTarget,
            Collected
        }

        private CollectMode collectMode;
        private Transform target;

        public event Action<Collectible, Transform> CollectedEvent;

        private void Awake()
        {
            allColliders = GetComponentsInChildren<Collider2D>();

            pickupTrigger.isTrigger = true;
        }

        private void Update()
        {
            if (target == null)
            {
                collectMode = CollectMode.Collected;
                return;
            }

            if (collectMode != CollectMode.MovingToTarget)
            {
                return;
            }

            moveTowardsSpeed += moveTowardsAcceleration;
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveTowardsSpeed);

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= minDistanceUntilCollect)
            {
                collectMode = CollectMode.Collected;

                CollectedEvent?.Invoke(this, target);

                if (destroyOnCollect)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.tag.Equals("Player"))
            {
                return;
            }

            pickupTrigger.enabled = false;
            target = other.transform;
            collectMode = CollectMode.MovingToTarget;

            // Remove all colliders
            for (int i = 0; i < allColliders.Length; i++)
            {
                Destroy(allColliders[i]);
            }

            if (scaleDuringCollect)
            {
                transform.DOScale(1.5f, 2f).SetEase(Ease.OutElastic);
            }
        }
    }
}
