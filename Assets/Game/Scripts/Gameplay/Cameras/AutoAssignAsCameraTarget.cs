using Game.DI;

namespace Game.Gameplay.Cameras
{
    public class AutoAssignAsCameraTarget : DIBehaviour
    {
        [Inject] private CameraFollowTarget cameraFollowTarget;

        protected override void Awake()
        {
            base.Awake();

            cameraFollowTarget.AssignTarget(transform);
        }
    }
}
