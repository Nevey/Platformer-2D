using System;
using Game.DI;
using UnityEngine;

namespace Game.Spawning
{
    [Injectable(Singleton = true)]
    public class CheckpointController : MonoBehaviour
    {
        [SerializeField] private Checkpoint initialActiveCheckpoint;

        private Checkpoint[] checkpoints;
        private Checkpoint currentActiveCheckpoint;

        public event Action SpawnedEvent;

        private void Awake()
        {
            checkpoints = FindObjectsOfType<Checkpoint>();

            for (int i = 0; i < checkpoints.Length; i++)
            {
                checkpoints[i].SpawnedEvent += OnSpawned;
                checkpoints[i].CheckpointActivatedEvent += OnCheckpointActivated;
            }

            currentActiveCheckpoint = initialActiveCheckpoint;
        }

        private void Start()
        {
            currentActiveCheckpoint.Activate();
        }

        private void OnDestroy()
        {
            for (int i = 0; i < checkpoints.Length; i++)
            {
                checkpoints[i].SpawnedEvent -= OnSpawned;
                checkpoints[i].CheckpointActivatedEvent -= OnCheckpointActivated;
            }
        }

        private void OnSpawned(Spawner spawner)
        {
            SpawnedEvent?.Invoke();
        }

        private void OnCheckpointActivated(Checkpoint checkpoint)
        {
            currentActiveCheckpoint.Deactivate();
            currentActiveCheckpoint = checkpoint;
        }

        public void SpawnAtCheckpoint()
        {
            currentActiveCheckpoint.Spawn();
        }

        public void SpawnAtCheckpointDelayed(float delayInSeconds)
        {
            Invoke(nameof(SpawnAtCheckpoint), delayInSeconds);
        }
    }
}
