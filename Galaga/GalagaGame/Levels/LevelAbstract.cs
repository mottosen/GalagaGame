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
        protected double EnemySpeed;
        protected EntityContainer<Enemy> Enemies = new EntityContainer<Enemy>(0);
        protected List<IMovementStrategy> MovementStrategies = new List<IMovementStrategy>();
        protected IMovementStrategy MovementStrategy = new MovementStrategyNoMove();

        public enum SquadronType {
            SquadronBox,
            SquadronLine,
            SquadronDiamonds
        }
        
        protected SquadronType[] Squadrons = (SquadronType[])Enum.GetValues(typeof(SquadronType));
    
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
            if (playerLives.IsEmpty()) {
                GalagaBus.GetBus().Unsubscribe(GameEventType.PlayerEvent, playerLives);
            }
            UpdatePlayer();
            UpdateShots();
            UpdateEnemies();
            Score.GetInstance().AddPoint();
        }

        public void Render() {
            backGroundImage.RenderEntity();
            player.RenderEntity();
            Enemies.RenderEntities();

            playerShots.RenderEntities();

            playerLives.Render();

            explosions.RenderAnimations();
            Score.GetInstance().RenderScore();
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

                foreach (Enemy enemy in Enemies) {
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
            });
            
            playerShots = newPlayerShots;
        }
        
        private void UpdateEnemies() {
            EntityContainer<Enemy> newEnemies = new EntityContainer<Enemy>();
            foreach (Enemy enemy in Enemies) {
                if (enemy.IsDestroyed()) {
                    Score.GetInstance().AddPoint(enemy.Points);
                    enemy.DeleteEntity();
                }
                
                if (!enemy.IsDeleted()) {
                    newEnemies.AddDynamicEntity(enemy);
                }
            }

            Enemies = newEnemies;
            
            if (Enemies.CountEntities() == 0) {
                playerShots = new EntityContainer<PlayerShot>();
                AddEnemies();
            }
            
            MovementStrategy.MoveEnemies(Enemies);
            EnemyCollision();
        }
        
        public void AddEnemies() {
            int rnd = Rand.Next(Squadrons.Length);
            ISquadron newSquadron = CreateSquadron(Squadrons[rnd]);
            MovementStrategy = MovementStrategies[Rand.Next(MovementStrategies.Count)];
            foreach (Enemy enemy in newSquadron.Enemies) {
                Enemies.AddDynamicEntity(enemy);
            }
        }
        
        public ISquadron CreateSquadron(SquadronType type) {
            switch (type) {
            case(SquadronType.SquadronBox):
                return new SquadronBox(50);
            case(SquadronType.SquadronLine):
                return new SquadronLine(50);
            case(SquadronType.SquadronDiamonds):
                return new SquadronDiamonds(50);
            default:
                throw new ArgumentException("Type conflict setting squadron.");
            }
        }
        
        private void EnemyCollision() {
            Enemies.Iterate(delegate(Enemy enemy) {
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