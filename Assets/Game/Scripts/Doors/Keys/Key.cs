using Game.Collectibles;
using Game.DI;
using UnityEngine;

namespace Game.Doors.Keys
{
    [RequireComponent(typeof(Collectible))]
    public class Key : DIBehaviour
    {
        [SerializeField] private DoorType doorType;

        [Inject] private KeyStorage keyStorage;

        private Collectible collectible;

        public DoorType DoorType => doorType;

        protected override void Awake()
        {
            base.Awake();

            collectible = GetComponent<Collectible>();
            collectible.CollectedEvent += OnCollected;
        }

        protected override void OnDestroy()
        {
            collectible.CollectedEvent -= OnCollected;

            base.OnDestroy();
        }

        private void OnCollected(Collectible collectible)
        {
            collectible.CollectedEvent -= OnCollected;

            keyStorage.CollectKey(this);
        }
    }
}
