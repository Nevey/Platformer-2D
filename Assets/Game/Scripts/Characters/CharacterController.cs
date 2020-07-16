using Game.Characters.Animations;
using Game.Characters.Movement;
using Game.DI;
using Game.UserInput;
using UnityEngine;

namespace Game.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(CharacterAnimator))]
    public abstract class CharacterController : DIBehaviour
    {
        [SerializeField] private float movementAcceleration = 1f;
        [SerializeField] private float maxMovementSpeed = 10f;
        [SerializeField] private float jumpStrength = 5f;

        private new Rigidbody2D rigidbody2D;
        private CapsuleCollider2D capsuleCollider2D;
        private CharacterAnimator characterAnimator;

        private ActionState movementActionState = ActionState.Stop;
        private JumpMode jumpMode;
        private Vector2 movementVector;

        protected MoveDirection moveDirection;

        protected override void Awake()
        {
            base.Awake();

            rigidbody2D = GetComponent<Rigidbody2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
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
            // TODO: Check if it's better to use "OnCollisionStay"
            CheckForGroundHit();

            if (movementActionState == ActionState.Stop)
            {
                return;
            }

            float acceleration = movementAcceleration;

            if (jumpMode != JumpMode.None)
            {
                acceleration *= 0.15f;
            }

            Vector2 force = movementVector * acceleration;
            Vector2 velocity = rigidbody2D.velocity + force;
            velocity.x = Mathf.Clamp(velocity.x, -maxMovementSpeed, maxMovementSpeed);
            rigidbody2D.velocity = velocity;
        }

        private void CheckForGroundHit()
        {
            LayerMask mask = LayerMask.GetMask("Ground", "Characters");
            RaycastHit2D[] hits = Physics2D.CapsuleCastAll(transform.position, capsuleCollider2D.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, 0.3f, mask);

            if (hits.Length == 1 && jumpMode == JumpMode.None)
            {
                characterAnimator.SetJumpMode();
                jumpMode = JumpMode.Fall;

                return;
            }

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform == transform)
                {
                    continue;
                }

                Debug.DrawRay(hits[i].point, hits[i].normal, Color.red, 5f);

                float angle = Vector2.Angle(hits[i].normal, Vector2.up);

                if (Mathf.Abs(angle) > 45f)
                {
                    continue;
                }

                jumpMode = JumpMode.None;
                break;
            }
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
            else if (jumpMode == JumpMode.Fall)
            {
                jumpMode = JumpMode.DoubleJump;
            }

            characterAnimator.SetJumpMode();
        }
    }
}
