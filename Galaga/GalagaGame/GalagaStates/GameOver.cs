using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using GalagaGame.Levels;
using Image = DIKUArcade.Graphics.Image;

namespace GalagaGame.GalagaStates {
    public class GameOver : IGameState {
        private Text[] menuButtons = {
            new Text(
                "Restart", 
                new Vec2F(0.125f, -0.1f), 
                new Vec2F(0.5f, 0.5f)),
            new Text(
                "Main Menu", 
                new Vec2F(0.525f, -0.1f), 
                new Vec2F(0.5f, 0.5f))
        };
        
        private Text gameOverText = new Text("Game Over!", 
            new Vec2F(0.2f, -0.1f), 
            new Vec2F(0.75f, 0.75f));
        
        private Text scoreText = new Text("", 
            new Vec2F(0.3f, -0.0775f), 
            new Vec2F(0.6f, 0.6f));
        
        private Text highscoreText = new Text(
            "",
            new Vec2F(0f, 0f),
            new Vec2F(1f, 0.9f));

        private Entity backGroundImage;
        private int activeMenuButton;
        private int maxMenuButtons;
        private int score;

        public GameOver(GameRunning aCurrentGame) {
            maxMenuButtons = menuButtons.Length;
            score = Score.GetInstance().GetScore();
        }

        public void GameLoop() {
            
        }

        public void InitializeGameState() {
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0, 0), new Vec2F(1, 1)), 
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            gameOverText.SetColor(Color.YellowGreen);
            scoreText.SetText(string.Format("Score: {0}", score));
            scoreText.SetColor(Color.WhiteSmoke);
            highscoreText.SetColor(Color.Red);
            CheckHighscore();
        }

        public void UpdateGameLogic() {
            
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            for (int i = 0; i < maxMenuButtons; i++) {
                if (i == activeMenuButton) {
                    menuButtons[i].SetColor(Color.YellowGreen);
                    var pos = menuButtons[i].GetShape().Position;
                } else {
                    menuButtons[i].SetColor(Color.WhiteSmoke);
                }
                menuButtons[i].RenderText();
            }
            scoreText.RenderText();
            gameOverText.RenderText();
            highscoreText.RenderText();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {
                case ("KEY_RIGHT"):
                    ChangeButton(1);
                    break;
                case ("KEY_LEFT"):
                    ChangeButton(-1);
                    break;
                case ("KEY_ENTER"):
                case ("KEY_SPACE"):
                    if (activeMenuButton == 0) {
                        Restart();
                    } else {
                        ReturnToMainMenu();
                    }

                    break;
                }
            }
        }
        
        private void Restart() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_RUNNING", ""));
        }

        private void ReturnToMainMenu() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "MAIN_MENU", ""));
        }

        private void ChangeButton(int value) {
            activeMenuButton += value;
            if (activeMenuButton >= maxMenuButtons) {
                activeMenuButton = maxMenuButtons - 1;
            } else if (activeMenuButton < 0) {
                activeMenuButton = 0;
            }
        }

        private void CheckHighscore() {
            int oldHighscore;
            using (StreamReader sr = File.OpenText(@"./../../highscore.txt")) {
                string origin = sr.ReadLine();
                
                if (origin == null) {
                    throw new FormatException(
                        "Document empty.");
                } else {
                    if (!Int32.TryParse(origin, out oldHighscore)) {
                        throw new InvalidCastException(
                            "The found highscore could not be casted to Int32.");
                    }
                }
            }
            
            if (score > oldHighscore || score == 42) {
                highscoreText.SetText("New Highscore!");
                using (StreamWriter sw = File.CreateText(@"./../../highscore.txt")) {
                    sw.WriteLine(string.Format("{0}", score));
                }
            }
        }
    }
}