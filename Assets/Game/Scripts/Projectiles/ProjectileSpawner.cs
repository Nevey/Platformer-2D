using Game.Spawning;
using UnityEngine;

namespace Game.Projectiles
{
    public abstract class ProjectileSpawner : Spawner
    {
        [Header("Projectile Spawner Settings")]
        [SerializeField] private float spawnForce;

        protected override void Awake()
        {
            base.Awake();
        }

        protected abstract Vector2 GetSpawnDirectionNormalized();

        public override GameObject Spawn()
        {
            GameObject instance = base.Spawn();

            Projectile projectileInstance = instance.GetComponent<Projectile>();

            if (projectileInstance == null)
            {
                instance.AddComponent<Projectile>();
            }

            Rigidbody2D rigidbody2DInstance = instance.GetComponent<Rigidbody2D>();
            rigidbody2DInstance.AddForce(GetSpawnDirectionNormalized() * spawnForce, ForceMode2D.Impulse);
            rigidbody2DInstance.AddTorque(spawnForce, ForceMode2D.Impulse);

            return instance;
        }
    }
}
