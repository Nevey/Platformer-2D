using Game.Characters.Movement;
using Game.DI;
using Game.GameplayFlow;
using Game.UserInput;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : Characters.CharacterController
    {
        [Inject] private PlayerInputController playerInput;
        [Inject] private GameOverController gameOverController;

        private bool isDisabled;

        protected override void Awake()
        {
            base.Awake();

            playerInput.ActionEvent += OnAction;
            gameOverController.WinEvent += OnWin;
        }

        protected override void OnDestroy()
        {
            playerInput.ActionEvent -= OnAction;
            gameOverController.WinEvent -= OnWin;

            base.OnDestroy();
        }

        private void OnAction(PlayerInputAction playerInputAction, ActionState actionState)
        {
            if (isDisabled)
            {
                return;
            }

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

        private void OnWin()
        {
            HandleMovement(ActionState.Stop, MoveDirection);
            isDisabled = true;
        }
    }
}
