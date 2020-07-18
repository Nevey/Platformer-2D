using Game.DI;
using Game.StateMachines;
using Game.UI;
using Game.UI.Screens;
using Game.Utils;

namespace Game.AppFlow.States
{
    public class GameOverState : State
    {
        [Inject] private UIController uiController;
        [Inject] private SceneLoader sceneLoader;

        private GameOverScreen gameOverScreen;

        protected override void OnEnter()
        {
            gameOverScreen = uiController.ShowScreen<GameOverScreen>();
            gameOverScreen.ContinueInputGivenEvent += OnContinueInputGiven;
        }

        protected override void OnExit()
        {
        }

        private void OnContinueInputGiven()
        {
            gameOverScreen.ContinueInputGivenEvent -= OnContinueInputGiven;
            sceneLoader.UnloadScenes(OnUnloadFinished, "Gameplay");
        }

        private void OnUnloadFinished()
        {
            owner.ToNextState();
        }
    }
}
