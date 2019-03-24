using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3.Squadrons {
    public class SquadronBox : ISquadron {
        private List<Vec2F> positions = new List<Vec2F> {
            new Vec2F(0.25f, 0.9f),
            new Vec2F(0.25f, 0.8f),
            new Vec2F(0.25f, 0.7f),
            new Vec2F(0.35f, 0.9f),
            new Vec2F(0.35f, 0.8f),
            new Vec2F(0.35f, 0.7f),
            new Vec2F(0.45f, 0.9f),
            new Vec2F(0.45f, 0.8f),
            new Vec2F(0.45f, 0.7f),
            new Vec2F(0.55f, 0.9f),
            new Vec2F(0.55f, 0.8f),
            new Vec2F(0.55f, 0.7f),
            new Vec2F(0.65f, 0.9f),
            new Vec2F(0.65f, 0.8f),
            new Vec2F(0.65f, 0.7f)
        };


        public SquadronBox(List<Image> aEnemyStrides) {
            // Create an empty container for the enemies.
            Enemies = new EntityContainer<Enemy>(MaxEnemies);

            // Instantiate the enemies.
            CreateEnemies(aEnemyStrides);
        }

        public int MaxEnemies { get; } = 0;
        public EntityContainer<Enemy> Enemies { get; }

        public void CreateEnemies(List<Image> enemyStrides) {
            for (var i = 0; i < positions.Count; i++) {
                var enemy = new Enemy(
                    new DynamicShape(positions[i], new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, enemyStrides));
                Enemies.AddDynamicEntity(enemy);
            }
        }
    }

    public class SquadronLine : ISquadron {
        private List<Vec2F> positions = new List<Vec2F> {
            new Vec2F(0.1f, 0.8f),
            new Vec2F(0.2f, 0.8f),
            new Vec2F(0.3f, 0.8f),
            new Vec2F(0.4f, 0.8f),
            new Vec2F(0.5f, 0.8f),
            new Vec2F(0.6f, 0.8f),
            new Vec2F(0.7f, 0.8f),
            new Vec2F(0.8f, 0.8f)
        };


        public SquadronLine(List<Image> aEnemyStrides) {
            // Create an empty container for the enemies.
            Enemies = new EntityContainer<Enemy>(MaxEnemies);

            // Instantiate the enemies.
            CreateEnemies(aEnemyStrides);
        }

        public int MaxEnemies { get; } = 0;
        public EntityContainer<Enemy> Enemies { get; }

        public void CreateEnemies(List<Image> enemyStrides) {
            for (var i = 0; i < positions.Count; i++) {
                var enemy = new Enemy(
                    new DynamicShape(positions[i], new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, enemyStrides));
                Enemies.AddDynamicEntity(enemy);
            }
        }
    }

    public class SquadronDiamonds : ISquadron {
        private List<Vec2F> positions = new List<Vec2F> {
            //left
            new Vec2F(0.15f, 0.9f),
            new Vec2F(0.1f, 0.8f),
            new Vec2F(0.1f, 0.7f),
            new Vec2F(0.2f, 0.8f),
            new Vec2F(0.2f, 0.7f),
            new Vec2F(0.15f, 0.6f),

            //mid
            new Vec2F(0.45f, 0.9f),
            new Vec2F(0.4f, 0.8f),
            new Vec2F(0.4f, 0.7f),
            new Vec2F(0.5f, 0.8f),
            new Vec2F(0.5f, 0.7f),
            new Vec2F(0.45f, 0.6f),

            //right
            new Vec2F(0.75f, 0.9f),
            new Vec2F(0.7f, 0.8f),
            new Vec2F(0.7f, 0.7f),
            new Vec2F(0.8f, 0.8f),
            new Vec2F(0.8f, 0.7f),
            new Vec2F(0.75f, 0.6f)
        };


        public SquadronDiamonds(List<Image> aEnemyStrides) {
            // Create an empty container for the enemies.
            Enemies = new EntityContainer<Enemy>(MaxEnemies);

            // Instantiate the enemies.
            CreateEnemies(aEnemyStrides);
        }

        public int MaxEnemies { get; } = 0;
        public EntityContainer<Enemy> Enemies { get; }

        public void CreateEnemies(List<Image> enemyStrides) {
            for (var i = 0; i < positions.Count; i++) {
                var enemy = new Enemy(
                    new DynamicShape(positions[i], new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, enemyStrides));
                Enemies.AddDynamicEntity(enemy);
            }
        }
    }
}