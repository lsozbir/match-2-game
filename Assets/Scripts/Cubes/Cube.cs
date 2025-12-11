using System;
using DG.Tweening;
using Grid;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using Services;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cubes
{
    public abstract class Cube : MonoBehaviour
    {
        [Inject] protected GridData GridData;
        [Inject] protected GridController GridController;
        [Inject] protected MoveManager MoveManager;
        [Inject] protected GoalManager GoalManager;
        [Inject] protected GoalView GoalView;
        [Inject] protected AudioManager AudioManager;
        [Inject] protected ParticleFactory ParticleFactory;
        [SerializeField] protected CubeType cubeType;
        private SpriteRenderer[] _spriteRenderers;
        
        // Current location in grid
        [field: SerializeField] public Vector2Int LocationInGrid { get; set; } = new (-1, -1);
        
        // How many blocks should this cube fall?
        [field: SerializeField] public int BlocksToFall { get; set; }

        protected void Awake()
        {
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            AttributeInjector.Inject(this, gameObject.scene.GetSceneContainer());
        }

        protected virtual void DestroyCube()
        {
            GridData.SetCubeAtIndex(null, LocationInGrid.x, LocationInGrid.y);
            
            if (GoalManager.CanSubtractFromGoal(cubeType))
            {
                SubtractFromGoalAndDestroyGameObject();
            }

            else
            {
                Destroy(gameObject);
                GoalManager.SubtractFromGoal(cubeType);
            }
        }

        // What should happen when cube is damaged? (e.g. rocket hit)
        public virtual void DamageCube() {}
        
        // What should happen when player clicks cube?
        public virtual void OnCubeClicked() { }
        
        // What should happen when an adjacent color cube is destroyed?
        public virtual void OnAdjacentMatchHappened() { }

        // UpdateCube is called between player input so cube can update itself (e.g. duck checks if it is in bottom layer)
        public virtual void UpdateCube() {}
        
        private void SubtractFromGoalAndDestroyGameObject()
        {
            var animationDuration = 1f;
            
            // Add renderers of this object to foremost sorting layer so it does not go behind UI.
            if(_spriteRenderers != null)
                foreach (var sr in _spriteRenderers)
                {
                    sr.sortingLayerName = "Foremost";
                }
            
            var sequence = DOTween.Sequence();
            sequence.Join(gameObject.transform.DOMove(GoalView.GetPositionOfGoalElementFromType(cubeType), animationDuration));
            sequence.Join(gameObject.transform.DOScale(0.4f, animationDuration));
            
            sequence.OnComplete(() =>
            {
                GoalManager.SubtractFromGoal(cubeType);
                AudioManager.PlayCubeCollectSound();
                Destroy(gameObject);
            });
        }
    }
}
