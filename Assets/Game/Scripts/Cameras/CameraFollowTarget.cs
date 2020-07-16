using UnityEngine;

namespace Game.Cameras
{
    public class CameraFollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float followTime = 0.1f;
        [SerializeField] private float followSpeed = 1f;
        [SerializeField] private float distanceZ = 10f;

        private Vector2 followVelocity;
        private Vector2 currentPosition;

        private void Update()
        {
            currentPosition = Vector2.SmoothDamp(currentPosition, target.position, ref followVelocity, followTime, followSpeed);
            transform.position = new Vector3(currentPosition.x, currentPosition.y, -distanceZ);
        }
    }
}
