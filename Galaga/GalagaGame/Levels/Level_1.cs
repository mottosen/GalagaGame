using GalagaGame.MovementStrategies;
using GalagaGame.Squadrons;

namespace GalagaGame.Levels {
    public class Level_1 : LevelAbstract {
        public Level_1() {
            EnemySpeed = 0.001f;
            
            // Squadrons
            Squadrons.Add(new SquadronBox(10));
            Squadrons.Add(new SquadronLine(10));
            Squadrons.Add(new SquadronDiamonds(10));

            // Movement Strategies
            MovementStrategies.Add(new MovementStrategyDown());
            MovementStrategies.Add(new MovementStrategyNoMove());
            MovementStrategies.Add(new MovementStrategyZigZagDown());
        }
    }
}