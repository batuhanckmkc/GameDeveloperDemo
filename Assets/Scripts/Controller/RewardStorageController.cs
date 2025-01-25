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

            var rewardItem = _flyingRewardFactory.CreateReward(reward);
            rewardItem.transform.SetParent(_parentTransform);
            rewardItem.transform.position = _rewardItemSpawnTransform.position;
            rewardItem.SetRewardUI(_rewardsDataSo.GetSprite(reward.rewardConfigurationData.rewardType));
            AnimateReward(rewardItem);
        }

        private void AnimateReward(RewardItem rewardItem)
        {
            var animationDuration = 0.65f; 
            rewardItem.transform.DOMove(_rewardStorageView.Container.position, animationDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    Debug.Log("Reward animation completed.");
                    Destroy(rewardItem.gameObject);
                });
        }
    }
}