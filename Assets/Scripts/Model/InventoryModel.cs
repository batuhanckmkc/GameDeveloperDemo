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
        public static int Gold;

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
            ReviveScreenView.OnRevive += SpendGold;
        }
        
        private void UnsubscribeEvents()
        {
            RewardStorageController.OnTakeRewards -= UpdateInventory;
            ReviveScreenView.OnGiveUp -= ClearInventory;
            ReviveScreenView.OnRevive -= SpendGold;
        }

        private void SpendGold(int cost)
        {
            if (Gold >= cost)
            {
                Gold -= cost;
                if (_inventoryDictionary.TryGetValue(RewardType.Gold, out var goldReward))
                {
                    goldReward.amount = Gold;
                }

                Debug.Log($"Spend successful! Remaining gold: {Gold}");
            }
            else
            {
                Debug.LogWarning("Not enough gold to spend!");
            }
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
                
                if (_inventoryDictionary.TryGetValue(RewardType.Gold, out var goldReward))
                {
                    Gold = goldReward.amount;
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
