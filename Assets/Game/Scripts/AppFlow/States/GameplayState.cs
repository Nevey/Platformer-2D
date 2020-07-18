using System;
using Game.DI;
using Game.GameplayFlow;
using Game.StateMachines;
using Game.UI;
using Game.UI.Screens;
using Game.Utils;

namespace Game.AppFlow.States
{
    public class GameplayState : State
    {
        [Inject] private SceneLoader sceneLoader;
        [Inject] private UIController uiController;
        [Inject] private GameOverController gameOverController;

        private GameplayScreen gameplayScreen;

        protected override void OnEnter()
        {
            gameplayScreen = uiController.ShowScreen<GameplayScreen>();

            sceneLoader.LoadScenes(true, "Gameplay");

            gameOverController.WinEvent += OnWin;
        }

        protected override void OnExit()
        {
            gameOverController.WinEvent -= OnWin;
            gameplayScreen.Hide();
        }

        private void OnWin()
        {
            owner.ToNextState();
        }
    }
}
