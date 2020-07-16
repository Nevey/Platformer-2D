using Game.DI;
using Game.Spawning;
using Game.StateMachines;

namespace Game.GameplayFlow.States
{
    public class SpawnState : State
    {
        [Inject] private CheckpointController checkpointController;

        protected override void OnEnter()
        {
            checkpointController.SpawnAtCheckpoint();
            owner.ToNextState();
        }

        protected override void OnExit()
        {
            
        }
    }
}
