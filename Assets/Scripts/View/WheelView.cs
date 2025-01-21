using System;
using DG.Tweening;
using UnityEngine;

namespace GameDeveloperDemo.View
{
    public class WheelView : MonoBehaviour
    {
        [SerializeField] private Transform wheel; 
        [SerializeField] private Transform indicator;
        [SerializeField] private SpinButton spinButton;
        private int _lastSliceIndex = -1;

        #region Animation Values

        private const float CircleAngle = 360f;
        private const float IndicatorMoveAngle = 45f;
        private const float TargetFPS = 60f;

        #endregion
        
        public void Initialize(Action onSpinButtonClicked)
        {
            spinButton.RegisterListener(onSpinButtonClicked);
        }

        public void DeInitialize(Action onSpinButtonClicked)
        {
            spinButton.UnregisterListener(onSpinButtonClicked);
        }

        public void RotateWheel(float sliceAngle, float finalAngle, float duration, Action onComplete)
        {
            wheel.DORotate(new Vector3(0, 0, -finalAngle), duration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutCubic)
                .OnUpdate(()=> AnimateIndicator(sliceAngle, duration))
                .OnComplete(()=> onComplete?.Invoke());
        }
        
        private void AnimateIndicator(float sliceAngle, float duration)
        {
            //float currentFPS = 1f / Time.deltaTime;
            float currentAngle = wheel.rotation.eulerAngles.z % CircleAngle;
            int currentSliceIndex = Mathf.FloorToInt(currentAngle / sliceAngle);
            if (currentSliceIndex != _lastSliceIndex)
            {
                _lastSliceIndex = currentSliceIndex;
                float speed = Mathf.Abs(sliceAngle / (duration * TargetFPS));
                indicator.DORotate(new Vector3(0, 0, -IndicatorMoveAngle), speed)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        indicator.DORotate(Vector3.zero, speed).SetEase(Ease.InCubic);
                    });
            }
        }
    }
}