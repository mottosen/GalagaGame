using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace Galaga_Exercise_3.GalagaStates {
    public class GameOver : IGameState {
        //private Entity buttonSelectImage = new Entity(
        //    new StationaryShape(new Vec2F(0, 0), new Vec2F(0.01f, 0.05f)), 
        //    new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
        
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

        private Entity backGroundImage;
        private int activeMenuButton;
        private int maxMenuButtons;
        private int score;

        public GameOver(IGameState aCurrentGame) {
            maxMenuButtons = menuButtons.Length;
            //buttonSelectImage.Shape.Rotate(0.5f * (float)Math.PI);
            score = ((GameRunning)aCurrentGame).Score.GetScore();
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
        }

        public void UpdateGameLogic() {
            
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            for (int i = 0; i < maxMenuButtons; i++) {
                if (i == activeMenuButton) {
                    menuButtons[i].SetColor(Color.YellowGreen);
                    var pos = menuButtons[i].GetShape().Position;
                    //buttonSelectImage.Shape.SetPosition(new Vec2F(pos.X - 0.03f, pos.Y + 0.4375f));
                } else {
                    menuButtons[i].SetColor(Color.WhiteSmoke);
                }
                menuButtons[i].RenderText();
            }
            //buttonSelectImage.RenderEntity();
            scoreText.RenderText();
            gameOverText.RenderText();
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
    }
}