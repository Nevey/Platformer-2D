using Game.Utils;
using UnityEngine;

namespace Game.Characters.Animations
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class SpritesheetAnimation : MonoBehaviour
    {
        [SerializeField] private float defaultAnimationInterval = 0.1f;

        private float animationInterval;
        private Sprite[] sprites;
        private SpriteRenderer spriteRenderer;
        private float currentInterval;
        private int currentSpriteIndex;
        private bool isFlippedHorizontally;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            SetDefaultAnimationInterval();
        }

        protected virtual void Update()
        {
            currentInterval += Time.deltaTime;

            if (currentInterval >= animationInterval)
            {
                currentInterval -= animationInterval;
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            currentSpriteIndex++;

            if (currentSpriteIndex >= sprites.Length)
            {
                currentSpriteIndex = 0;
            }
            
            spriteRenderer.sprite = sprites[currentSpriteIndex];
            spriteRenderer.flipX = isFlippedHorizontally;
        }

        protected void SetSprites(Sprite[] sprites)
        {
            this.sprites = sprites;
        }

        protected void SetIsFlippedHorizontally(bool isFlipped)
        {
            isFlippedHorizontally = isFlipped;
        }

        protected void SetAnimationInterval(float interval)
        {
            animationInterval = interval;
        }

        protected void SetDefaultAnimationInterval()
        {
            animationInterval = defaultAnimationInterval;
        }
    }
}
