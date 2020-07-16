using Game.Characters.Movement;
using Game.DI;
using Game.UserInput;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : Characters.CharacterController
    {
        [Inject] private PlayerInputController playerInput;

        protected override void Awake()
        {
            base.Awake();

            playerInput.ActionEvent += OnAction;
        }

        protected override void OnDestroy()
        {
            playerInput.ActionEvent -= OnAction;

            base.OnDestroy();
        }

        private void OnAction(PlayerInputAction playerInputAction, ActionState actionState)
        {
            switch (playerInputAction)
            {
                case PlayerInputAction.MoveLeft:
                    HandleMovement(actionState, MoveDirection.Left);
                    break;

                case PlayerInputAction.MoveRight:
                    HandleMovement(actionState, MoveDirection.Right);
                    break;

                case PlayerInputAction.Jump:
                    HandleJump(actionState);
                    break;
            }
        }
    }
}
