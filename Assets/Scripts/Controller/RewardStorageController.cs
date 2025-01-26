using System.Collections.Generic;
using DG.Tweening;
using GameDeveloperDemo.Factories;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.ScriptableObjects;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class RewardStorageController : MonoBehaviour
    {
        private RewardsDataSO _rewardsDataSo;
        private FlyingRewardFactory _flyingRewardFactory;
        private StorageRewardFactory _storageRewardFactory;
        private RewardStorageView _rewardStorageView;
        private Transform _rewardItemSpawnTransform;

        private readonly Dictionary<RewardType, StorageRewardItem> _rewardStorageDictionary = new();

        public void Initialize(
            FlyingRewardFactory flyingRewardFactory,
            StorageRewardFactory storageRewardFactory,
            RewardStorageView rewardStorageView,
            RewardsDataSO rewardsDataSo,
            Transform rewardItemSpawnTransform)
        {
            _rewardsDataSo = rewardsDataSo;
            _storageRewardFactory = storageRewardFactory;
            _flyingRewardFactory = flyingRewardFactory;
            _rewardStorageView = rewardStorageView;
            _rewardItemSpawnTransform = rewardItemSpawnTransform;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            WheelController.OnSpinComplete += ProcessReward;
        }

        private void UnsubscribeEvents()
        {
            WheelController.OnSpinComplete -= ProcessReward;
        }

        private void ProcessReward(ZoneRewardData reward)
        {
            if (reward.rewardConfigurationData.rewardType == RewardType.Bomb)
                return;

            UpdateOrCreateReward(reward);
            var rewardVisualList = _flyingRewardFactory.CreateMultipleRewards(reward, GetRewardFlyAmount(reward));
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
                var newRewardItem = _storageRewardFactory.CreateReward(reward) as StorageRewardItem;
                newRewardItem.InjectData(reward);
                newRewardItem.SetRewardUI(_rewardsDataSo.GetSprite(reward.rewardConfigurationData.rewardType));
                _rewardStorageView.AddRewardItem(newRewardItem);

                _rewardStorageDictionary[reward.rewardConfigurationData.rewardType] = newRewardItem;
            }
        }

        private void AnimateFlyingRewards(List<FlyingWheelRewardItem> rewardVisualList)
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
                sequence.Join(flyingReward.Fly(_rewardStorageView.Container.position));
            }
        }

        private int GetRewardFlyAmount(ZoneRewardData zoneRewardData)
        {
            const int rewardFlyLimit = 5;
            return Mathf.Min(zoneRewardData.amount, rewardFlyLimit);
        }
    }
}
