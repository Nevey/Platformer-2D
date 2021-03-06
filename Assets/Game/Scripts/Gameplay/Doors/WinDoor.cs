using Game.DI;
using Game.Gameplay.GameplayFlow;
using UnityEngine;

namespace Game.Gameplay.Doors
{
    public class WinDoor : Door
    {
        [Inject] private GameOverController gameOverController;

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.tag.Equals("Player"))
            {
                return;
            }

            Unlock();
        }

        protected override void Unlock()
        {
            base.Unlock();

            gameOverController.WinGame();
        }

        protected override void OnUnlockAnimationFinished()
        {
            gameOverController.EndGame();
        }
    }
}
