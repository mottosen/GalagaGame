namespace GalagaGame.Levels {
    public class Level_2 : LevelAbstract {
        public Level_2() {
            EnemySpeed = 0.001f;
            StrongEnemyRatio = 20;
            
            // Squadrons
            PossibleSquadrons = new [] {
                SquadronTypes.Box,
                SquadronTypes.Diamonds
            };
            
            // Movement Strategies
            PossibleMovements = new[] {
                MovementTypes.NoMove,
                MovementTypes.ZigZag
            };
        }
    }
}