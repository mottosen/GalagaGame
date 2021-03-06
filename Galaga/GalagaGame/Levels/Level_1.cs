using System;
using GalagaGame.MovementStrategies;
using GalagaGame.Squadrons;

namespace GalagaGame.Levels {
    public class Level_1 : LevelAbstract {
        public Level_1() {
            EnemySpeed = 0.001f;
            StrongEnemyRatio = 0;
            
            // Squadrons
            PossibleSquadrons = new [] {
                SquadronTypes.Box,
                SquadronTypes.Line
            };
            
            // Movement Strategies
            PossibleMovements = new[] {
                MovementTypes.NoMove,
                MovementTypes.Down
            };
        }
    }
}