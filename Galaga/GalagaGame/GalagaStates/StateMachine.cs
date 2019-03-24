using System;
using DIKUArcade.EventBus;
using DIKUArcade.State;

namespace Galaga_Exercise_3.GalagaStates {
    public class StateMachine : IGameEventProcessor<object> {
        public IGameState ActiveState { get; private set; }
        private IGameState currentGame;
        
        public StateMachine() {
            ActiveState = MainMenu.GetInstance();
        }

        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
            case (GameStateType.GameRunning):
                ActiveState = new GameRunning();
                currentGame = ActiveState;
                break;
            case (GameStateType.GamePaused):
                ActiveState = new GamePaused(currentGame);
                break;
            case (GameStateType.MainMenu):
                ActiveState = MainMenu.GetInstance();
                break;
            case (GameStateType.GameOver):
                ActiveState = new GameOver(currentGame);
                break;
            }
            ActiveState.InitializeGameState();
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.GameStateEvent) {
                switch (gameEvent.Message) {
                case "CHANGE_STATE":
                    if (gameEvent.Parameter2 == "CONTINUE") {
                        ActiveState = currentGame;
                    } else {
                        SwitchState(StateTransformer.TransformStringToState(gameEvent.Parameter1));
                    }
                    break;
                }
            } else if (eventType == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                }
            }
        }
        
        public void KeyPress(string key) {
            ActiveState.HandleKeyEvent(key, "KEY_PRESS");
        }

        public void KeyRelease(string key) {
            ActiveState.HandleKeyEvent(key, "KEY_RELEASE");
        }
    }
}