using Game.Health;
using UnityEngine;

namespace Game.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Damager))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private bool mayBounceOnGround;
        [SerializeField] private float lifeSpawn = 2f;

        private CircleCollider2D circleCollider2D;
        private int currentBounceCount;
        private float currentBouncelessLifespan;

        private void Awake()
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
            circleCollider2D.isTrigger = false;
        }

        private void Update()
        {
            currentBouncelessLifespan += Time.deltaTime;

            if (currentBouncelessLifespan > lifeSpawn)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            // Immediately destroy on impact if a health component was found on collided object
            if (other.gameObject.GetComponent<HealthComponent>() != null || !mayBounceOnGround)
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
