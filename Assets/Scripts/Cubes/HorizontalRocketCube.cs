using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Cubes
{
    public class HorizontalRocketCube : Cube
    {
        [SerializeField] private GameObject horizontalRocketCubeLeftGameObject;
        [SerializeField] private GameObject horizontalRocketCubeRightGameObject;
        private bool isActive = false;
        private const float FlyDuration = 1f;
        private bool _isLeftAnimationFinished;
        private bool _isRightAnimationFinished;

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
    
        private enum Direction { Right, Left }
    
        private void Fly()
        {
            if(isActive) return;
            isActive = true;
            // Set Z to max so rocket stays in front of other cubes
            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
            FlyRocket(Direction.Left);
            FlyRocket(Direction.Right);
        }

        private IEnumerator DestroyCubesHorizontally(Direction direction)
        {
            var amountOfCubesToDestroy = direction == Direction.Left ? 
                LocationInGrid.x : 
                GridData.GridWidth - 1 - LocationInGrid.x;
        
            var cubesDestroyed = 0;
        
            while (cubesDestroyed < amountOfCubesToDestroy)
            {
                var indexToDestroy = direction == Direction.Left ? LocationInGrid.x - ++cubesDestroyed : 
                    LocationInGrid.x + ++cubesDestroyed;
            
                GridData.GetCubeAtIndex(indexToDestroy, LocationInGrid.y)?.DamageCube();
                yield return new WaitForSeconds(FlyDuration / amountOfCubesToDestroy);
            }
        }

        private void FlyRocket(Direction direction)
        {
            var destination = direction == Direction.Left ? 
                transform.position - new Vector3(LocationInGrid.x + 0.5f, 0, 0):
                transform.position + new Vector3(GridData.GridWidth - LocationInGrid.x - 0.5f, 0, 0);
        
            var tween = direction == Direction.Left ? 
                horizontalRocketCubeLeftGameObject.transform.DOMove(destination, FlyDuration).SetEase(Ease.Linear):
                horizontalRocketCubeRightGameObject.transform.DOMove(destination, FlyDuration).SetEase(Ease.Linear);
        
            tween.OnStart(() =>
            {
                StartCoroutine(DestroyCubesHorizontally(direction));
                GridController.AddAnimation();
            });
            tween.OnComplete(() =>
            {
                if(direction == Direction.Left) _isLeftAnimationFinished = true;
                else _isRightAnimationFinished = true;
                GridController.EndAnimation();

                if (_isLeftAnimationFinished && _isRightAnimationFinished)
                {
                    base.DestroyCube();
                }
                    
            });
        }
    
    }
}
