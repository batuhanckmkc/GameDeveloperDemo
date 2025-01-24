using System;
using DG.Tweening;
using GameDeveloperDemo.Model;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace GameDeveloperDemo.View
{
    public class WheelView : MonoBehaviour
    {
        [SerializeField] private Image wheel; 
        [SerializeField] private Image pointer;
        [SerializeField] private SpinButton spinButton;
        [SerializeField] private ZoneDataSO zoneDataSo;
        private int _lastSliceIndex = -1;

        #region Animation Values

        private const float CircleAngle = 360f;
        private const float PointerMoveAngle = 45f;
        private const float TargetFPS = 60f;

        #endregion
        
        public void Initialize(Action onSpinButtonClicked)
        {
            spinButton.RegisterListener(onSpinButtonClicked);
            SetView(zoneDataSo.GetZoneData(ZoneType.SafeZone));
        }

        public void Deinitialize(Action onSpinButtonClicked)
        {
            spinButton.UnregisterListener(onSpinButtonClicked);
        }

        public void RotateWheel(float sliceAngle, float finalAngle, float duration, Action onComplete)
        {
            wheel.transform.DORotate(new Vector3(0, 0, -finalAngle), duration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutCubic)
                .OnUpdate(()=> AnimatePointer(sliceAngle, duration))
                .OnComplete(()=> onComplete?.Invoke());
        }
        
        private void AnimatePointer(float sliceAngle, float duration)
        {
            //float currentFPS = 1f / Time.deltaTime;
            float currentAngle = wheel.transform.rotation.eulerAngles.z % CircleAngle;
            int currentSliceIndex = Mathf.FloorToInt(currentAngle / sliceAngle);
            if (currentSliceIndex != _lastSliceIndex)
            {
                _lastSliceIndex = currentSliceIndex;
                float speed = Mathf.Abs(sliceAngle / (duration * TargetFPS));
                pointer.transform.DORotate(new Vector3(0, 0, -PointerMoveAngle), speed)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        pointer.transform.DORotate(Vector3.zero, speed).SetEase(Ease.InCubic);
                    });
            }
        }

        private void SetView(ZoneData zoneData)
        {
            wheel.sprite = zoneData.wheelSprite;
            pointer.sprite = zoneData.pointerSprite;
        }
        
        public void OpenSpinButton() => spinButton.button.interactable = true;
        public void CloseSpinButton() => spinButton.button.interactable = false;
    }
}