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

        #if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnButtonDown(PlayerInputAction.MoveLeft);
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                OnButtonReleased(PlayerInputAction.MoveLeft);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                OnButtonDown(PlayerInputAction.Jump);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                OnButtonReleased(PlayerInputAction.Jump);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                OnButtonDown(PlayerInputAction.MoveRight);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                OnButtonReleased(PlayerInputAction.MoveRight);
            }
        }
        #endif

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
