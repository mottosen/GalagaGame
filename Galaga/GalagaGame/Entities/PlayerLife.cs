using System;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace GalagaGame {
    public class PlayerLife : Entity {
        private static Image stride =
            ImageStride.CreateStrides(1, Path.Combine("Assets", "Images", "Player.png"))[0];

        public PlayerLife(StationaryShape shape, int placement) : base(shape, PlayerLife.stride) {
            switch (placement) {
                case(1):
                    shape.SetPosition(new Vec2F(0.01f, 0.94f));
                    break;
                case(2):
                    shape.SetPosition(new Vec2F(0.06f, 0.94f));
                    break;
                case(3):
                    shape.SetPosition(new Vec2F(0.11f, 0.94f));
                    break;
                default:
                    throw new ArgumentException(
                        "The PlayerLife could not be placed correctly.");
            }
        }
    }
}