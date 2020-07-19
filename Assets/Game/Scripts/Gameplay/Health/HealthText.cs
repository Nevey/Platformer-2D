using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Gameplay.Health
{
    [RequireComponent(typeof(HealthComponent))]
    public class HealthText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text;
        
        [Header("Animation Settings")]
        [SerializeField] private float fadeInDuration = 0.1f;
        [SerializeField] private float fadeOutDuration = 2f;
        [SerializeField] private float showDuration = 3f;

        private HealthComponent healthComponent;
        private Tween fadeTween;

        private void Awake()
        {
            healthComponent = GetComponent<HealthComponent>();
            healthComponent.HealthUpdatedEvent += OnHealthUpdated;
        }

        private void Start()
        {
            SetText(healthComponent.CurrentHealth);
            FadeOutText();
        }

        private void OnDestroy()
        {
            healthComponent.HealthUpdatedEvent -= OnHealthUpdated;
        }

        private void OnHealthUpdated(int health)
        {
            if (IsInvoking(nameof(FadeOutText)))
            {
                CancelInvoke(nameof(FadeOutText));
            }

            fadeTween?.Kill();
            fadeTween = text.DOFade(1f, fadeInDuration);

            SetText(health);

            Invoke(nameof(FadeOutText), showDuration);
        }

        private void SetText(int health)
        {
            text.SetText(health.ToString());
        }

        private void FadeOutText()
        {
            fadeTween = text.DOFade(0f, fadeOutDuration);
        }
    }
}
