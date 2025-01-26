using System;
using System.Collections.Generic;
using DG.Tweening;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.Model.Data;
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
        [SerializeField] private Transform firstItemTransform;
        [SerializeField] private Button spinButton;

        public Transform RewardItemSpawnTransform => firstItemTransform;
        public Transform RewardItemParent => wheel.transform;
        private int _lastSliceIndex = -1;
        private List<RewardItem.RewardItem> _rewardItems = new();
        private RewardsDataSO _rewardsDataSo;
        
        #region Animation Values

        private const float PointerMoveAngle = 45f;
        private const float PointerSpeed = 100f;

        #endregion

        public void Initialize(Action onSpinButtonClicked, RewardsDataSO rewardsDataSo)
        {
            spinButton.onClick.AddListener(()=> onSpinButtonClicked?.Invoke());
            _rewardsDataSo = rewardsDataSo;
        }

        public void Deinitialize(Action onSpinButtonClicked)
        {
            spinButton.onClick.RemoveListener(()=> onSpinButtonClicked?.Invoke());
        }

        private void OnValidate()
        {
            AttachButtons();
        }

        private void AttachButtons()
        {
            if (spinButton == null)
                spinButton = transform.Find(Constants.ButtonPrefix + "spin")?.GetComponent<Button>();
        }

        public void RotateWheel(float sliceAngle, float finalAngle, float duration, Action onComplete)
        {
            wheel.transform.DORotate(new Vector3(0, 0, -finalAngle), duration, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutQuart)
                .OnUpdate(()=> AnimatePointer(sliceAngle, duration))
                .OnComplete(()=>
                {
                    pointer.DOKill();
                    onComplete?.Invoke();
                });
        }
        
        private void AnimatePointer(float sliceAngle, float duration)
        {
            float currentAngle = wheel.transform.rotation.eulerAngles.z % Constants.CircleAngle;
            int currentSliceIndex = Mathf.FloorToInt(currentAngle / sliceAngle);
            if (currentSliceIndex != _lastSliceIndex)
            {
                _lastSliceIndex = currentSliceIndex;
                float speed = Mathf.Abs(sliceAngle / (duration * PointerSpeed));
                pointer.transform.DORotate(new Vector3(0, 0, PointerMoveAngle), speed)
                    .SetEase(Ease.OutCirc)
                    .OnComplete(() =>
                    {
                        pointer.transform.DORotate(Vector3.zero, speed).SetEase(Ease.OutSine);
                    });
            }
        }

        public ZoneRewardData GetRewardAtIndex(int rewardIndex)
        {
            return _rewardItems[rewardIndex].ZoneRewardData;
        }
        
        public void SetRewardItems(List<RewardItem.RewardItem> rewardItems)
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
        
        public void OpenSpinButton() => spinButton.interactable = true;
        public void CloseSpinButton() => spinButton.interactable = false;
    }
}