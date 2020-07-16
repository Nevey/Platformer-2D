using Game.Enemies.Movement.States;
using Game.StateMachines;

namespace Game.Enemies.Movement
{
    public class EnemyMovementStateMachine : EnemyStateMachine
    {
        public EnemyMovementStateMachine()
        {
            SetInitialState<StartMovementState>();

            AddTransition<StartMovementState, ScanForwardState>();
            AddTransition<ScanForwardState, StartMovementState>();
        }
    }
}
