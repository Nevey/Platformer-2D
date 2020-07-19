using System;
using System.Collections.Generic;
using Game.DI;
using Game.Utils;
using UnityEngine;

namespace Game.Spawning
{
    [Injectable(Singleton = true)]
    public class CheckpointController : MonoBehaviour
    {
        private readonly List<Checkpoint> checkpoints = new List<Checkpoint>();
        private Checkpoint currentActiveCheckpoint;

        public event Action SpawnedEvent;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            ClearAllCheckpoints();
        }

        private void ClearAllCheckpoints()
        {
            if (checkpoints == null)
            {
                return;
            }

            for (int i = 0; i < checkpoints.Count; i++)
            {
                checkpoints[i].SpawnedEvent -= OnSpawned;
                checkpoints[i].CheckpointActivatedEvent -= OnCheckpointActivated;
            }

            checkpoints.Clear();
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

        public void RegisterCheckpoint(Checkpoint checkpoint)
        {
            if (checkpoints.Contains(checkpoint))
            {
                return;
            }

            checkpoint.SpawnedEvent += OnSpawned;
            checkpoint.CheckpointActivatedEvent += OnCheckpointActivated;

            if (checkpoint.IsInitialCheckpoint)
            {
                if (currentActiveCheckpoint != null)
                {
                    throw Log.Exception($"Multiple initial checkpoints found: <b>{checkpoint.name}</b> -- Current initial checkpoint: {currentActiveCheckpoint.name}");
                }

                currentActiveCheckpoint = checkpoint;
                currentActiveCheckpoint.Activate();
            }

            checkpoints.Add(checkpoint);
        }

        public void UnregisterCheckpoint(Checkpoint checkpoint)
        {
            if (!checkpoints.Contains(checkpoint))
            {
                return;
            }

            checkpoint.SpawnedEvent -= OnSpawned;
            checkpoint.CheckpointActivatedEvent -= OnCheckpointActivated;

            checkpoints.Remove(checkpoint);
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
