using System;
using System.Collections.Generic;
using Game.DI;
using Game.Player;
using UnityEngine;

namespace Game.Health
{
    [Injectable(Singleton = true)]
    public class DeathController
    {
        private readonly List<HealthComponent> healthComponents = new List<HealthComponent>();

        public event Action<PlayerController> PlayerDiedEvent;

        public void RegisterHealthComponent(HealthComponent healthComponent)
        {
            if (healthComponents.Contains(healthComponent))
            {
                return;
            }

            healthComponent.KilledEvent += OnHealthComponentKilled;
            healthComponents.Add(healthComponent);
        }

        public void UnregisterHealthComponent(HealthComponent healthComponent)
        {
            if (!healthComponents.Contains(healthComponent))
            {
                return;
            }

            healthComponent.KilledEvent -= OnHealthComponentKilled;
            healthComponents.Remove(healthComponent);
        }

        private void OnHealthComponentKilled(HealthComponent healthComponent)
        {
            MonoBehaviour.Destroy(healthComponent.gameObject);

            PlayerController playerController = healthComponent.GetComponent<PlayerController>();

            if (playerController == null)
            {
                return;
            }

            PlayerDiedEvent?.Invoke(playerController);
        }
    }
}
