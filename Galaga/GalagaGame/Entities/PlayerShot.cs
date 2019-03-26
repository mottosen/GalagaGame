using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace GalagaGame {
    public class PlayerShot : Entity {
        private Vec2F direction = new Vec2F(0F, 0.01F);
        private static Image stride = ImageStride.CreateStrides(1, Path.Combine("Assets", "Images", "BulletRed2.png"))[0];

        public PlayerShot(DynamicShape shape) : base(shape, stride) {
            Shape.AsDynamicShape().ChangeDirection(direction);
        }

        public void Move() {
            var destination = Shape.Position + direction;
            if (destination.Y < 1F) {
                Shape.Move();
            } else {
                Shape.Move();
                DeleteEntity();
            }
        }
    }
}