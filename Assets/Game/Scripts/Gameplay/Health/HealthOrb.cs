using Game.Gameplay.Collectibles;
using UnityEngine;

namespace Game.Gameplay.Health
{
    [RequireComponent(typeof(Collectible))]
    public class HealthOrb : MonoBehaviour
    {
        [SerializeField] private int healthValue = 1;

        private Collectible collectible;

        private void Awake()
        {
            collectible = GetComponent<Collectible>();
            collectible.CollectedEvent += OnCollected;
        }

        private void OnDestroy()
        {
            collectible.CollectedEvent -= OnCollected;
        }

        private void OnCollected(Collectible collectible, Transform collecter)
        {
            collectible.CollectedEvent -= OnCollected;

            HealthComponent healthComponent = collecter.GetComponent<HealthComponent>();
            healthComponent?.UpdateHealth(healthValue);
        }
    }
}
