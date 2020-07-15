using UnityEngine;

namespace Game.AppFlow
{
    public class AppFlowStateMachineWrapper : MonoBehaviour
    {
        private AppFlowStateMachine appFlowStateMachine;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            appFlowStateMachine = new AppFlowStateMachine();
        }

        private void Start()
        {
            appFlowStateMachine.Start();
        }

        private void OnDestroy()
        {
            appFlowStateMachine.Stop();
        }
    }
}
