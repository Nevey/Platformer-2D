using System;
using System.Linq;
using Game.DI;
using Game.Gameplay.Characters.Animations;
using Game.Gameplay.Characters.Movement;
using Game.Gameplay.Characters.Physics;
using Game.Gameplay.UserInput;
using UnityEngine;

namespace Game.Gameplay.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(CharacterAnimator))]
    [RequireComponent(typeof(PhysicsMaterialSwapper))]
    public abstract class CharacterController : DIBehaviour
    {
        [SerializeField] private float movementAcceleration = 1f;
        [SerializeField] private float maxMovementSpeed = 10f;
        [SerializeField] private float maxJumpSpeed = 10f;
        [SerializeField] private float jumpStrength = 5f;
        [SerializeField] private float doubleJumpStrength = 2.5f;
        [SerializeField] private float minDoubleJumpInterval = 5f;
        [Tooltip("Offset for character to stop walking into a wall")]
        [SerializeField] private float wallStopOffset = 0.2f;
        [SerializeField] private Transform handContainer;

        protected new Rigidbody2D rigidbody2D;
        private CapsuleCollider2D capsuleCollider2D;
        protected CharacterAnimator characterAnimator;
        private PhysicsMaterialSwapper physicsMaterialSwapper;

        private ActionState movementActionState = ActionState.Stop;
        private JumpMode jumpMode;
        private Vector2 movementVector;
        private float currentDoubleJumpInterval;
        private float wallStopRayDistance;

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

            wallStopRayDistance = (capsuleCollider2D.size.x / 2f) + wallStopOffset;
        }

        protected override void Start()
        {
            base.Start();
            MoveDirectionUpdatedEvent?.Invoke(moveDirection);
        }

        protected virtual void Update()
        {
            CheckForGroundHit();

            currentDoubleJumpInterval += Time.deltaTime;

            characterAnimator.SetWalkSpeed(rigidbody2D.velocity.x);
            characterAnimator.SetJumpSpeed(rigidbody2D.velocity.y);

            if (movementActionState == ActionState.Stop
                && jumpMode == JumpMode.None
                && rigidbody2D.velocity == Vector2.zero)
            {
                physicsMaterialSwapper.RevertSwap();
                characterAnimator.SetIdleMode();

                return;
            }

            if (jumpMode == JumpMode.None)
            {
                physicsMaterialSwapper.RevertSwap();
                characterAnimator.SetWalkMode();
            }
        }

        protected virtual void FixedUpdate()
        {
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

            // Check if we're running into a wall in given direction
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, movementVector, wallStopRayDistance, LayerMask.GetMask("Characters", "Ground"));
            hits = FilterTriggerColliders(hits);

            // Will always include self
            if (hits.Length > 1)
            {
                Debug.DrawRay(transform.position, movementVector * wallStopRayDistance, Color.magenta, 1f);
                velocity.x = 0f;
            }

            velocity.x = Mathf.Clamp(velocity.x, -maxMovementSpeed, maxMovementSpeed);

            rigidbody2D.velocity = velocity;
        }

        private RaycastHit2D[] FilterTriggerColliders(RaycastHit2D[] hits)
        {
            return hits.Where(x => x.collider.isTrigger == false).ToArray();
        }

        private void CheckForGroundHit()
        {
            LayerMask mask = LayerMask.GetMask("Ground", "Characters");
            RaycastHit2D[] hits = Physics2D.CapsuleCastAll(transform.position, capsuleCollider2D.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, 0.03f, mask);
            hits = FilterTriggerColliders(hits);

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

                Debug.DrawRay(hits[i].point, hits[i].normal, Color.red, 0.2f);

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

            // Swap our whatever we're holding to point in the correct direction
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

            if (movementVector == Vector2.zero && jumpMode == JumpMode.None)
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

            if (currentDoubleJumpInterval < minDoubleJumpInterval && jumpMode != JumpMode.None)
            {
                return;
            }

            float strength = 0f;

            switch (jumpMode)
            {
                case JumpMode.None:
                    strength = jumpStrength;
                    jumpMode = JumpMode.SingleJump;
                    break;

                case JumpMode.Fall:
                case JumpMode.SingleJump:
                    strength = doubleJumpStrength;
                    jumpMode = JumpMode.DoubleJump;
                    break;
                
                case JumpMode.DoubleJump:
                    // Do nothing
                    break;
            }

            rigidbody2D.AddForce(Vector2.up * strength, ForceMode2D.Impulse);

            currentDoubleJumpInterval = 0f;

            physicsMaterialSwapper.Swap();
            characterAnimator.SetJumpMode();
        }
    }
}
