using System;
using Game.DI;
using UnityEngine;

namespace Game.UserInput
{
    [Injectable(Singleton = true)]
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private PlayerInputButton leftButton;
        [SerializeField] private PlayerInputButton rightButton;
        [SerializeField] private PlayerInputButton jumpButton;

        public event Action<PlayerInputAction, ActionState> ActionEvent;

        private void Awake()
        {
            leftButton.PressedEvent += OnButtonDown;
            leftButton.ReleasedEvent += OnButtonReleased;

            rightButton.PressedEvent += OnButtonDown;
            rightButton.ReleasedEvent += OnButtonReleased;

            jumpButton.PressedEvent += OnButtonDown;
            jumpButton.ReleasedEvent += OnButtonReleased;
        }

        private void OnDestroy()
        {
            leftButton.PressedEvent -= OnButtonDown;
            leftButton.ReleasedEvent -= OnButtonReleased;

            rightButton.PressedEvent -= OnButtonDown;
            rightButton.ReleasedEvent -= OnButtonReleased;

            jumpButton.PressedEvent -= OnButtonDown;
            jumpButton.ReleasedEvent -= OnButtonReleased;
        }

        private void OnButtonDown(PlayerInputAction playerInputAction)
        {
            ActionEvent?.Invoke(playerInputAction, ActionState.Start);
        }

        private void OnButtonReleased(PlayerInputAction playerInputAction)
        {
            ActionEvent?.Invoke(playerInputAction, ActionState.Stop);
        }
    }
}
