using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Screens
{
    public class MainScreen : UIScreen
    {
        [SerializeField] private Button playButton;

        public event Action PlayButtonPressedEvent;

        protected override void OnShow()
        {
            playButton.onClick.AddListener(OnPlayButtonPressed);
        }

        protected override void OnHide()
        {
            playButton.onClick.RemoveAllListeners();
        }

        private void OnPlayButtonPressed()
        {
            PlayButtonPressedEvent?.Invoke();
        }
    }
}
