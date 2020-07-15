using Game.AppFlow.States;
using Game.StateMachines;

namespace Game.AppFlow
{
    public class AppFlowStateMachine : StateMachine
    {
        public AppFlowStateMachine()
        {
            SetInitialState<BootState>();

            AddTransition<BootState, MainMenuState>();
            AddTransition<MainMenuState, GameplayState>();
            AddTransition<GameplayState, GameOverState>();
            AddTransition<GameOverState, MainMenuState>();
        }
    }
}
