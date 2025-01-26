using System;
using System.Collections.Generic;
using DG.Tweening;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.ScriptableObjects;
using GameDeveloperDemo.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace GameDeveloperDemo.View
{
    public class WheelView : MonoBehaviour
    {
        [SerializeField] private Image wheel; 
        [SerializeField] private Image pointer;
        [SerializeField] private SpinButton spinButton;
        public Transform RewardItemSpawnTransform => pointer.transform;
        public Transform RewardItemParent => wheel.transform;
        private int _lastSliceIndex = -1;
        private List<RewardItem> _rewardItems = new();
        private RewardsDataSO _rewardsDataSo;
        
        #region Animation Values

        private const float PointerMoveAngle = 45f;
        private const float TargetFPS = 60f;

        #endregion

        public void Initialize(Action onSpinButtonClicked, RewardsDataSO rewardsDataSo)
        {
            spinButton.RegisterListener(onSpinButtonClicked);
            _rewardsDataSo = rewardsDataSo;
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
            float currentAngle = wheel.transform.rotation.eulerAngles.z % Constants.CircleAngle;
            int currentSliceIndex = Mathf.FloorToInt(currentAngle / sliceAngle);
            if (currentSliceIndex != _lastSliceIndex)
            {
                _lastSliceIndex = currentSliceIndex;
                float speed = Mathf.Abs(sliceAngle / (duration * TargetFPS));
                pointer.transform.DORotate(new Vector3(0, 0, PointerMoveAngle), speed)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        pointer.transform.DORotate(Vector3.zero, speed).SetEase(Ease.InCubic);
                    });
            }
        }

        public ZoneRewardData GetRewardAtIndex(int rewardIndex)
        {
            return _rewardItems[rewardIndex].ZoneRewardData;
        }
        
        public void SetRewardItems(List<RewardItem> rewardItems)
        {
            _rewardItems = rewardItems;
        }
        
        public void SetView(ZoneModel zoneModel)
        {
            wheel.sprite = zoneModel.ZoneData.wheelSprite;
            pointer.sprite = zoneModel.ZoneData.pointerSprite;
            UpdateRewardItemsView(zoneModel);
        }

        private void UpdateRewardItemsView(ZoneModel zoneModel)
        {
            List<ZoneRewardData> rewards = zoneModel.ZoneData.zoneRewardsDataSo.RewardModels;
            for (int i = 0; i < _rewardItems.Count; i++)
            {
                var betterRewardInstance = new ZoneRewardData
                {
                    rewardConfigurationData = rewards[i].rewardConfigurationData,
                    amount = rewards[i].amount
                };

                betterRewardInstance.amount += zoneModel.CurrentZoneIndex / zoneModel.ZoneData.activationAmount - 1;
                
                _rewardItems[i].InjectData(betterRewardInstance);
                _rewardItems[i].SetRewardUI(_rewardsDataSo.GetSprite(betterRewardInstance.rewardConfigurationData.rewardType));
            }
        }

        public void ResetView()
        {
            wheel.transform.rotation = Quaternion.identity;
        }
        
        public void OpenSpinButton() => spinButton.button.interactable = true;
        public void CloseSpinButton() => spinButton.button.interactable = false;
    }
}