using DG.Tweening;
using Reflex.Extensions;
using Reflex.Injectors;
using Unity.VisualScripting;
using UnityEngine;

namespace Cubes
{
    public class BalloonCube : Cube
    {
        [SerializeField] private SpriteRenderer balloonSpriteRenderer;
        private bool _isPopping = false;
        public float popDuration = 0.2f;
        public float popScaleMultiplier = 1.2f;
        
        public override void DamageCube()
        {
            Pop();
        }
        
        public override void OnAdjacentMatchHappened()
        {
            Pop();
        }

        private void Pop()
        {
            if (_isPopping) return;
            _isPopping = true;
            var popSequence = DOTween.Sequence();
            popSequence.Append(transform.DOScale(transform.localScale * popScaleMultiplier, popDuration / 2f)
                .SetEase(Ease.OutBack));
            popSequence.Join(balloonSpriteRenderer.DOFade(0.25f, popDuration));

            popSequence.OnStart(() =>
            {
                AudioManager.PlayBalloonSound();
                GridController.AddAnimation();
            });
            popSequence.OnComplete(() =>
            {
                base.DestroyCube();
                GridController.EndAnimation();
                GridController.IsGridDirty = true;
            });
        }
    }
}
