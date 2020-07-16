using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Health
{
    [RequireComponent(typeof(Health))]
    public class HealthText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text;
        
        [Header("Animation Settings")]
        [SerializeField] private float fadeInDuration = 0.1f;
        [SerializeField] private float fadeOutDuration = 2f;
        [SerializeField] private float showDuration = 3f;

        private Health healthComponent;

        private void Awake()
        {
            healthComponent = GetComponent<Health>();
            healthComponent.HealthUpdatedEvent += OnHealthUpdated;

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

            text.DOFade(1f, fadeInDuration);
            text.SetText(health.ToString());

            Invoke(nameof(FadeOutText), showDuration);
        }

        private void FadeOutText()
        {
            text.DOFade(0f, fadeOutDuration);
        }
    }
}
