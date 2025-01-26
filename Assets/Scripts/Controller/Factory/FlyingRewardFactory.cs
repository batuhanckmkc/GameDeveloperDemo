using System.Collections.Generic;
using GameDeveloperDemo.Controller;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Factories
{
    public class FlyingRewardFactory : IRewardFactory
    {
        private readonly FlyingWheelRewardItem _flyingRewardPrefab;
        public FlyingRewardFactory(FlyingWheelRewardItem flyingRewardPrefab)
        {
            _flyingRewardPrefab = flyingRewardPrefab;
        }

        public RewardItem CreateReward(ZoneRewardData zoneRewardData)
        {
            FlyingWheelRewardItem flyingReward = Object.Instantiate(_flyingRewardPrefab);
            flyingReward.InjectData(zoneRewardData);
            return flyingReward;
        }
        
        public List<FlyingWheelRewardItem> CreateMultipleRewards(ZoneRewardData zoneRewardData, int createCount)
        {
            List<FlyingWheelRewardItem> flyingWheelRewardItems = new List<FlyingWheelRewardItem>();
            for (int i = 0; i < createCount; i++)
            {
                var flyingReward = CreateReward(zoneRewardData) as FlyingWheelRewardItem;
                flyingWheelRewardItems.Add(flyingReward);
            }
            return flyingWheelRewardItems;
        }
    }
}