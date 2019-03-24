using DIKUArcade.Entities;

namespace Galaga_Exercise_3.MovementStrategies {
    public interface IMovementStrategy {
        void MoveEnemy(Enemy enemy);
        void MoveEnemies(EntityContainer<Enemy> enemies);
    }
}