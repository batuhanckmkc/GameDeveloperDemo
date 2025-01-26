using GameDeveloperDemo.Controller;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Factories
{
    public class StorageRewardFactory : IRewardFactory
    {
        private readonly StorageRewardItem _storageRewardItem;
        public StorageRewardFactory(StorageRewardItem storageRewardItem)
        {
            _storageRewardItem = storageRewardItem;
        }

        public RewardItem CreateReward(ZoneRewardData zoneRewardData)
        {
            StorageRewardItem storageRewardItem = Object.Instantiate(_storageRewardItem);
            storageRewardItem.InjectData(zoneRewardData);
            return storageRewardItem;
        }
    }
}