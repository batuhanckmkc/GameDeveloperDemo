using GameDeveloperDemo.Controller;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Factories
{
    public class WheelRewardFactory : IRewardFactory
    {
        private readonly WheelRewardItem _wheelRewardItemPrefab;

        public WheelRewardFactory(WheelRewardItem wheelRewardItemPrefab)
        {
            _wheelRewardItemPrefab = wheelRewardItemPrefab;
        }

        public RewardItem CreateReward(ZoneRewardData zoneRewardData)
        {
            WheelRewardItem wheelRewardItem = Object.Instantiate(_wheelRewardItemPrefab);
            wheelRewardItem.InjectData(zoneRewardData);
            return wheelRewardItem;
        }
    }
}