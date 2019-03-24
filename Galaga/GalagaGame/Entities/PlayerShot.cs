using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3 {
    public class PlayerShot : Entity {
        private Vec2F direction = new Vec2F(0F, 0.01F);
        private DynamicShape myShape;

        public PlayerShot(DynamicShape shape, IBaseImage image) : base(shape, image) {
            myShape = shape;
            myShape.ChangeDirection(direction);
        }

        public void Move() {
            var destination = myShape.Position + direction;
            if (destination.Y < 1F) {
                myShape.Move();
            } else {
                myShape.Move();
                DeleteEntity();
            }
        }
    }
}