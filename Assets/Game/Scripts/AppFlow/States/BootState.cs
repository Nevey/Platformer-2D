using Game.StateMachines;
using UnityEngine.SceneManagement;

namespace Game.AppFlow.States
{
    public class BootState : State
    {
        protected override void OnEnter()
        {
            SceneManager.LoadSceneAsync("Main");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected override void OnExit()
        {
            
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            owner.ToNextState();
        }
    }
}
