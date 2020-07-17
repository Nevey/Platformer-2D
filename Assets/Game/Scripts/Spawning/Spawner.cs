using System;
using Game.DI;
using Game.Utils;
using UnityEngine;

namespace Game.Spawning
{
    public class Spawner : DIBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Vector2 relativeSpawnPoint;

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
            Vector3 p = RotationUtil.GetVectorSimple(0f, transform.eulerAngles.y, 0f, relativeSpawnPoint);
            p.x += transform.position.x;
            p.y += transform.position.y;
            p.z = 0f;

            return p;
        }

        public virtual GameObject Spawn()
        {
            GameObject instance = Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
            SpawnedEvent?.Invoke(this);

            return instance;
        }
    }
}
