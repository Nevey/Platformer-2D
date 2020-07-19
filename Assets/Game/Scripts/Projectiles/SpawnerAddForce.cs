using Game.Spawning;
using UnityEngine;

namespace Game.Projectiles
{
    public abstract class SpawnerAddForce : Spawner
    {
        [Header("Projectile Spawner Settings")]
        [SerializeField] private float spawnForce = 1f;
        [SerializeField] private float maxTorque = 1f;

        protected override void Awake()
        {
            base.Awake();
        }

        protected abstract Vector2 GetSpawnDirectionNormalized();

        public override GameObject Spawn(bool ignoreCollisionWithSpawnerOwner = true)
        {
            GameObject instance = base.Spawn(ignoreCollisionWithSpawnerOwner);

            Rigidbody2D rigidbody2DInstance = instance.GetComponent<Rigidbody2D>();
            rigidbody2DInstance.AddForce(GetSpawnDirectionNormalized() * spawnForce, ForceMode2D.Impulse);
            rigidbody2DInstance.AddTorque(Random.Range(-maxTorque, maxTorque), ForceMode2D.Impulse);

            return instance;
        }
    }
}
