using Game.Utils;

namespace Game.StateMachines
{
    /// <summary>
    /// Core State used by the StateMachine. Extend from this class to create your own State.
    /// </summary>
    public abstract class State
    {
        protected StateMachine owner;

        protected virtual void OnInitialize() {}
        protected abstract void OnEnter();
        protected abstract void OnExit();

        public void Initialize(StateMachine owner)
        {
            this.owner = owner;
            OnInitialize();
        }

        public void Enter()
        {
            Log.Write(GetType().Name);
            OnEnter();
        }

        public void Exit()
        {
            Log.Write(GetType().Name);
            OnExit();
        }
    }
}
