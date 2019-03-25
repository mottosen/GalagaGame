using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using Galaga_Exercise_3.MovementStrategies;
using Galaga_Exercise_3.Squadrons;

namespace Galaga_Exercise_3.GalagaStates {
    public class GameRunning : IGameState {
        private Entity backGroundImage;
        
        // Enemies
        private List<Image> enemyStrides;
        private EntityContainer<Enemy> enemies;
        private List<ISquadron> squadrons = new List<ISquadron>();
        private List<IMovementStrategy> movementStrategies = new List<IMovementStrategy>();
        private IMovementStrategy movementStrategy = new MovementStrategyNoMove();
    
        // Explosions
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private int explosionLength = 500;
        
        private Random rand = new Random();
        
        // Player
        private Player player;
        private Entity playerLife1;
        private Entity playerLife2;
        private Entity playerLife3;
        private List<Entity> playerLives = new List<Entity>();
        
        // Smooth Movement
        private bool rightKeyDown;
        private bool leftKeyDown;
        
        // Shots
        private Image shotStride;
        private List<PlayerShot> playerShots;

        // Score
        public Score Score { get; private set; }

        public GameRunning() {
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0, 0), new Vec2F(1, 1)), 
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            
            // Player
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            playerLife1 = new Entity(
                new StationaryShape(new Vec2F(0.01f, 0.94f), new Vec2F(0.05f, 0.05f)), 
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            playerLife2 = new Entity(
                new StationaryShape(new Vec2F(0.06f, 0.94f), new Vec2F(0.05f, 0.05f)), 
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            playerLife3 = new Entity(
                new StationaryShape(new Vec2F(0.11f, 0.94f), new Vec2F(0.05f, 0.05f)), 
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            playerLives = new List<Entity>() {
                playerLife1,
                playerLife2,
                playerLife3
            };
            
            // Shots
            shotStride =
                ImageStride.CreateStrides(1, Path.Combine("Assets", "Images", "BulletRed2.png"))[0];
            playerShots = new List<PlayerShot>();
            
            // Enemies
            enemyStrides =
                ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
            enemies = new EntityContainer<Enemy>(0);
            
            // Squadrons
            squadrons.Add(new SquadronBox(enemyStrides));
            squadrons.Add(new SquadronLine(enemyStrides));
            squadrons.Add(new SquadronDiamonds(enemyStrides));
            
            // Movement Strategies
            movementStrategies.Add(new MovementStrategyDown());
            movementStrategies.Add(new MovementStrategyNoMove());
            movementStrategies.Add(new MovementStrategyZigZagDown());

            // Explosions
            explosionStrides =
                ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(8);

            // Score
            Score = new Score(new Vec2F(0.0f, -0.923f), new Vec2F(1f, 1f));
        }

        public void GameLoop() {
            throw new System.NotImplementedException();
        }

        public void InitializeGameState() {
            
        }

        public void UpdateGameLogic() {
            UpdatePlayer();
            UpdateShots();
            UpdateEnemies();
            Score.AddPoint();
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            player.RenderEntity();
            enemies.RenderEntities();

            foreach (PlayerShot shot in playerShots) {
                shot.Image = shotStride;
                shot.RenderEntity();
            }

            foreach (Entity playerLife in playerLives) {
                playerLife.RenderEntity();
            }

            explosions.RenderAnimations();
            Score.RenderScore();
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") {
                switch (keyValue) {
                case "KEY_ESCAPE":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, 
                            this, 
                            "CHANGE_STATE", 
                            "GAME_PAUSED", ""));
                    break;
                case "KEY_LEFT":
                    leftKeyDown = true;
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "SET_DIRECTION", 
                            "LEFT", ""));
                    break;
                case "KEY_RIGHT":
                    rightKeyDown = true;
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "SET_DIRECTION", 
                            "RIGHT", ""));
                    break;
                case "KEY_SPACE":
                    AddShot();
                    break;
                case "KEY_F":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "LOSE_LIFE", 
                            "", ""));
                    if (playerLives.Count > 0) {
                        playerLives.RemoveAt(playerLives.Count - 1);
                    }
                    break;
                }
            } else if (keyAction == "KEY_RELEASE") {
                switch (keyValue) {
                case "KEY_LEFT":
                    leftKeyDown = false;
                    if (rightKeyDown) {
                        GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.PlayerEvent, 
                                this, 
                                "SET_DIRECTION", 
                                "RIGHT", ""));
                    } else {
                        GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.PlayerEvent, 
                                this, 
                                "SET_DIRECTION", 
                                "NONE", ""));
                    }
                    break;
                case "KEY_RIGHT":
                    rightKeyDown = false;
                    if (leftKeyDown) {
                        GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.PlayerEvent, 
                                this, 
                                "SET_DIRECTION", 
                                "LEFT", ""));
                    } else {
                        GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.PlayerEvent, 
                                this, 
                                "SET_DIRECTION", 
                                "NONE", ""));
                    }
                    break;
                }
            }
        }

        private void UpdatePlayer() {
            player.Update();
        }

        private void UpdateShots() {
            List<PlayerShot> newPlayerShots = new List<PlayerShot>();
            foreach (PlayerShot shot in playerShots) {
                if (!shot.IsDeleted()) {
                    shot.Move();
                    newPlayerShots.Add(shot);
                } 

                foreach (Enemy enemy in enemies) {
                    CollisionData collision =
                        CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape.AsDynamicShape());
                    if (collision.Collision && !shot.IsDeleted()) {
                        Score.AddPoint(1000);
                        shot.DeleteEntity();
                        enemy.DeleteEntity();
                        AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y,
                            enemy.Shape.Extent.X, enemy.Shape.Extent.Y);
                    }
                }
            }
            
            playerShots = newPlayerShots;
        }
        
        private void UpdateEnemies() {
            EntityContainer<Enemy> newEnemies = new EntityContainer<Enemy>();
            foreach (Enemy enemy in enemies) {
                if (!enemy.IsDeleted()) {
                    newEnemies.AddDynamicEntity(enemy);
                }
            }

            enemies = newEnemies;
            
            if (enemies.CountEntities() == 0) {
                playerShots = new List<PlayerShot>();
                AddEnemies();
            }
            
            movementStrategy.MoveEnemies(enemies);
            EnemyCollision();
        }

        public void AddEnemies() {
            ISquadron newSquadron = squadrons[rand.Next(squadrons.Count)];
            movementStrategy = movementStrategies[rand.Next(movementStrategies.Count)];
            foreach (Enemy enemy in newSquadron.Enemies) {
                enemies.AddDynamicEntity(enemy);
            }
            newSquadron.Enemies.ClearContainer();
            newSquadron.CreateEnemies(enemyStrides);
        }
        
        private void EnemyCollision() {
            enemies.Iterate(delegate(Enemy enemy) {
                if (CollisionDetection.Aabb(player.Shape.AsDynamicShape(), enemy.Shape).Collision) {
                    enemy.DeleteEntity();
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "LOSE_LIFE", 
                            "", ""));
                    if (playerLives.Count > 0) {
                        playerLives.RemoveAt(playerLives.Count - 1);
                    }
                }

                if (enemy.Shape.Position.Y + enemy.Shape.Extent.Y <= 0.0f) {
                    enemy.DeleteEntity();
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "LOSE_LIFE", 
                            "", ""));
                    if (playerLives.Count > 0) {
                        playerLives.RemoveAt(playerLives.Count - 1);
                    }
                }
            });
        }

        public void AddShot() {
            PlayerShot shot = new PlayerShot(
                new DynamicShape(
                    player.GetCannonPoint(), 
                    new Vec2F(0.008F, 0.027F)), 
                    shotStride);
            playerShots.Add(shot);
        }

        public void AddExplosion(float posX, float posY, float extentX, float extentY) {
            explosions.AddAnimation(new StationaryShape(posX, posY, extentX, extentY),
                explosionLength, new ImageStride(explosionLength / 8, explosionStrides));
        }
    }
}