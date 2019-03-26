using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace GalagaGame {
    public class Enemy : Entity {
        protected int lives;
        public int Points { get; protected set; }
        public Vec2F StartingPosition { get; }

        public Enemy(DynamicShape shape, IBaseImage image) : base(shape, image) {
            StartingPosition = shape.Position;
        }

        public bool IsDestroyed() {
            return lives <= 0;
        }

        public void Hit() {
            lives--;
        }
    }
}