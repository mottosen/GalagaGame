using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace GalagaGame {
    public class StrongEnemy : Enemy {
        private static List<Image> strides = ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "GreenMonster.png"));

        public StrongEnemy(DynamicShape shape) : base(shape, new ImageStride(160, StrongEnemy.strides)) {
            lives = 3;
            Points = 3000;
        }
    }
}