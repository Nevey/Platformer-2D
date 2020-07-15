using Game.DI;
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
        
        private GameplayScreen gameplayScreen;

        protected override void OnEnter()
        {
            gameplayScreen = uiController.ShowScreen<GameplayScreen>();

            sceneLoader.LoadScenes("Gameplay");
        }

        protected override void OnExit()
        {
            gameplayScreen.Hide();
        }
    }
}
