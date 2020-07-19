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

        private Coroutine routine;

        private IEnumerator WaitBeforeStartMovement(MoveDirection moveDirection)
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
            routine = StartCoroutine(WaitBeforeStartMovement(moveDirection));
        }

        public void StopMovement()
        {
            if (routine != null)
            {
                StopCoroutine(routine);
                routine = null;
            }

            HandleMovement(ActionState.Stop, MoveDirection);
        }
    }
}
