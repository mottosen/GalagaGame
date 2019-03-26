using DIKUArcade.Entities;

namespace GalagaGame.MovementStrategies {
    public interface IMovementStrategy {
        void MoveEnemy(Enemy enemy);
        void MoveEnemies(EntityContainer<Enemy> enemies);
    }
}