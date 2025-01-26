using System;
using System.Collections.Generic;
using DG.Tweening;
using GameDeveloperDemo.Controller.Factory;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.Model.Data;
using GameDeveloperDemo.Model.Enum;
using GameDeveloperDemo.ScriptableObjects;
using GameDeveloperDemo.View;
using GameDeveloperDemo.View.RewardItem;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class RewardStorageController : MonoBehaviour
    {
        private RewardsDataSO _rewardsDataSo;
        private RewardStorageView _rewardStorageView;
        private Transform _rewardItemSpawnTransform;
        private RewardFactory _rewardFactory;
        
        private readonly Dictionary<RewardType, StorageRewardItem> _rewardStorageDictionary = new();
        public static event Action OnExit;
        public static event Action<Dictionary<RewardType, StorageRewardItem>> OnTakeRewards;

        public void Initialize(RewardFactory rewardFactory, RewardStorageView rewardStorageView, RewardsDataSO rewardsDataSo, Transform rewardItemSpawnTransform)
        {
            _rewardFactory = rewardFactory;
            _rewardsDataSo = rewardsDataSo;
            _rewardStorageView = rewardStorageView;
            _rewardItemSpawnTransform = rewardItemSpawnTransform;
            _rewardStorageView.Initialize(OnExitButtonClicked);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void OnDestroy()
        {
            _rewardStorageView.Deinitialize(OnExitButtonClicked);
        }

        private void SubscribeEvents()
        {
            WheelController.OnSpinClick += OnSpinClick;
            WheelController.OnSpinComplete += ProcessReward;
            ZoneController.OnZoneChange += OnZoneChange;
            ReviveScreenView.OnGiveUp += ClearAllRewards;
        }

        private void UnsubscribeEvents()
        {
            WheelController.OnSpinClick -= OnSpinClick;
            WheelController.OnSpinComplete -= ProcessReward;
            ZoneController.OnZoneChange -= OnZoneChange;
            ReviveScreenView.OnGiveUp -= ClearAllRewards;
        }

        private void ProcessReward(ZoneRewardData reward)
        {
            if (reward.rewardConfigurationData.rewardType == RewardType.Bomb)
                return;

            UpdateOrCreateReward(reward);
            var rewardVisualList = _rewardFactory.CreateMultipleRewards<FlyingRewardItem>(reward, RewardItemType.Fly,GetRewardFlyAmount(reward));
            AnimateFlyingRewards(rewardVisualList);
        }

        private void UpdateOrCreateReward(ZoneRewardData reward)
        {
            if (_rewardStorageDictionary.TryGetValue(reward.rewardConfigurationData.rewardType, out var existingReward))
            {
                existingReward.ZoneRewardData.amount += reward.amount;
                existingReward.SetRewardUI(_rewardsDataSo.GetSprite(reward.rewardConfigurationData.rewardType));
            }
            else
            {
                var newRewardItem = _rewardFactory.CreateReward<StorageRewardItem>(reward, RewardItemType.Storage);
                newRewardItem.InjectData(reward);
                newRewardItem.SetRewardUI(_rewardsDataSo.GetSprite(reward.rewardConfigurationData.rewardType));
                _rewardStorageView.AddRewardItem(newRewardItem);

                _rewardStorageDictionary[reward.rewardConfigurationData.rewardType] = newRewardItem;
            }
        }

        private void AnimateFlyingRewards(List<FlyingRewardItem> rewardVisualList)
        {
            float flyInterval = 0.15f;
            var sequence = DOTween.Sequence();

            foreach (var flyingReward in rewardVisualList)
            {
                flyingReward.SetRewardUI(_rewardsDataSo.GetSprite(flyingReward.ZoneRewardData.rewardConfigurationData.rewardType));
                flyingReward.transform.SetParent(_rewardStorageView.ParentTransform);
                flyingReward.transform.position = _rewardItemSpawnTransform.position;
                flyingReward.transform.SetAsLastSibling();
                sequence.PrependInterval(flyInterval);
                sequence.Join(flyingReward.ScaleUp());
                sequence.Join(flyingReward.Fly(_rewardStorageView.ParentTransform.position));
            }
        }

        private int GetRewardFlyAmount(ZoneRewardData zoneRewardData)
        {
            const int rewardFlyLimit = 5;
            return Mathf.Min(zoneRewardData.amount, rewardFlyLimit);
        }
        
        private void ClearAllRewards()
        {
            _rewardStorageView.ClearItems();
            _rewardStorageDictionary.Clear();
        }
        
        private void OnExitButtonClicked()
        {
            OnTakeRewards?.Invoke(_rewardStorageDictionary);
            OnExit?.Invoke();
            ClearAllRewards();
        }
        
        private void OnZoneChange(ZoneModel zoneModel)
        {
            _rewardStorageView.UpdateExitButtonState(zoneModel.ZoneData.zoneType is ZoneType.SafeZone or ZoneType.SuperZone);
        }

        private void OnSpinClick()
        {
            _rewardStorageView.UpdateExitButtonState(false);
        }
    }
}
