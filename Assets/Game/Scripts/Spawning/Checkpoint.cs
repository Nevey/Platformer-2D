using System;
using UnityEngine;

namespace Game.Spawning
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Checkpoint : Spawner
    {
        [SerializeField] private Sprite inactiveSprite;
        [SerializeField] private Sprite activatedSprite;

        private CircleCollider2D circleCollider2D;
        private SpriteRenderer spriteRenderer;
        private bool isActivated;

        public event Action<Checkpoint> CheckpointActivatedEvent;

        private void Awake()
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
            circleCollider2D.isTrigger = true;

            spriteRenderer = GetComponent<SpriteRenderer>();
            Deactivate();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.tag.Equals("Player") || isActivated)
            {
                return;
            }

            Activate();

            CheckpointActivatedEvent?.Invoke(this);
        }

        private void UpdateSprite()
        {
            spriteRenderer.sprite = isActivated ? activatedSprite : inactiveSprite;
        }

        public void Activate()
        {
            isActivated = true;
            UpdateSprite();
        }

        public void Deactivate()
        {
            isActivated = false;
            UpdateSprite();
        }
    }
}
