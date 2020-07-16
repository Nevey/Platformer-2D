using Game.GameplayFlow.States;
using Game.StateMachines;

namespace Game.GameplayFlow
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
