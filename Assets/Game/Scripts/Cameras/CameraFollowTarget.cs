using Game.DI;
using UnityEngine;

namespace Game.Cameras
{
    [Injectable(Singleton = true)]
    public class CameraFollowTarget : MonoBehaviour
    {
        [SerializeField] private float followTime = 0.1f;
        [SerializeField] private float followSpeed = 1f;
        [SerializeField] private float distanceZ = 10f;

        private Transform target;
        private Vector2 followVelocity;
        private Vector2 targetPosition;
        private Vector2 currentPosition;

        private void Awake()
        {
            currentPosition = targetPosition = transform.position;
        }

        private void Update()
        {
            if (target != null)
            {
                targetPosition = target.position;
            }

            currentPosition = Vector2.SmoothDamp(currentPosition, targetPosition, ref followVelocity, followTime, followSpeed);
            transform.position = new Vector3(currentPosition.x, currentPosition.y, -distanceZ);
        }

        public void AssignTarget(Transform target)
        {
            this.target = target;
        }

        public void UnassignTarget()
        {
            target = null;
        }
    }
}
