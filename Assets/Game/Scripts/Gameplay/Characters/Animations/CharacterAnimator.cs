using Game.Animations;
using UnityEngine;

namespace Game.Gameplay.Characters.Animations
{
    public class CharacterAnimator : SpritesheetAnimator
    {
        [Header("Idle Animation")]
        [SerializeField] private Sprite[] idleSprites;

        [Header("Walk Animation")]
        [SerializeField] private float walkSpeedMultiplier = 0.1f;
        [SerializeField] private Sprite[] walkSprites;

        [Header("Jump Animation")]
        [SerializeField] private Sprite[] jumpSprites;

        private float walkSpeed;
        private float jumpSpeed;

        private enum AnimationMode
        {
            Idle,
            Walk,
            Jump
        }

        private AnimationMode animationMode;

        protected override void Awake()
        {
            base.Awake();

            SetIdleMode();
        }

        protected override void Update()
        {
            switch (animationMode)
            {
                case AnimationMode.Idle:
                    break;
                
                case AnimationMode.Walk:

                    if (walkSpeed == 0f)
                    {
                        SetIdleMode();
                    }
                    else
                    {
                        SetWalkMode();
                        SetAnimationInterval(Mathf.Abs(walkSpeed * walkSpeedMultiplier));
                        SetIsFlippedHorizontally(walkSpeed < 0);
                    }
                    break;
                
                case AnimationMode.Jump:
                    // TODO: Set sprites based on jump velocity
                    SetIsFlippedHorizontally(walkSpeed < 0);
                    break;
            }

            base.Update();
        }

        private void SetAnimationMode(AnimationMode animationMode)
        {
            this.animationMode = animationMode;
        }

        public void SetIdleMode()
        {
            if (animationMode == AnimationMode.Idle)
            {
                return;
            }

            SetAnimationMode(AnimationMode.Idle);
            SetSprites(idleSprites);
            SetDefaultAnimationInterval();
        }

        public void SetWalkMode()
        {
            if (animationMode == AnimationMode.Walk)
            {
                return;
            }

            SetAnimationMode(AnimationMode.Walk);
            SetSprites(walkSprites);
        }

        public void SetJumpMode()
        {
            if (animationMode == AnimationMode.Jump)
            {
                return;
            }

            SetAnimationMode(AnimationMode.Jump);
            SetSprites(jumpSprites);
        }

        public void SetWalkSpeed(float speed)
        {
            walkSpeed = speed;
        }

        public void SetJumpSpeed(float speed)
        {
            jumpSpeed = speed;
        }
    }
}
