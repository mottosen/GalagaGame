using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using GalagaGame.MovementStrategies;

namespace GalagaGame.Squadrons {
    public interface ISquadron {
        EntityContainer<Enemy> Enemies { get; }

        void CreateEnemies();
        bool CleanEnemies();
        void MoveEnemies();
    }
}