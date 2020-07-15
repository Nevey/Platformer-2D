using Game.Characters.Animations;
using Game.DI;
using Game.UserInput;
using UnityEngine;

namespace Game.Characters.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : DIBehaviour
    {
        [SerializeField] private float movementAcceleration = 1f;
        [SerializeField] private float maxMovementSpeed = 10f;
        [SerializeField] private float jumpStrength = 5f;

        [Inject] private PlayerInputController playerInput;

        private new Rigidbody2D rigidbody2D;
        private ActionState movementActionState;
        private JumpMode jumpMode;
        private Vector2 movementVector;
        private CharacterAnimator characterAnimator;

        protected override void Awake()
        {
            base.Awake();

            rigidbody2D = GetComponent<Rigidbody2D>();
            characterAnimator = GetComponent<CharacterAnimator>();

            playerInput.ActionEvent += OnAction;

            characterAnimator.SetIdleMode();
        }

        protected override void OnDestroy()
        {
            playerInput.ActionEvent -= OnAction;

            base.OnDestroy();
        }

        private void Update()
        {
            if (movementActionState == ActionState.Stop && jumpMode == JumpMode.None)
            {
                // TODO: Do this only once..
                if (rigidbody2D.velocity == Vector2.zero)
                {
                    characterAnimator.SetIdleMode();
                }

                return;
            }

            if (jumpMode == JumpMode.None)
            {
                characterAnimator.SetWalkMode();
            }

            characterAnimator.SetWalkSpeed(rigidbody2D.velocity.x);
            characterAnimator.SetJumpSpeed(rigidbody2D.velocity.y);
        }

        private void FixedUpdate()
        {
            if (movementActionState == ActionState.Stop)
            {
                return;
            }

            Vector2 force = movementVector * movementAcceleration;
            Vector2 velocity = rigidbody2D.velocity + force;
            velocity.x = Mathf.Clamp(velocity.x, -maxMovementSpeed, maxMovementSpeed);
            rigidbody2D.velocity = velocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            // Check if we're colliding with ground layer
            if (other.gameObject.layer != 8)
            {
                return;
            }

            jumpMode = JumpMode.None;
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

        private void HandleMovement(ActionState actionState, MoveDirection moveDirection)
        {
            switch (actionState)
            {
                case ActionState.Start:
                    movementVector = GetMovementVector(moveDirection);
                    break;
                
                case ActionState.Stop:
                    movementVector = Vector2.zero;
                    break;
            }

            movementActionState = actionState;

            if (jumpMode == JumpMode.None)
            {
                characterAnimator.SetWalkMode();
            }
        }

        private Vector3 GetMovementVector(MoveDirection moveDirection)
        {
            switch (moveDirection)
            {
                case MoveDirection.Left:
                    return Vector2.left;
                
                case MoveDirection.Right:
                    return Vector2.right;
            }

            return Vector2.zero;
        }

        private void HandleJump(ActionState actionState)
        {
            if (actionState == ActionState.Stop || jumpMode == JumpMode.DoubleJump)
            {
                return;
            }

            rigidbody2D.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);

            if ((int)jumpMode < (int)JumpMode.DoubleJump)
            {
                jumpMode++;
            }

            characterAnimator.SetJumpMode();
        }
    }
}
