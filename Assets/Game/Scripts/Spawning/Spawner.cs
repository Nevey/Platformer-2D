using System;
using UnityEngine;

namespace Game.Spawning
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        public event Action<Spawner> SpawnedEvent;

        public void Spawn()
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
            SpawnedEvent?.Invoke(this);
        }
    }
}
