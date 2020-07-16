using System;
using System.Collections;
using Game.Characters.Movement;
using Game.UserInput;
using UnityEngine;
using CharacterController = Game.Characters.CharacterController;

namespace Game.Enemies
{
    public class EnemyController : CharacterController
    {
        [Header("Enemy Settings")]
        [SerializeField] private float delayInSeconds;

        public event Action MovementStartedEvent;
        public event Action NoGroundFoundEvent;

        private Coroutine routine;

        protected override void Update()
        {
            base.Update();

            ScanForGround();
        }

        private void ScanForGround()
        {
            Vector2 scanDirection = new Vector2(GetMovementVector(moveDirection).x, -1f);

            Debug.DrawRay(transform.position, scanDirection, Color.blue, 0.5f);

            // Find the ground in front of my feet!
            RaycastHit2D hit = Physics2D.Raycast(transform.position, scanDirection, 1f, 1 << 8);

            if (!hit)
            {
                NoGroundFoundEvent?.Invoke();
            }
        }

        private IEnumerator WaitForMovement(MoveDirection moveDirection)
        {
            yield return new WaitForSeconds(delayInSeconds);
            StartMovement(moveDirection);
        }

        public void StartMovement(MoveDirection moveDirection)
        {
            routine = null;
            HandleMovement(ActionState.Start, moveDirection);

            MovementStartedEvent?.Invoke();
        }

        public void StartMovementDelayed(MoveDirection moveDirection)
        {
            routine = StartCoroutine(WaitForMovement(moveDirection));
        }

        public void StopMovement()
        {
            if (routine != null)
            {
                StopCoroutine(routine);
                routine = null;
            }

            HandleMovement(ActionState.Stop, MoveDirection.Right);
        }
    }
}
