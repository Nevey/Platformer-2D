using System;
using Game.DI;

namespace Game.Gameplay.GameplayFlow
{
    [Injectable(Singleton = true)]
    public class GameOverController
    {
        public event Action WinEvent;
        public event Action EndGameEvent;

        public void WinGame()
        {
            WinEvent?.Invoke();
        }

        public void EndGame()
        {
            EndGameEvent?.Invoke();
        }
    }
}
