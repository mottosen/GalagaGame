using System;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3 {
    public class Player : Entity, IGameEventProcessor<object> {
        private Vec2F direction = new Vec2F();
        private int lives;

        public Player(DynamicShape shape, IBaseImage image) : base(shape, image) {
            GalagaBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
            lives = 3;
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                case "SET_DIRECTION":
                    switch (gameEvent.Parameter1) {
                    case "LEFT":
                        Direction(new Vec2F(-0.01f, 0.0f));
                        break;
                    case "RIGHT":
                        Direction(new Vec2F(0.01f, 0.0f));
                        break;
                    case "NONE":
                        Direction(new Vec2F(0, 0));
                        break;
                    }
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
            Console.WriteLine(lives);
            lives--;
            if (lives < 0) {
                Die();
            }
        }

        private void Die() {
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

        public void Move() {
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