using System;
using TMPro;
using UnityEngine;

namespace Game.UI.Screens
{
    public class GameOverScreen : UIScreen
    {
        [SerializeField] private TextMeshProUGUI tapToContinueText;

        public event Action ContinueInputGivenEvent;

        protected override void OnShow()
        {
            tapToContinueText.gameObject.SetActive(false);
            Invoke(nameof(ShowTapText), 2f);
        }

        protected override void OnHide()
        {
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && tapToContinueText.gameObject.activeSelf)
            {
                ContinueInputGivenEvent?.Invoke();
            }
        }

        private void ShowTapText()
        {
            tapToContinueText.gameObject.SetActive(true);
        }
    }
}
