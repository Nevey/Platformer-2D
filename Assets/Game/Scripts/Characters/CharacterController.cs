using System;
using Game.Characters.Animations;
using Game.Characters.Movement;
using Game.Characters.Physics;
using Game.DI;
using Game.UserInput;
using UnityEngine;

namespace Game.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(CharacterAnimator))]
    [RequireComponent(typeof(PhysicsMaterialSwapper))]
    public abstract class CharacterController : DIBehaviour
    {
        [SerializeField] private float movementAcceleration = 1f;
        [SerializeField] private float maxMovementSpeed = 10f;
        [SerializeField] private float jumpStrength = 5f;
        [SerializeField] private Transform handContainer;

        private new Rigidbody2D rigidbody2D;
        private CapsuleCollider2D capsuleCollider2D;
        private CharacterAnimator characterAnimator;
        private PhysicsMaterialSwapper physicsMaterialSwapper;

        private ActionState movementActionState = ActionState.Stop;
        private JumpMode jumpMode;
        private Vector2 movementVector;

        protected MoveDirection moveDirection;

        public MoveDirection MoveDirection => moveDirection;

        public event Action<MoveDirection> MoveDirectionUpdatedEvent;

        protected override void Awake()
        {
            base.Awake();

            rigidbody2D = GetComponent<Rigidbody2D>();
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            characterAnimator = GetComponent<CharacterAnimator>();
            physicsMaterialSwapper = GetComponent<PhysicsMaterialSwapper>();

            characterAnimator.SetIdleMode();
        }

        protected override void Start()
        {
            base.Start();
            MoveDirectionUpdatedEvent?.Invoke(moveDirection);
        }

        protected virtual void Update()
        {
            if (movementActionState == ActionState.Stop && jumpMode == JumpMode.None)
            {
                if (rigidbody2D.velocity == Vector2.zero)
                {
                    physicsMaterialSwapper.RevertSwap();
                    characterAnimator.SetIdleMode();
                }

                return;
            }

            if (jumpMode == JumpMode.None)
            {
                physicsMaterialSwapper.RevertSwap();
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
                physicsMaterialSwapper.Swap();
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


                physicsMaterialSwapper.RevertSwap();
                jumpMode = JumpMode.None;

                break;
            }
        }

        protected void HandleMovement(ActionState actionState, MoveDirection newMoveDirection)
        {
            switch (actionState)
            {
                case ActionState.Start:
                    movementVector = newMoveDirection.GetVectorNormalized();
                    break;
                
                case ActionState.Stop:
                    movementVector = Vector2.zero;
                    break;
            }

            movementActionState = actionState;

            if (moveDirection != newMoveDirection)
            {
                Vector2 handContainerPosition = handContainer.localPosition;
                handContainerPosition.x *= -1;
                handContainer.localPosition = handContainerPosition;

                Vector2 handContainerRotation = Vector2.zero;
                handContainerRotation.y = newMoveDirection == MoveDirection.Right ? 0 : 180;
                handContainer.localEulerAngles = handContainerRotation;

                MoveDirectionUpdatedEvent?.Invoke(newMoveDirection);
            }

            moveDirection = newMoveDirection;

            if (jumpMode == JumpMode.None)
            {
                physicsMaterialSwapper.RevertSwap();
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

            physicsMaterialSwapper.Swap();
            characterAnimator.SetJumpMode();
        }
    }
}
