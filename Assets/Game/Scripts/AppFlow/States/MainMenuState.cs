using Game.DI;
using Game.StateMachines;
using Game.UI;
using Game.UI.Screens;

namespace Game.AppFlow.States
{
    public class MainMenuState : State
    {
        [Inject] private UIController uiController;

        private MainScreen mainScreen;

        protected override void OnEnter()
        {
            mainScreen = uiController.ShowScreen<MainScreen>();
            mainScreen.PlayButtonPressedEvent += OnPlayButtonPressed;
        }

        protected override void OnExit()
        {
            mainScreen.Hide();
            mainScreen.PlayButtonPressedEvent -= OnPlayButtonPressed;
        }

        private void OnPlayButtonPressed()
        {
            owner.ToNextState();
        }
    }
}
