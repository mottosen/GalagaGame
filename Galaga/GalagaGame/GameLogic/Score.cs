using System.Diagnostics;
using System.Drawing;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace GalagaGame {
    public class Score {
        private static Score instance;
        private Text display;
        private int score;

        public Score() {
            score = 0;
            display = new Text(
                score.ToString(), 
                new Vec2F(0.0f, -0.923f), 
                new Vec2F(1f, 1f));
            display.SetColor(Color.WhiteSmoke);
            display.SetFontSize(25);
        }

        public static Score GetInstance() {
            return Score.instance ?? (Score.instance = new Score());
        }
        
        public void AddPoint() {
            score++;
        }

        public void AddPoint(int points) {
            score += points;
        }

        public void RenderScore() {
            display.SetText(string.Format("{0}", score.ToString()));
            display.RenderText();
        }

        public int GetScore() {
            return score;
        }

        public void ResetScore() {
            score = 0;
        }

        public bool TrueVictory() {
            return score == 42;
        }
    }
}