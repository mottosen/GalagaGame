using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace GalagaGame {
    public class NormalEnemy : Enemy {
        private static List<Image> strides = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));

        public NormalEnemy(DynamicShape shape) : base(shape, new ImageStride(80, NormalEnemy.strides)) {
            lives = 1;
            Points = 1000;
        }
    }
}