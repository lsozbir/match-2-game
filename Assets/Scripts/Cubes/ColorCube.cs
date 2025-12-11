using System;
using System.Collections.Generic;
using Grid;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using Services;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

namespace Cubes
{
    public class ColorCube : Cube
    {
        protected override void DestroyCube()
        {
            AudioManager.PlayCubeExplodeSound();
            ParticleFactory.CreateCubeExplosionParticle(cubeType, this.transform.position);
            base.DestroyCube();
        }

        public override void OnCubeClicked()
        {
            CheckNeighbours(null, this);
        }

        public override void DamageCube()
        {
            DestroyCube();
        }

        private void CheckNeighbours(HashSet<ColorCube> visited, ColorCube clickedCube)
        {
            // Recursion protection
            visited ??= new HashSet<ColorCube>();
            if (!visited.Add(this)) return;
            
            // Left
            if (GridData.IsIndexIsValid(LocationInGrid.x - 1, LocationInGrid.y)
                && GridData.GetCubeAtIndex(LocationInGrid.x - 1, LocationInGrid.y) is ColorCube leftCube
                && leftCube.cubeType == this.cubeType)
            {
                leftCube.CheckNeighbours(visited, clickedCube);
            }

            // Right
            if (GridData.IsIndexIsValid(LocationInGrid.x + 1, LocationInGrid.y)
                && GridData.GetCubeAtIndex(LocationInGrid.x + 1, LocationInGrid.y) is ColorCube rightCube
                && rightCube.cubeType == this.cubeType)
            {
                rightCube.CheckNeighbours(visited, clickedCube);
            }

            // Down
            if (GridData.IsIndexIsValid(LocationInGrid.x, LocationInGrid.y - 1)
                && GridData.GetCubeAtIndex(LocationInGrid.x, LocationInGrid.y - 1) is ColorCube downCube
                && downCube.cubeType == this.cubeType)
            {
                downCube.CheckNeighbours(visited, clickedCube);
            }

            // Up
            if (GridData.IsIndexIsValid(LocationInGrid.x, LocationInGrid.y + 1)
                && GridData.GetCubeAtIndex(LocationInGrid.x, LocationInGrid.y + 1) is ColorCube upCube
                && upCube.cubeType == this.cubeType)
            {
                upCube.CheckNeighbours(visited, clickedCube);
            }

            // Nothing happens if match is smaller than 2
            if (this != clickedCube || visited.Count < 2) return;

            foreach (var colorCube in visited)
            {
                NotifyAdjacentMatch(colorCube.LocationInGrid.x, colorCube.LocationInGrid.y);
                colorCube.DestroyCube();
            }
            
            // Spawn rocket if count greater than 4
            if (visited.Count > 4)
            {
                var t = new Random().Next(0, 2) == 0 ? CubeType.HorizontalRocket : CubeType.VerticalRocket;
                GridController.SpawnCubeAtIndex(t, LocationInGrid.x, LocationInGrid.y);
            }
            
            GridController.IsGridDirty = true;
            MoveManager.DecrementMoves();
        }

        private Type GetRandomRocketType()
        {
            var random = new Random().Next(0, 2);
            return random == 0 ? typeof(HorizontalRocketCube) : typeof(VerticalRocketCube);
        }

        private void NotifyAdjacentMatch(int x, int y)
        {
            // Up
            if (GridData.GetCubeAtIndex(x, y + 1) is { } upCube)
            {
                upCube.OnAdjacentMatchHappened();
            }

            // Down
            if (GridData.GetCubeAtIndex(x, y - 1) is { } downCube)
            {
                downCube.OnAdjacentMatchHappened();
            }

             // Right
            if (GridData.GetCubeAtIndex(x + 1, y) is { } rightCube)
            {
                rightCube.OnAdjacentMatchHappened();
            }

            // Left
            if (GridData.GetCubeAtIndex(x - 1, y) is { } leftCube)
            {
                leftCube.OnAdjacentMatchHappened();
            }
        }
    }
}