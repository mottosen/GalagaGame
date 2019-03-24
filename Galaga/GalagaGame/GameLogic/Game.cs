using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Galaga_Exercise_3.GalagaStates;
using Galaga_Exercise_3.MovementStrategies;
using Galaga_Exercise_3.Squadrons;

namespace Galaga_Exercise_3 {
    public class Game : IGameEventProcessor<object> {
        // System
        private GameTimer gameTimer;
        private Window win;
        private StateMachine stateMachine;

        public Game() {
            win = new Window("Film", 500, 500);
            gameTimer = new GameTimer(60, 60);

            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent,
                GameEventType.WindowEvent,
                GameEventType.PlayerEvent,
                GameEventType.GameStateEvent
            });
            win.RegisterEventBus(GalagaBus.GetBus());
            GalagaBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
            

            stateMachine = new StateMachine();
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);
        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    
                    // TODO: change processor back from sequentially
                    GalagaBus.GetBus().ProcessEventsSequentially();
                    stateMachine.ActiveState.UpdateGameLogic();
                }
                
                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    
                    stateMachine.ActiveState.RenderState();
                    
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
                                gameTimer.CapturedFrames;
                }
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                }
            }
        }
    }
}