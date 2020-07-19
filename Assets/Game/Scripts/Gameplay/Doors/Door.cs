using DG.Tweening;
using Game.DI;
using Game.Gameplay.Doors.Keys;
using UnityEngine;

namespace Game.Gameplay.Doors
{
    public class Door : DIBehaviour
    {
        [SerializeField] private DoorType doorType;
        [SerializeField] private float unlockAnimationDuration = 2f;
        [SerializeField] private float unlockMovementDistance = 2f;

        [Inject] private KeyStorage keyStorage;

        private bool isUnlocked;

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.tag.Equals("Player"))
            {
                return;
            }

            bool success = keyStorage.UseKeyForDoor(doorType);

            if (success)
            {
                Unlock();
            }
        }

        protected virtual void Unlock()
        {
            if (isUnlocked)
            {
                return;
            }

            isUnlocked = true;

            PlayUnlockAnimation();
        }

        private void PlayUnlockAnimation()
        {
            Vector2 targetPosition = transform.position;
            targetPosition.y -= unlockMovementDistance;

            transform.DOMove(targetPosition, unlockAnimationDuration).OnComplete(OnUnlockAnimationFinished);
        }

        protected virtual void OnUnlockAnimationFinished() { }
    }
}
