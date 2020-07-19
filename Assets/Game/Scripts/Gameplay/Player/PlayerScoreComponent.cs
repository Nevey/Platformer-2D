using Game.DI;
using Game.Gameplay.Health;
using Game.Gameplay.Scoring;
using UnityEngine;

namespace Game.Gameplay.Player
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
