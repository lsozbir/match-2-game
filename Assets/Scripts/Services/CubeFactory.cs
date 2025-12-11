using System;
using System.Collections.Generic;
using Cubes;
using UnityEngine;
using Random = System.Random;

namespace Services
{
    public class CubeFactory : MonoBehaviour
    {
        [SerializeField] private GameObject redCubePrefab;
        [SerializeField] private GameObject blueCubePrefab;
        [SerializeField] private GameObject greenCubePrefab;
        [SerializeField] private GameObject yellowCubePrefab;
        [SerializeField] private GameObject purpleCubePrefab;
        [SerializeField] private GameObject duckCubePrefab;
        [SerializeField] private GameObject balloonCubePrefab;
        [SerializeField] private GameObject verticalRocketCubePrefab;
        [SerializeField] private GameObject horizontalRocketCubePrefab;
        private List<GameObject> _coloredCubes;
        private List<GameObject> _specialCubes;
        private Dictionary<CubeType, GameObject> _cubePrefabMap;
        
        private void Awake()
        {
            _coloredCubes = new List<GameObject>
            {
                redCubePrefab,
                blueCubePrefab,
                greenCubePrefab,
                //yellowCubePrefab,
                //purpleCubePrefab
            };
            
            _specialCubes = new List<GameObject>
            {
                duckCubePrefab,
                balloonCubePrefab,
            };
            
            _cubePrefabMap = new Dictionary<CubeType, GameObject>
            {
                { CubeType.Red, redCubePrefab },
                { CubeType.Blue, blueCubePrefab },
                { CubeType.Green, greenCubePrefab },
                { CubeType.Yellow, yellowCubePrefab },
                { CubeType.Purple, purpleCubePrefab },
                { CubeType.Duck, duckCubePrefab },
                { CubeType.Balloon, balloonCubePrefab },
                { CubeType.VerticalRocket, verticalRocketCubePrefab },
                { CubeType.HorizontalRocket, horizontalRocketCubePrefab }
            };
        }
        
        public GameObject CreateRandomColoredCube(Transform parent = null)
        {
            return Instantiate(_coloredCubes[new Random().Next(0, _coloredCubes.Count)], parent, false);
        }
        
        public GameObject CreateSpecialCube(Transform parent = null)
        {
            return Instantiate(_specialCubes[new Random().Next(0, _specialCubes.Count)], parent, false);
        }

        public GameObject CreateRandomCube(Transform parent = null)
        {
            return new Random().Next(0, 2) == 0 ? CreateRandomColoredCube(parent) : CreateSpecialCube(parent);
        }

        public GameObject CreateCubeWithType(CubeType cubeType, int x, int y, Transform parent = null)
        { 
            _cubePrefabMap.TryGetValue(cubeType, out var value);
            if (value == null) return null;
            var spawnedObject = Instantiate(value, parent, false);
            var cube = spawnedObject.GetComponent<Cube>();
            cube.LocationInGrid = new Vector2Int(x, y);
            return spawnedObject;
        }

        public Sprite ReturnSpriteOfType(CubeType cubeType)
        {
            _cubePrefabMap.TryGetValue(cubeType, out var value);
            return value == null ? null : value.GetComponentInChildren<SpriteRenderer>().sprite;
        }
        
    }
}