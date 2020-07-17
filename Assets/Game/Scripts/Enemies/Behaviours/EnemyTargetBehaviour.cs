using UnityEngine;

namespace Game.Enemies.Behaviours
{
    [RequireComponent(typeof(EnemyScanBehaviour))]
    public class EnemyTargetBehaviour : MonoBehaviour
    {
        private EnemyScanBehaviour enemyScanBehaviour;
        private Transform target;

        public bool HasTarget => target != null;
        public Transform Target => target;

        private void Awake()
        {
            enemyScanBehaviour = GetComponent<EnemyScanBehaviour>();
            enemyScanBehaviour.PlayerFoundEvent += OnPlayerFound;
        }

        private void OnDestroy()
        {
            enemyScanBehaviour.PlayerFoundEvent -= OnPlayerFound;
        }

        private void OnPlayerFound(Transform target)
        {
            this.target = target;
        }
    }
}
