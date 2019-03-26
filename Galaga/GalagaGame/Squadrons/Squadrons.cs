using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace GalagaGame.Squadrons {
    public class SquadronBox : ISquadron {
        private int strongEnemyRatio;
        private Random rand = new Random();
        
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

        public SquadronBox(int ratio) {
            // Create an empty container for the enemies.
            Enemies = new EntityContainer<Enemy>(MaxEnemies);

            // Ratio for choosing enemy strides
            strongEnemyRatio = ratio;
            
            // Instantiate the enemies.
            CreateEnemies();
        }

        public int MaxEnemies { get; } = 0;
        public EntityContainer<Enemy> Enemies { get; }

        public void CreateEnemies() {
            for (var i = 0; i < positions.Count; i++) {
                int randNum = rand.Next(100);
                Enemy enemy;
                if (strongEnemyRatio <= randNum) {
                    enemy = new StrongEnemy(new DynamicShape(positions[i], new Vec2F(0.1f, 0.1f)));
                } else {
                    enemy = new NormalEnemy(new DynamicShape(positions[i], new Vec2F(0.1f, 0.1f)));
                }
                
                Enemies.AddDynamicEntity(enemy);
            }
        }
    }

    public class SquadronLine : ISquadron {
        private int strongEnemyRatio;
        private Random rand = new Random();
        
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


        public SquadronLine(int ratio) {
            // Create an empty container for the enemies.
            Enemies = new EntityContainer<Enemy>(MaxEnemies);

            // Ratio for choosing enemy strides
            strongEnemyRatio = ratio;
            
            // Instantiate the enemies.
            CreateEnemies();
        }

        public int MaxEnemies { get; } = 0;
        public EntityContainer<Enemy> Enemies { get; }

        public void CreateEnemies() {
            for (var i = 0; i < positions.Count; i++) {
                int randNum = rand.Next(100);
                Enemy enemy;
                if (strongEnemyRatio <= randNum) {
                    enemy = new StrongEnemy(new DynamicShape(positions[i], new Vec2F(0.1f, 0.1f)));
                } else {
                    enemy = new NormalEnemy(new DynamicShape(positions[i], new Vec2F(0.1f, 0.1f)));
                }
                
                Enemies.AddDynamicEntity(enemy);
            }
        }
    }

    public class SquadronDiamonds : ISquadron {
        private int strongEnemyRatio;
        private Random rand = new Random();
        
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


        public SquadronDiamonds(int ratio) {
            // Create an empty container for the enemies.
            Enemies = new EntityContainer<Enemy>(MaxEnemies);

            // Ratio for choosing enemy strides
            strongEnemyRatio = ratio;
            
            // Instantiate the enemies.
            CreateEnemies();
        }

        public int MaxEnemies { get; } = 0;
        public EntityContainer<Enemy> Enemies { get; }

        public void CreateEnemies() {
            for (var i = 0; i < positions.Count; i++) {
                int randNum = rand.Next(100);
                Enemy enemy;
                if (strongEnemyRatio <= randNum) {
                    enemy = new StrongEnemy(new DynamicShape(positions[i], new Vec2F(0.1f, 0.1f)));
                } else {
                    enemy = new NormalEnemy(new DynamicShape(positions[i], new Vec2F(0.1f, 0.1f)));
                }
                
                Enemies.AddDynamicEntity(enemy);
            }
        }
    }
}