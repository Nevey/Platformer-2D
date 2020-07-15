using Game.DI;
using Game.StateMachines;
using Game.Utils;

namespace Game.AppFlow.States
{
    public class BootState : State
    {
        [Inject] private SceneLoader sceneLoader;
        
        protected override void OnEnter()
        {
            // Unload all open scenes, in case we're in the editor and have multiple scenes open
            sceneLoader.UnloadAllScenes(UnloadAllScenesFinished, "Boot");
        }

        protected override void OnExit()
        {
            
        }

        private void UnloadAllScenesFinished()
        {
            // Load Main and UI scenes
            sceneLoader.LoadScenes(OnMainScenesLoaded, "Main", "UI");
        }

        private void OnMainScenesLoaded()
        {
            // Unload Boot scene
            sceneLoader.UnloadScenes(owner.ToNextState, "Boot");
        }
    }
}
