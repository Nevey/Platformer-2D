using Game.Gameplay.GameplayFlow.States;
using Game.StateMachines;

namespace Game.Gameplay.GameplayFlow
{
    public class GameplayStateMachine : StateMachine
    {
        public GameplayStateMachine()
        {
            SetInitialState<SpawnState>();

            AddTransition<SpawnState, AliveState>();
            AddTransition<AliveState, DeathState>();
            AddTransition<DeathState, AliveState>();
        }
    }
}
