using System;
using System.Collections.Generic;
using Game.Characters.Movement;
using Game.DI;
using UnityEngine;

namespace Game.Health
{
    [Injectable(Singleton = true)]
    public class DeathController
    {
        private readonly List<Health> healthComponents = new List<Health>();

        public event Action<PlayerController> PlayerDiedEvent;

        public void RegisterHealthComponent(Health healthComponent)
        {
            if (healthComponents.Contains(healthComponent))
            {
                return;
            }

            healthComponent.KilledEvent += OnHealthComponentKilled;
            healthComponents.Add(healthComponent);
        }

        public void UnregisterHealthComponent(Health healthComponent)
        {
            if (!healthComponents.Contains(healthComponent))
            {
                return;
            }

            healthComponent.KilledEvent -= OnHealthComponentKilled;
            healthComponents.Remove(healthComponent);
        }

        private void OnHealthComponentKilled(Health healthComponent)
        {
            PlayerController playerController = healthComponent.GetComponent<PlayerController>();

            if (playerController == null)
            {
                return;
            }

            PlayerDiedEvent?.Invoke(playerController);
        }
    }
}
