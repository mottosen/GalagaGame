using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using GalagaGame.MovementStrategies;
using GalagaGame.Squadrons;

namespace GalagaGame.Levels {
    public abstract class LevelAbstract {
        private Entity backGroundImage;
        
        protected Random Rand = new Random();
        
        // Enemies
        protected float EnemySpeed;
        protected int StrongEnemyRatio;
        protected List<ISquadron> Enemies = new List<ISquadron>();
        protected SquadronTypes[] PossibleSquadrons;
        protected MovementTypes[] PossibleMovements;
        
        protected enum SquadronTypes {
            Box,
            Line,
            Diamonds
        }
        
        protected enum MovementTypes {
            NoMove,
            Down,
            ZigZag
        }
        
        // Explosions
        private List<Image> explosionStrides;
        private AnimationContainer explosions;
        private int explosionLength = 500;
        
        // Player
        private Player player;
        private LifeBar playerLives = new LifeBar();
        
        // Shots
        private EntityContainer<PlayerShot> playerShots = new EntityContainer<PlayerShot>();

        public LevelAbstract() {
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0, 0), new Vec2F(1, 1)), 
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
            
            // Player
            player = new Player(
                new DynamicShape(
                    new Vec2F(0.45f, 0.1f), 
                    new Vec2F(0.1f, 0.1f))
                );
            
            // Explosions
            explosionStrides =
                ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(8);
        }

        public void Update() {
            UpdatePlayer();
            UpdateShots();
            UpdateEnemies();
            Score.GetInstance().AddPoint();
        }

        public void Render() {
            backGroundImage.RenderEntity();
            player.RenderEntity();
            foreach (var squadron in Enemies) {
                squadron.Enemies.RenderEntities();
            }

            playerShots.RenderEntities();
            playerLives.Render();

            explosions.RenderAnimations();
            Score.GetInstance().RenderScore();
        }

        public void EndLevel() {
            GalagaBus.GetBus().Unsubscribe(GameEventType.PlayerEvent, player);
            GalagaBus.GetBus().Unsubscribe(GameEventType.PlayerEvent, playerLives);
        }
        
        private void UpdatePlayer() {
            player.Update();
        }

        private void UpdateShots() {
            EntityContainer<PlayerShot> newPlayerShots = new EntityContainer<PlayerShot>();
            playerShots.Iterate(delegate(PlayerShot shot) {
                if (!shot.IsDeleted()) {
                    shot.Move();
                    newPlayerShots.AddDynamicEntity(shot);
                }

                foreach (ISquadron squadron in Enemies) {
                    foreach (Enemy enemy in squadron.Enemies) {
                        CollisionData collision =
                            CollisionDetection.Aabb(
                                shot.Shape.AsDynamicShape(), 
                                enemy.Shape.AsDynamicShape());
                        if (collision.Collision && !shot.IsDeleted()) {
                            shot.DeleteEntity();
                            enemy.Hit();
                            AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y,
                                enemy.Shape.Extent.X, enemy.Shape.Extent.Y);
                        }
                    }
                }
            });
            
            playerShots = newPlayerShots;
        }
        
        private void UpdateEnemies() {
            List<ISquadron> newEnemies = new List<ISquadron>();
            if (Enemies.Count == 0) {
                playerShots = new EntityContainer<PlayerShot>();
                AddEnemies();
            }
            
            foreach (ISquadron squadron in Enemies) {
                // only works on non empty squadrons
                if (squadron.CleanEnemies()) {
                    newEnemies.Add(squadron);
                    squadron.MoveEnemies();
                    EnemyCollision(squadron.Enemies);
                }
            }
            
            Enemies = newEnemies;
        }
        
        private void AddEnemies() {
            int rnd = Rand.Next(PossibleSquadrons.Length);
            ISquadron newSquadron = GetSquadron(PossibleSquadrons[rnd]);
            Enemies.Add(newSquadron);
        }
        
        private ISquadron GetSquadron(SquadronTypes desiredSquadron) {
            IMovementStrategy chosenMovement =
                GetMovement(PossibleMovements[Rand.Next(PossibleMovements.Length)]);

            switch (desiredSquadron) {
            case(SquadronTypes.Box):
                return new SquadronBox(StrongEnemyRatio, chosenMovement);
            case(SquadronTypes.Line):
                return new SquadronLine(StrongEnemyRatio, chosenMovement);
            case(SquadronTypes.Diamonds):
                return new SquadronDiamonds(StrongEnemyRatio, chosenMovement);
            default:
                throw new ArgumentException("Type conflict setting squadron.");
            }
        }
        
        private IMovementStrategy GetMovement(MovementTypes desiredMovement) {
            switch (desiredMovement) {
            case(MovementTypes.NoMove):
                return new MovementStrategyNoMove();
            case(MovementTypes.Down):
                return new MovementStrategyDown(EnemySpeed);
            case(MovementTypes.ZigZag):
                return new MovementStrategyZigZagDown(EnemySpeed);
            default:
                throw new ArgumentException("Type conflict setting movement.");
            }
        }
        
        private void EnemyCollision(EntityContainer<Enemy> squadron) {
            squadron.Iterate(delegate(Enemy enemy) {
                if (CollisionDetection.Aabb(player.Shape.AsDynamicShape(), enemy.Shape).Collision) {
                    enemy.DeleteEntity();
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "LOSE_LIFE", 
                            "", ""));
                }

                if (enemy.Shape.Position.Y + enemy.Shape.Extent.Y <= 0.0f) {
                    enemy.DeleteEntity();
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, 
                            this, 
                            "LOSE_LIFE", 
                            "", ""));
                }
            });
        }

        public void AddShot() {
            PlayerShot shot = new PlayerShot(
                new DynamicShape(
                    player.GetCannonPoint(), 
                    new Vec2F(0.008F, 0.027F)));
            playerShots.AddDynamicEntity(shot);
        }

        public void AddExplosion(float posX, float posY, float extentX, float extentY) {
            explosions.AddAnimation(new StationaryShape(posX, posY, extentX, extentY),
                explosionLength, new ImageStride(explosionLength / 8, explosionStrides));
        }
    }
}