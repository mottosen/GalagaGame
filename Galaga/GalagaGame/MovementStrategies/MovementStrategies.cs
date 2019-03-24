using System;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace Galaga_Exercise_3.MovementStrategies {
    public class MovementStrategyNoMove : IMovementStrategy {
        public void MoveEnemy(Enemy enemy) { }

        public void MoveEnemies(EntityContainer<Enemy> enemies) { }
    }

    public class MovementStrategyDown : IMovementStrategy {
        private float speed = 0.001f;

        public void MoveEnemy(Enemy enemy) {
            var currPos = enemy.Shape.Position;
            var newPos = new Vec2F(currPos.X, currPos.Y - speed);
            enemy.Shape.SetPosition(newPos);
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }

    public class MovementStrategyZigZagDown : IMovementStrategy {
        private float amplitude = 0.05f;
        private float period = 0.045f;
        private float speed = 0.0005f;

        public void MoveEnemy(Enemy enemy) {
            var currPos = enemy.Shape.Position;
            var startPos = enemy.StartingPosition;

            var newPos = new Vec2F(0, 0);

            newPos.Y = currPos.Y - speed;
            var sineThing = (float) Math.Sin(2 * Math.PI * (startPos.Y - newPos.Y) / period);
            newPos.X = startPos.X + amplitude * sineThing;

            enemy.Shape.SetPosition(newPos);
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
    }
}