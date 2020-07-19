using System;
using Game.Utils;
using TMPro;
using UnityEngine;

namespace Game.UI.Screens
{
    public class GameOverScreen : UIScreen
    {
        [SerializeField] private TextMeshProUGUI tapToContinueText;
        [SerializeField] private TextMeshProUGUI scoreText;

        private string defaultScoreString = null;
        private int? score;

        public event Action ContinueInputGivenEvent;

        protected override void OnShow()
        {
            if (score == null)
            {
                throw Log.Exception("Please call <b>\"GameOverScreen.SetScoreValue\"</b> before <b>\"GameOverScreen.Show\"</b>");
            }

            defaultScoreString = String.IsNullOrEmpty(defaultScoreString) ? String.Format(scoreText.text, score.Value) : defaultScoreString;
            scoreText.text = defaultScoreString;

            tapToContinueText.gameObject.SetActive(false);
            Invoke(nameof(ShowContinueText), 2f);
        }

        protected override void OnHide()
        {
            score = null;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && tapToContinueText.gameObject.activeSelf)
            {
                ContinueInputGivenEvent?.Invoke();
            }
        }

        private void ShowContinueText()
        {
            tapToContinueText.gameObject.SetActive(true);
        }

        public void SetScoreValue(int score)
        {
            this.score = score;
        }
    }
}
