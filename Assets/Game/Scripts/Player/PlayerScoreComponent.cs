using Game.DI;
using Game.Health;
using Game.Scoring;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(HealthComponent))]
    public class PlayerScoreComponent : DIBehaviour
    {
        [Inject] private ScoreController scoreController;

        private HealthComponent healthComponent;

        protected override void Awake()
        {
            base.Awake();

            healthComponent = GetComponent<HealthComponent>();
            healthComponent.HealthUpdatedEvent += OnHealthUpdated;
        }

        protected override void OnDestroy()
        {
            healthComponent.HealthUpdatedEvent -= OnHealthUpdated;

            base.OnDestroy();
        }

        private void OnHealthUpdated(int health)
        {
            scoreController.SetScore(health);
        }
    }
}
