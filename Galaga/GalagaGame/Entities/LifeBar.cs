using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Math;

namespace GalagaGame {
    public class LifeBar : IGameEventProcessor<object> {
        private List<PlayerLife> playerLives = new List<PlayerLife>();

        public LifeBar() {
            GalagaBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
            for (int i = 1; i < 4; i++) {
                playerLives.Add(new PlayerLife(
                    new StationaryShape(
                        new Vec2F(0f, 0f),
                        new Vec2F(0.05f, 0.05f)),
                        i));
            }
        }

        public void Render() {
            foreach (PlayerLife life in playerLives) {
                life.RenderEntity();
            }
        }

        public bool IsEmpty() {
            return playerLives.Count <= 0;
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                case "LOSE_LIFE":
                    if (playerLives.Count > 0) {
                        playerLives.RemoveAt(playerLives.Count - 1);
                    }
                    break;
                }
            }
        }
    }
}