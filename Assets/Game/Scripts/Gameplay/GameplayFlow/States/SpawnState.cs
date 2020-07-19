using Game.DI;
using Game.Gameplay.Spawning;
using Game.StateMachines;

namespace Game.Gameplay.GameplayFlow.States
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
