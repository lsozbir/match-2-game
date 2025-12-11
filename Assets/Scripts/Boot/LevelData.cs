using System.Collections.Generic;
using Cubes;

namespace Boot
{
    public class LevelData
    {
        public CubeType[,] LevelStartingGrid { get; private set; }
        public int LevelMoveCount { get; private set; }
        public CubeType[] LevelSpawnableCubeTypes { get; private set; }
        public Dictionary<CubeType, int> LevelGoal { get; private set; }

        public LevelData(CubeType[,] levelStartingGrid, int levelMoveCount, 
            CubeType[] levelSpawnableCubeTypes, Dictionary<CubeType, int> levelGoal)
        {
            LevelStartingGrid = levelStartingGrid;
            LevelSpawnableCubeTypes = levelSpawnableCubeTypes;
            LevelGoal = levelGoal;
            LevelMoveCount = levelMoveCount;
        }
    }
}
