using Game.Characters.Animations;
using Game.Characters.Movement;
using Game.DI;
using Game.UserInput;
using UnityEngine;

namespace Game.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class CharacterController : DIBehaviour
    {
        [SerializeField] private float movementAcceleration = 1f;
        [SerializeField] private float maxMovementSpeed = 10f;
        [SerializeField] private float jumpStrength = 5f;

        private new Rigidbody2D rigidbody2D;
        private ActionState movementActionState = ActionState.Stop;
        private JumpMode jumpMode;
        private Vector2 movementVector;
        private CharacterAnimator characterAnimator;

        protected MoveDirection moveDirection;

        protected override void Awake()
        {
            base.Awake();

            rigidbody2D = GetComponent<Rigidbody2D>();
            characterAnimator = GetComponent<CharacterAnimator>();

            characterAnimator.SetIdleMode();
        }

        protected virtual void Update()
        {
            if (movementActionState == ActionState.Stop && jumpMode == JumpMode.None)
            {
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

        protected virtual void FixedUpdate()
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

        protected Vector3 GetMovementVector(MoveDirection moveDirection)
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

        protected void HandleMovement(ActionState actionState, MoveDirection moveDirection)
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
            this.moveDirection = moveDirection;

            if (jumpMode == JumpMode.None)
            {
                characterAnimator.SetWalkMode();
            }
        }

        protected void HandleJump(ActionState actionState)
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
