using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Cubes
{
    public class VerticalRocketCube : Cube
    {
        [SerializeField] private GameObject verticalRocketCubeUpGameObject;
        [SerializeField] private GameObject verticalRocketCubeDownGameObject;
        private const float FlyDuration = 1f;
        private bool isActive;
        private bool _isUpAnimationFinished;
        private bool _isDownAnimationFinished;

        public override void OnCubeClicked()
        {
            MoveManager.DecrementMoves();
            Fly();
        }

        protected override void DestroyCube()
        {
            Fly();
        }
        
        public override void DamageCube()
        {
            Fly();
        }
    
        private enum Direction { Down, Up }
    
        private void Fly()
        {
            if(isActive) return;
            isActive = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
            FlyRocket(Direction.Up);
            FlyRocket(Direction.Down);
        }

        private IEnumerator DestroyCubesVertically(Direction direction)
        {
            var amountOfCubesToDestroy =
                direction == Direction.Up ? 
                    GridData.GridHeight - 1 - LocationInGrid.y : 
                    LocationInGrid.y;
        
            var cubesDestroyed = 0;
        
            while (cubesDestroyed < amountOfCubesToDestroy)
            {
                var indexToDestroy = direction == Direction.Up ? 
                    LocationInGrid.y + ++cubesDestroyed : 
                    LocationInGrid.y - ++cubesDestroyed;
            
                GridData.GetCubeAtIndex(LocationInGrid.x, indexToDestroy)?.DamageCube();
                yield return new WaitForSeconds(FlyDuration / amountOfCubesToDestroy);
            }
        }

        private void FlyRocket(Direction direction)
        {
            var destination = direction == Direction.Up ? 
                transform.position + new Vector3(0f, GridData.GridHeight - 0.5f - LocationInGrid.y, 0f):
                transform.position - new Vector3(0f, LocationInGrid.y + 0.5f, 0);
        
            var tween = direction == Direction.Up ? 
                verticalRocketCubeUpGameObject.transform.DOMove(destination, FlyDuration).SetEase(Ease.Linear):
                verticalRocketCubeDownGameObject.transform.DOMove(destination, FlyDuration).SetEase(Ease.Linear);
        
            tween.OnStart(() =>
            {
                StartCoroutine(DestroyCubesVertically(direction));
                GridController.AddAnimation();
            });
            tween.OnComplete(() =>
            {
                if(direction == Direction.Up) _isUpAnimationFinished = true;
                else _isDownAnimationFinished = true;
                GridController.EndAnimation();
            
                if (_isUpAnimationFinished && _isDownAnimationFinished)
                {
                    base.DestroyCube();
                }
            });
        }


    
    }
}
