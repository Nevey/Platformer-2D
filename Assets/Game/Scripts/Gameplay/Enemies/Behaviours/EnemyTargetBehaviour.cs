using UnityEngine;

namespace Game.Gameplay.Enemies.Behaviours
{
    public class EnemyTargetBehaviour : MonoBehaviour
    {
        private Transform target;

        public bool HasTarget => target != null;
        public Transform Target => target;

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void ClearTarget()
        {
            target = null;
        }
    }
}
