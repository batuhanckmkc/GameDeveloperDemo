using System;
using System.Collections.Generic;
using GameDeveloperDemo.Controller;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Model
{
    public class InventoryModel : IDisposable
    {
        private readonly Dictionary<RewardType, ZoneRewardData> _inventoryDictionary = new();

        public InventoryModel()
        {
            SubscribeEvents();
        }
        
        public void Dispose()
        {
            UnsubscribeEvents();
            ClearInventory();
        }

        private void SubscribeEvents()
        {
            RewardStorageController.OnTakeRewards += UpdateInventory;
            ReviveScreenView.OnGiveUp += ClearInventory;
        }
        
        private void UnsubscribeEvents()
        {
            RewardStorageController.OnTakeRewards -= UpdateInventory;
            ReviveScreenView.OnGiveUp -= ClearInventory;
        }

        private void UpdateInventory(Dictionary<RewardType, StorageRewardItem> rewardStorageDictionary)
        {
            foreach (var rewardItem in rewardStorageDictionary.Values)
            {
                ZoneRewardData rewardData = rewardItem.ZoneRewardData;

                if (_inventoryDictionary.TryGetValue(rewardData.rewardConfigurationData.rewardType, out var existingReward))
                {
                    existingReward.amount += rewardData.amount;
                }
                else
                {
                    _inventoryDictionary[rewardData.rewardConfigurationData.rewardType] = new ZoneRewardData
                    {
                        rewardConfigurationData = rewardData.rewardConfigurationData,
                        amount = rewardData.amount
                    };
                }
            }
            Debug.Log("Inventory updated with rewards.");
        }

        public Dictionary<RewardType, ZoneRewardData> GetInventory()
        {
            return _inventoryDictionary;
        }

        private void ClearInventory()
        {
            _inventoryDictionary.Clear();
            Debug.Log("Inventory cleared.");
        }
    }
}
