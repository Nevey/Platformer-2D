using Game.DI;
using Game.Gameplay.Cameras;
using Game.Gameplay.Scoring;
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
        [Inject] private ScoreController scoreController;
        [Inject] private CameraFollowTarget cameraFollowTarget;

        private GameOverScreen gameOverScreen;

        protected override void OnEnter()
        {
            gameOverScreen = uiController.GetScreen<GameOverScreen>();
            gameOverScreen.SetScoreValue(scoreController.Score);

            uiController.ShowScreen<GameOverScreen>();

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
            cameraFollowTarget.DisableCamera();
            owner.ToNextState();
        }
    }
}
