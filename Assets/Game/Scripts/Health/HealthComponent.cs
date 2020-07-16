using System;
using Game.DI;
using UnityEngine;

namespace Game.Health
{
    [RequireComponent(typeof(Collider2D))]
    public class HealthComponent : DIBehaviour
    {
        [SerializeField] private int startHealth = 100;

        [Inject] private DeathController deathController;

        private int currentHealth;

        public int CurrentHealth => currentHealth;

        public event Action<int> HealthUpdatedEvent;
        public event Action<HealthComponent> KilledEvent;

        protected override void Awake()
        {
            base.Awake();

            currentHealth = startHealth;
            UpdateHealth(0);

            deathController.RegisterHealthComponent(this);
        }

        protected override void OnDestroy()
        {
            deathController.UnregisterHealthComponent(this);
        }

        private void CheckForKilled()
        {
            if (currentHealth > 0)
            {
                return;
            }

            currentHealth = 0;
            KilledEvent?.Invoke(this);
        }

        public void UpdateHealth(int amount)
        {
            currentHealth += amount;

            HealthUpdatedEvent?.Invoke(currentHealth);

            CheckForKilled();
        }

        public void DrainAllHealth()
        {
            UpdateHealth(-currentHealth);
        }
    }
}
