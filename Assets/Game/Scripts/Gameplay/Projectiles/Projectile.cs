using Game.Gameplay.Health;
using UnityEngine;

namespace Game.Gameplay.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Damager))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifeSpan = 1f;

        private CircleCollider2D circleCollider2D;
        private Damager damager;
        private float currentBouncelessLifespan;

        private void Awake()
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
            circleCollider2D.isTrigger = false;

            damager = GetComponent<Damager>();
            damager.DamageDoneEvent += OnDamageDone;
        }

        private void OnDestroy()
        {
            damager.DamageDoneEvent -= OnDamageDone;
        }

        private void Update()
        {
            currentBouncelessLifespan += Time.deltaTime;

            if (currentBouncelessLifespan > lifeSpan)
            {
                Destroy(gameObject);
            }
        }

        private void OnDamageDone(GameObject otherGameObject)
        {
            Destroy(gameObject);
        }
    }
}
