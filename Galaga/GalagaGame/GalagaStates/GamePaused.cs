using System.Drawing;
using System.IO;
using System;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using GalagaGame.Levels;
using Image = DIKUArcade.Graphics.Image;

namespace GalagaGame.GalagaStates {
    public class GamePaused : IGameState{
        private IGameState currentGame;
        
        private Entity filter = new Entity(
            new StationaryShape(new Vec2F(0, 0), new Vec2F(1, 1)),
            new Image(Path.Combine("Assets", "Images", "Filter.png")));
        private Entity buttonSelectImage = new Entity(
            new StationaryShape(new Vec2F(0, 0), new Vec2F(0.01f, 0.05f)), 
            new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
        private Text[] menuButtons = {
            new Text(
                "Continue", 
                new Vec2F(0.1f, 0.1f), 
                new Vec2F(0.5f, 0.5f)),
            new Text(
                "Main Menu", 
                new Vec2F(0.1f, 0.0f), 
                new Vec2F(0.5f, 0.5f))
        };
        
        private int activeMenuButton;
        private int maxMenuButtons;

        public GamePaused(GameRunning aCurrentGame) {
            if (Score.GetInstance().TrueVictory()) {
                GalagaBus.GetBus().RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.GameStateEvent,
                        this,
                        "CHANGE_STATE",
                        "GAME_OVER", ""));;
            }
            currentGame = aCurrentGame;
            maxMenuButtons = menuButtons.Length;
            buttonSelectImage.Shape.Rotate(0.5f * (float)Math.PI);
        }

        public void GameLoop() {
            
        }

        public void InitializeGameState() {
            
        }

        public void UpdateGameLogic() {
            
        }

        public void RenderState() {
            currentGame.RenderState();
            filter.RenderEntity();
            
            for (int i = 0; i < maxMenuButtons; i++) {
                if (i == activeMenuButton) {
                    menuButtons[i].SetColor(Color.YellowGreen);
                    var pos = menuButtons[i].GetShape().Position;
                    buttonSelectImage.Shape.SetPosition(new Vec2F(pos.X - 0.03f, pos.Y + 0.4375f));
                } else {
                    menuButtons[i].SetColor(Color.WhiteSmoke);
                }
                menuButtons[i].RenderText();
            }
            buttonSelectImage.RenderEntity();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {
                case ("KEY_DOWN"):
                    ChangeButton(1);
                    break;
                case ("KEY_UP"):
                    ChangeButton(-1);
                    break;
                case ("KEY_ENTER"):
                case ("KEY_SPACE"):
                    if (activeMenuButton == 0) {
                        Continue();
                    } else {
                        ReturnToMainMenu();
                    }
                    break;
                case ("KEY_ESCAPE"):
                    Continue();
                    break;
                }
            }
        }

        private void Continue() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_RUNNING", "CONTINUE"));
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