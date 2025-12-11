using Boot;
using Cubes;
using Reflex.Attributes;
using Services;
using UnityEngine;

namespace Grid
{
    public class GridData
    {
        [Inject] private CubeFactory _cubeFactory;
        [Inject] private GridController _gridController;
        [Inject] private LevelData _levelData;
        
        private Cube[,] _gridArray;
        private Cube[,] GridArray
        {
            get => _gridArray;
            set
            {
                _gridArray = value; 
                GridWidth = _gridArray.GetLength(0);
                GridHeight = _gridArray.GetLength(1);
            }
        }
        public int GridWidth { get; private set; }
        public int GridHeight { get; private set; }
        public int[] BlocksToCreate { get; private set; }
        
        // Here we swap the dimensions of the LevelStartingGrid so it feels natural when typing it in Text
        public void InitializeGrid()
        {
            GridArray = new Cube[_levelData.LevelStartingGrid.GetLength(1), 
                _levelData.LevelStartingGrid.GetLength(0)];
            BlocksToCreate = new int[GridWidth];
            
            for (var x = 0; x < GridWidth; x++)
            {
                for (var y = 0; y < GridHeight; y++)
                {
                    var cubeObject = 
                        _cubeFactory.CreateCubeWithType(_levelData.LevelStartingGrid[GridHeight - 1 - y, GridWidth - 1 - x], x, y);
                    var cube = cubeObject.GetComponent<Cube>();
                    SetCubeAtIndex(cube, x, y);
                }
            }
        }

        public void UpdateFallCounts()
        {
            for (var x = 0; x < GridWidth; x++)
            {
                var emptyBlocksBeneathCount = 0;
                for (var y = 0; y < GridHeight; y++)
                {
                    var cube = GetCubeAtIndex(x, y);
                    if (!cube) emptyBlocksBeneathCount++;
                    else cube.BlocksToFall = emptyBlocksBeneathCount;
                    
                    if (y == GridHeight - 1)
                    {
                        BlocksToCreate[x] = emptyBlocksBeneathCount;
                    }
                }
            }
        }
        
        public void SetCubeAtIndex(Cube cube, int x, int y)
        {
            if(!IsIndexIsValid(x, y)) return;
            _gridArray[x, y] = cube;
            if(cube) cube.LocationInGrid = new Vector2Int(x, y);
        }

        public Cube GetCubeAtIndex(int x, int y)
        {
            return !IsIndexIsValid(x, y) ? null : _gridArray[x, y];
        }

        public bool IsIndexIsValid(int x, int y)
        {
            return x >= 0 && y >= 0 && x < GridWidth && y < GridHeight;
        }

        public void UpdateAllCubes()
        {
            
            for (var y = 0; y < GridHeight; y++)
            for(var x = 0; x < GridWidth; x++)
            {
                if(GridArray[x, y])
                    GridArray[x, y].UpdateCube();
            }
                    
        }
    }
}
