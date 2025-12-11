using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Cubes
{
    public class DuckCube : Cube
    {
        private bool _isDuckAnimating = false;
        private const float RotateAngle = 30f;
        
        public override void UpdateCube()
        {
            if (LocationInGrid.y != 0 || _isDuckAnimating) return;
            _isDuckAnimating = true;

            var soundLength = AudioManager.PlayDuckSound();
            
            var swingSequence = DOTween.Sequence();
            swingSequence.Append(
                transform.DORotate(new Vector3(0, 0, -RotateAngle), soundLength * 1 / 3).SetEase(Ease.InOutSine)
            );
            swingSequence.Append(
                transform.DORotate(new Vector3(0, 0, RotateAngle), soundLength * 2 / 3).SetEase(Ease.InOutSine)
            );
            
            swingSequence.OnStart(() =>
            {
                GridController.AddAnimation();
            });
            swingSequence.OnComplete(() =>
            {
                GridController.EndAnimation();
                base.DestroyCube();
            });
        }
        
    }
}