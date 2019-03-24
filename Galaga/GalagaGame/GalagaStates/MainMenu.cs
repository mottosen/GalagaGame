using System;
using System.Drawing;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;
using Image = DIKUArcade.Graphics.Image;

namespace Galaga_Exercise_3.GalagaStates {
    public class MainMenu : IGameState {
        private static MainMenu instance = null;

        private Entity backGroundImage = new Entity(
            new StationaryShape(new Vec2F(0, 0), new Vec2F(1f, 1f)), 
            new Image(Path.Combine("Assets", "Images", "TitleImage.png")));
        private Entity buttonSelectImage = new Entity(
            new StationaryShape(new Vec2F(0, 0), new Vec2F(0.01f, 0.05f)), 
            new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
        private Text[] menuButtons = {
            new Text(
                "New Game", 
                new Vec2F(0.1f, 0.1f), 
                new Vec2F(0.5f, 0.5f)),
            new Text(
                "Quit", 
                new Vec2F(0.1f, 0.0f), 
                new Vec2F(0.5f, 0.5f))
        };
        
        private int activeMenuButton;
        private int maxMenuButtons;

        public MainMenu() {
            activeMenuButton = 0;
            maxMenuButtons = menuButtons.Length;
            buttonSelectImage.Shape.Rotate(0.5f * (float)Math.PI);
        }

        public static MainMenu GetInstance() {
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
        }

        public void GameLoop() {
            
        }

        public void InitializeGameState() {
            
        }

        public void UpdateGameLogic() {
            
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
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
                        NewGame();
                    } else {
                        Quit();
                    }
                    break;
                case ("KEY_ESCAPE"):
                    Quit();
                    break;
                }
            }
        }

        private void NewGame() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_RUNNING", ""));
        }
        
        private void Quit() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.WindowEvent,
                    this,
                    "CLOSE_WINDOW",
                    "", ""));
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