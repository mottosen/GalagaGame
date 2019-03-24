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
        
        // Smooth Movement
        private bool rightKeyDown;
        private bool leftKeyDown;
        
        // Shots
        private Image shotStride;
        private List<PlayerShot> playerShots;

        // Score
        public Score Score { get; private set; }

        public void GameLoop() {
            throw new System.NotImplementedException();
        }

        public void InitializeGameState() {
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0, 0), new Vec2F(1, 1)), 
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            
            // Player
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            
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
            Score = new Score(new Vec2F(0.0f, -0.12f), new Vec2F(0.2f, 0.2f));
        }

        public void UpdateGameLogic() {
            player.Move();
            IterateShots();
            HandleEnemies();
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
                case "KEY_SPACE":
                    break;
                }
            }
        }
        
        private void HandleEnemies() {
            if (enemies.CountEntities() == 0) {
                playerShots = new List<PlayerShot>();
                AddEnemies();
            }
            movementStrategy.MoveEnemies(enemies);
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
        
        public void AddShot() {
            PlayerShot shot = new PlayerShot(
                new DynamicShape(player.GetCannonPoint(), new Vec2F(0.008F, 0.027F)), shotStride);
            playerShots.Add(shot);
        }

        public void IterateShots() {
            List<PlayerShot> newPlayerShots = new List<PlayerShot>();
            foreach (PlayerShot shot in playerShots) {
                if (!shot.IsDeleted()) {
                    shot.Move();
                    newPlayerShots.Add(shot);
                }

                foreach (Enemy enemy in enemies) {
                    CollisionData collision =
                        CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape);
                    if (collision.Collision && !shot.IsDeleted()) {
                        AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y,
                            enemy.Shape.Extent.X, enemy.Shape.Extent.Y);
                        Score.AddPoint(1000);
                        shot.DeleteEntity();
                        enemy.DeleteEntity();
                    }
                }
            }

            EntityContainer<Enemy> newEnemies = new EntityContainer<Enemy>();
            foreach (Enemy enemy in enemies) {
                if (!enemy.IsDeleted()) {
                    newEnemies.AddDynamicEntity(enemy);
                }
            }

            enemies = newEnemies;
            playerShots = newPlayerShots;
        }

        public void AddExplosion(float posX, float posY, float extentX, float extentY) {
            explosions.AddAnimation(new StationaryShape(posX, posY, extentX, extentY),
                explosionLength, new ImageStride(explosionLength / 8, explosionStrides));
        }
    }
}