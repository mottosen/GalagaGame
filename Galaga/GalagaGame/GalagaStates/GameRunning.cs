using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Policy;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using GalagaGame.Levels;
using GalagaGame.MovementStrategies;
using GalagaGame.Squadrons;

namespace GalagaGame.GalagaStates {
    public class GameRunning : IGameState {
        public LevelAbstract Level { get; private set; }
        private int pointsToNextLevel;
        private int levelIndex;
        private Text levelText = new Text(
            "",
            new Vec2F(0.01f, -0.01f), 
            new Vec2F(1f, 1f));

        private Text LevelText {
            get {
                levelText.SetText(string.Format("Level: {0}", levelIndex + 1));
                return levelText;
            }
        }

        public GameRunning() {
            levelText.SetColor(Color.WhiteSmoke);
            levelText.SetFontSize(15);
        }
        
        private List<LevelAbstract> levels = new List<LevelAbstract>() {
            new Level_1(),
            new Level_2()
        };
        
        public void GameLoop() {
            throw new System.NotImplementedException();
        }

        public void InitializeGameState() {
            Score.GetInstance().ResetScore();
            Level = levels[0];
            pointsToNextLevel = 20000;
        }

        public void UpdateGameLogic() {
            if (Score.GetInstance().GetScore() >= pointsToNextLevel) {
                NextLevel();
            }
            Level.Update();
        }

        public void RenderState() {
            Level.Render();
            LevelText.RenderText();
        }

        public LevelAbstract GetLevel() {
            return Level;
        }

        private void NextLevel() {
            Level.EndLevel();
            levelIndex++;
            Level = levels[levelIndex];
            pointsToNextLevel = 1000000;
        }
        
        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {
                case "KEY_ESCAPE":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, 
                            this, 
                            "CHANGE_STATE", 
                            "GAME_PAUSED", ""));
                    break;
                case "KEY_LEFT":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "SET_DIRECTION", 
                            "LEFT", ""));
                    break;
                case "KEY_RIGHT":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "SET_DIRECTION", 
                            "RIGHT", ""));
                    break;
                case "KEY_SPACE":
                    Level.AddShot();
                    break;
                case "KEY_F":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "LOSE_LIFE", 
                            "", ""));
                    break;
                }
            } else if (keyAction == "KEY_RELEASE") {
                switch (keyValue) {
                case "KEY_LEFT":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "SET_DIRECTION", 
                            "RIGHT", ""));
                    break;
                case "KEY_RIGHT":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "SET_DIRECTION", 
                            "LEFT", ""));
                    break;
                }
            }
        }
    }
}