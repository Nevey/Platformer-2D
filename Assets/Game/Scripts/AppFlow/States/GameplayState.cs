using Game.DI;
using Game.Gameplay.Cameras;
using Game.Gameplay.GameplayFlow;
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
        [Inject] private CameraFollowTarget cameraFollowTarget;

        private GameplayScreen gameplayScreen;

        protected override void OnEnter()
        {
            gameplayScreen = uiController.ShowScreen<GameplayScreen>();

            sceneLoader.LoadScenes(true, "Gameplay");

            gameOverController.WinEvent += OnWin;

            cameraFollowTarget.EnableCamera();
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
