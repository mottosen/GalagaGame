using System;
using System.Collections.Generic;
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
        private LevelAbstract level;
        private int desiredLevel;
        private int pointsToNextLevel;
        private int levelIndex;
        
        private List<LevelAbstract> levels = new List<LevelAbstract>() {
            new Level_1()
        };
        
        public GameRunning(int theDesiredLevel) {
            desiredLevel = theDesiredLevel;
        }
        
        public void GameLoop() {
            throw new System.NotImplementedException();
        }

        public void InitializeGameState() {
            Score.GetInstance().ResetScore();
            level = levels[0];
            pointsToNextLevel = 20000;
        }

        public void UpdateGameLogic() {
            if (Score.GetInstance().GetScore() >= pointsToNextLevel) {
                //NextLevel();
            }
            level.Update();
        }

        public void RenderState() {
            level.Render();
        }

        public LevelAbstract GetLevel() {
            return level;
        }

        private void NextLevel() {
            levelIndex++;
            level = levels[levelIndex];
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
                    level.AddShot();
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