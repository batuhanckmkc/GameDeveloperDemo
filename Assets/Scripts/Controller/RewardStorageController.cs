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
        private RewardStorageView _rewardStorageView;
        private Transform _parentTransform;
        private Transform _rewardItemSpawnTransform;
        public void Initialize(FlyingRewardFactory flyingRewardFactory, RewardStorageView rewardStorageView, RewardsDataSO rewardsDataSo, Transform parentTransform, Transform rewardItemSpawnTransform)
        {
            _rewardsDataSo = rewardsDataSo;
            _parentTransform = parentTransform;
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
            if(reward.rewardConfigurationData.rewardType == RewardType.Bomb)
                return;


            float flyInterval = 0.15f;
            var sequence = DOTween.Sequence();
            var rewardItemList = _flyingRewardFactory.CreateMultipleRewards(reward, GetRewardFlyAmount(reward));
            for (int i = 0; i < rewardItemList.Count; i++)
            {
                rewardItemList[i].transform.SetParent(_parentTransform);
                rewardItemList[i].transform.position = _rewardItemSpawnTransform.position;
                rewardItemList[i].transform.SetAsLastSibling();
                rewardItemList[i].SetSprite(_rewardsDataSo.GetSprite(reward.rewardConfigurationData.rewardType));
                rewardItemList[i].SetAmount(i == 0);

                sequence.PrependInterval(flyInterval);
                sequence.Join(rewardItemList[i].ScaleUp());
                sequence.Join(rewardItemList[i].Fly(_rewardStorageView.Container.position));
            }
        }

        private int GetRewardFlyAmount(ZoneRewardData zoneRewardData)
        {
            var flyAmount = zoneRewardData.rewardConfigurationData.rewardType == RewardType.Gold || zoneRewardData.amount >= 5
                ? 5
                : zoneRewardData.amount;
            return flyAmount;
        }
    }
}