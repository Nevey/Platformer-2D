using Game.Projectiles;
using UnityEngine;

namespace Game.Health
{
    [RequireComponent(typeof(HealthComponent))]
    public class HealthOrbSpawner : SpawnerAddForce
    {
        [SerializeField] private int spawnAmount = 7;
        [SerializeField] private float minVerticalSpawnForce = 1f;
        [SerializeField] private float maxVerticlaSpawnForce = 2f;
        [SerializeField] private float maxHorizontalSpawnforce = 2f;

        private HealthComponent healthComponent;

        protected override void Awake()
        {
            base.Awake();

            healthComponent = GetComponent<HealthComponent>();
            healthComponent.KilledEvent += OnKilled;
        }

        protected override void OnDestroy()
        {
            healthComponent.KilledEvent -= OnKilled;
            base.OnDestroy();
        }

        private void OnKilled(HealthComponent obj)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                Spawn(false);
            }
        }

        protected override Vector2 GetSpawnDirectionNormalized()
        {
            return new Vector2(
                Random.Range(-maxHorizontalSpawnforce, maxHorizontalSpawnforce),
                Random.Range(minVerticalSpawnForce, maxVerticlaSpawnForce)).normalized;
        }
    }
}
