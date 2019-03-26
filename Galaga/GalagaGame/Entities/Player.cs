using System;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace GalagaGame {
    public class Player : Entity, IGameEventProcessor<object> {
        private Vec2F direction = new Vec2F();
        private int lives;
        private int moveDirection;

        private static Image stride =
            ImageStride.CreateStrides(1, Path.Combine("Assets", "Images", "Player.png"))[0];

        public Player(DynamicShape shape) : base(shape, Player.stride) {
            GalagaBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
            lives = 3;
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                case "SET_DIRECTION":
                    switch (gameEvent.Parameter1) {
                    case "LEFT":
                        moveDirection--;
                        break;
                    case "RIGHT":
                        moveDirection++;
                        break;
                    }
                    Direction(new Vec2F(moveDirection * 0.01f, 0));
                    break;
                case "SHOOT":
                    Shoot();
                    break;
                case "LOSE_LIFE":
                    LoseLife();
                    break;
                }
            }
        }

        private void LoseLife() {
            lives--;
        }
        
        public void Update() {
            if (lives < 0) {
                Die();
            }
            Move();
        }
        
        private void Die() {
            GalagaBus.GetBus().Unsubscribe(GameEventType.PlayerEvent, this);
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_OVER", ""));
        }
        
        private void Direction(Vec2F dir) {
            direction = dir;
            Shape.AsDynamicShape().ChangeDirection(dir);
        }

        private void Move() {
            var destination = Shape.Position + direction;
            if (destination.X > 0 && destination.X + Shape.Extent.X < 1) {
                Shape.Move();
            }
        }

        public Vec2F GetCannonPoint() {
            var pos = Shape.Position;
            return new Vec2F(pos.X + 0.046F, pos.Y + 0.1F);
        }

        private void Shoot() {
            
        }
    }
}