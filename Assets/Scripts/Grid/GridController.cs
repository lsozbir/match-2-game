using System;
using System.Collections;
using Boot;
using Cubes;
using DG.Tweening;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using Services;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Grid
{
    public class GridController : MonoBehaviour
    {
        [Inject] private CubeFactory _cubeFactory;
        [Inject] private CanvasService _canvasService;
        [Inject] private LevelData _levelData;
        [SerializeField] public GameObject gridWrapper;
        private SpriteRenderer _gridSpriteRenderer;
        private SpriteRenderer _gridWrapperSpriteRenderer;
        [Inject] private GridData _gridData;
        
        // Offsets and paddings
        private const float GridWrapperPaddingX = 0.2f;
        private const float GridWrapperPaddingY = 0.37f;
        private const float GridWrapperYOffset = 0.1f;

        private const float CubeFallSpeed = 10f;
        private Vector2 _gridSize;
        private float _stepSize;
        public int AnimationCount {get; private set;}

        public void AddAnimation()
        {
            AnimationCount++;
        }

        public void EndAnimation()
        {
            AnimationCount--;
            if(AnimationCount == 0) 
                IsGridDirty = true;
        }

        public bool IsGridDirty { get; set; }

        private void Awake()
        {
            _gridSpriteRenderer = GetComponent<SpriteRenderer>();
            _gridWrapperSpriteRenderer = gridWrapper.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            AttributeInjector.Inject(this, gameObject.scene.GetSceneContainer());
            _gridData.InitializeGrid();
            InitializeGrid();
        }

        private void Update()
        {
            if(IsGridDirty && AnimationCount == 0) UpdateGrid();
            //Debug.Log(AnimationCount);
        }

        private void InitializeGrid()
        {
            _gridSpriteRenderer.size = new Vector2(_gridData.GridWidth, _gridData.GridHeight);
            
            _gridWrapperSpriteRenderer.size = new Vector2(_gridData.GridWidth + GridWrapperPaddingX, 
                    _gridData.GridHeight + GridWrapperPaddingY);
            
            gridWrapper.transform.position += new Vector3(0, GridWrapperYOffset, 0);
            // Calculate Grid and Step Size
            _gridSize = _gridSpriteRenderer.size;
            _stepSize = _gridSize.x / _gridData.GridWidth;
            
            for (var y = 0; y < _gridData.GridHeight; y++)
            {
                for (var x = 0; x < _gridData.GridWidth; x++)
                {
                    _gridData.GetCubeAtIndex(x, y).transform.position = CalculateCubePositionAtIndex(x, y);
                }
            }
        }
        
        public void SpawnCubeAtIndex(CubeType cubeType, int x, int y)
        {
            var cubeObject = _cubeFactory.CreateCubeWithType(cubeType, x, y);
            if (!cubeObject)
            {
                Debug.LogWarning("Cube of type " + cubeType + " cannot be spawned!");
                return;
            }

            var cube = cubeObject.GetComponent<Cube>();
            _gridData.SetCubeAtIndex(cube, x, y);
            cubeObject.transform.position = CalculateCubePositionAtIndex(x, y);
        }

        private Vector3 CalculateCubePositionAtIndex(int x, int y)
        {
            var cubeXPosition = _stepSize * x - (_gridSize.x / 2) + _stepSize / 2;
            var cubeYPosition = _stepSize * y - (_gridSize.y / 2) + _stepSize / 2;
            var cubeZPosition = _gridData.GridHeight - y;
            return new Vector3(cubeXPosition, cubeYPosition, cubeZPosition);
        }

        private void FallAllCubes()
        {
            for (var y = 0; y < _gridData.GridHeight; y++)
            {
                for (var x = 0; x < _gridData.GridWidth; x++)
                {
                    var cube = _gridData.GetCubeAtIndex(x, y);
                    if (!cube) continue;
                    FallCube(cube);
                }
            }
        }

        private void UpdateGrid()
        {
            _gridData.UpdateFallCounts();
            _gridData.UpdateAllCubes();
            FallAllCubes();
            FillEmptySpacesWithCubes();
            IsGridDirty = false;
        }
        
        private void FillEmptySpacesWithCubes()
        {
            for (var x = 0; x < _gridData.GridWidth; x++)
            {
                for (var y = 0; y < _gridData.BlocksToCreate[x]; y++)
                {
                    if (_levelData.LevelSpawnableCubeTypes.Length == 0) return;
                    var cubeTypeToSpawn = 
                        _levelData.LevelSpawnableCubeTypes[new Random().Next(0, _levelData.LevelSpawnableCubeTypes.Length)];
                    
                    var cubeObject = _cubeFactory.CreateCubeWithType(cubeTypeToSpawn, x, y);
                    var cube = cubeObject.GetComponent<Cube>();
                    cube.LocationInGrid = new Vector2Int(x, _gridData.GridHeight + y);
                    cubeObject.transform.position = CalculateCubePositionAtIndex(x, cube.LocationInGrid.y);
                    
                    cube.BlocksToFall = _gridData.BlocksToCreate[x];
                    FallCube(cube);
                }
            }
        }
        
        // Falls a cube from its current location using cube.BlocksToFall
        private void FallCube(Cube cube)
        {
            if (cube.BlocksToFall == 0) return;
            
            var positionToFall = 
                CalculateCubePositionAtIndex(cube.LocationInGrid.x, cube.LocationInGrid.y - cube.BlocksToFall);
            _gridData.SetCubeAtIndex(null, cube.LocationInGrid.x, cube.LocationInGrid.y);
            _gridData.SetCubeAtIndex(cube, cube.LocationInGrid.x, cube.LocationInGrid.y - cube.BlocksToFall);
            
            // Animation
            var duration = Vector2.Distance(cube.transform.position, positionToFall) / CubeFallSpeed;
            var tween = cube.transform.DOMove(positionToFall, duration).SetEase(Ease.InQuad);
            tween.OnStart(() =>
            {
                AddAnimation();
            });
            tween.OnComplete(() =>
            {
                EndAnimation();
                cube.UpdateCube();
            });
        }
    }
}
