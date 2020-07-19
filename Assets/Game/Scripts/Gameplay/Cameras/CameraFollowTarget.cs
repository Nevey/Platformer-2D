using Game.DI;
using UnityEngine;

namespace Game.Gameplay.Cameras
{
    [RequireComponent(typeof(Camera))]
    [Injectable(Singleton = true)]
    public class CameraFollowTarget : MonoBehaviour
    {
        [SerializeField] private float followTime = 0.1f;
        [SerializeField] private float followSpeed = 1f;
        [SerializeField] private float distanceZ = 10f;

        private new Camera camera;
        private Transform target;
        private Vector2 followVelocity;
        private Vector2 targetPosition;
        private Vector2 currentPosition;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            camera = GetComponent<Camera>();
            DisableCamera();

            // TODO: To avoid the need to do this, wrap up injection layer implementation
            UnassignTarget();

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

        public void EnableCamera()
        {
            camera.enabled = true;
        }

        public void DisableCamera()
        {
            camera.enabled = false;
        }
    }
}
