using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3 {
    public class Enemy : Entity {
        public Enemy(DynamicShape shape, IBaseImage image) : base(shape, image) {
            StartingPosition = shape.Position;
        }

        public Vec2F StartingPosition { get; }
    }
}