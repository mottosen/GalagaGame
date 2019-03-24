using System.Drawing;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3 {
    public class Score {
        private Text display;
        private int score;

        public Score(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text(score.ToString(), position, extent);
            display.SetColor(Color.Aqua);
            display.SetFontSize(120);
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
        
        public bool TrueVictory() {
            return score == 42;
        }
    }
}