using System;
using Game.DI;
using Game.Utils;
using UnityEngine;

namespace Game.Gameplay.Spawning
{
    public class Spawner : DIBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Vector2 relativeSpawnPoint;
        [SerializeField] protected Transform owner;

        public event Action<Spawner> SpawnedEvent;

        private void OnDrawGizmos()
        {
            Color gizmosColor = Color.magenta;
            gizmosColor.a = 0.75f;
            Gizmos.color = gizmosColor;

            Gizmos.DrawSphere(GetSpawnPosition(), 0.1f);
        }

        private Vector2 GetSpawnPosition()
        {
            Vector2 spawnPosition = RotationUtil.GetVectorSimple(0f, transform.eulerAngles.y, 0f, relativeSpawnPoint);
            spawnPosition.x += transform.position.x;
            spawnPosition.y += transform.position.y;

            return spawnPosition;
        }

        public virtual GameObject Spawn(bool ignoreCollisionWithSpawnerOwner = false)
        {
            GameObject instance = Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
            SpawnedEvent?.Invoke(this);

            if (ignoreCollisionWithSpawnerOwner)
            {
                IgnoreCollisionsUtil.IgnoreAllCollisions(owner.gameObject, instance);
            }

            return instance;
        }
    }
}
