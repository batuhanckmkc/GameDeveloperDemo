using System;
using System.Collections.Generic;
using System.Linq;
using GameDeveloperDemo.Model.Data;
using GameDeveloperDemo.Model.Enum;
using GameDeveloperDemo.ScriptableObjects;
using GameDeveloperDemo.View.RewardItem;
using Object = UnityEngine.Object;

namespace GameDeveloperDemo.Controller.Factory
{
    public class RewardFactory
    {
        private readonly RewardItemPrefabDataSO _rewardItemPrefabDataSo;
        public RewardFactory(RewardItemPrefabDataSO rewardItemPrefabDataSo)
        {
            _rewardItemPrefabDataSo = rewardItemPrefabDataSo;
        }
        private RewardItem GetRewardItem(RewardItemType rewardItemType)
        {
            return _rewardItemPrefabDataSo.RewardItems.First(reward => reward.rewardItemType == rewardItemType).rewardItemPrefab;
        }
        
        public T CreateReward<T>(ZoneRewardData zoneRewardData, RewardItemType rewardItemType) where T : RewardItem
        {
            try
            {
                var rewardItemPrefab = GetRewardItem(rewardItemType);
                T rewardItem = Object.Instantiate((T)rewardItemPrefab);
                rewardItem.InjectData(zoneRewardData);
                return rewardItem;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public List<T> CreateMultipleRewards<T>(ZoneRewardData zoneRewardData, RewardItemType rewardItemType, int createCount) where T : RewardItem
        {
            List<T> rewardItems = new List<T>();
            for (int i = 0; i < createCount; i++)
            {
                var reward = CreateReward<T>(zoneRewardData, rewardItemType);
                rewardItems.Add((T)reward);
            }
            return rewardItems;
        }
    }
}