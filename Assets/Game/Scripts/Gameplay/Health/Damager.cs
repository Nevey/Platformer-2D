using System;
using UnityEngine;

namespace Game.Gameplay.Health
{
    [RequireComponent(typeof(Collider2D))]
    public class Damager : MonoBehaviour
    {
        [SerializeField] private bool instaKill;
        [SerializeField] private int damage = 10;

        public event Action<GameObject> DamageDoneEvent;

        private void OnCollisionEnter2D(Collision2D other)
        {
            HealthComponent otherHealth = other.gameObject.GetComponent<HealthComponent>();

            if (otherHealth != null)
            {
                DamageDoneEvent?.Invoke(other.gameObject);
            }

            if (instaKill)
            {
                otherHealth?.DrainAllHealth();
                return;
            }

            otherHealth?.UpdateHealth(-damage);
        }
    }
}
