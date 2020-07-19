using Game.DI;

namespace Game.Scoring
{
    [Injectable(Singleton = true)]
    public class ScoreController
    {
        private int score;

        public int Score => score;

        public void UpdateScore(int amount)
        {
            score += amount;
        }

        public void SetScore(int amount)
        {
            score = amount;
        }

        public void ResetScore()
        {
            score = 0;
        }
    }
}
