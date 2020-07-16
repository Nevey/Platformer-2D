using UnityEngine;

namespace Game.Health
{
    [RequireComponent(typeof(Collider2D))]
    public class Damager : MonoBehaviour
    {
        [SerializeField] private bool instaKill;
        [SerializeField] private int damage = 10;

        private void OnCollisionEnter2D(Collision2D other)
        {
            HealthComponent otherHealth = other.gameObject.GetComponent<HealthComponent>();

            if (instaKill)
            {
                otherHealth?.DrainAllHealth();
                return;
            }

            otherHealth?.UpdateHealth(-damage);
        }
    }
}
