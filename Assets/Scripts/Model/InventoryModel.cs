using System;
using System.Collections.Generic;
using GameDeveloperDemo.Controller;
using UnityEngine;

namespace GameDeveloperDemo.Model
{
    public class InventoryModel : IDisposable
    {
        private readonly List<ZoneRewardData> _inventory = new();

        public InventoryModel()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            WheelController.OnSpinComplete += AddReward;
        }

        private void UnsubscribeEvents()
        {
            WheelController.OnSpinComplete -= AddReward;
        }
        
        private void AddReward(ZoneRewardData reward)
        {
            _inventory.Add(reward);
            Debug.Log($"Added to inventory: {reward.rewardConfigurationData.rewardType} - {reward.amount}");
        }

        public List<ZoneRewardData> GetAllRewards()
        {
            return new List<ZoneRewardData>(_inventory);
        }

        private void ClearInventory()
        {
            _inventory.Clear();
            Debug.Log("Inventory cleared.");
        }

        public void Dispose()
        {
            ClearInventory();
            UnsubscribeEvents();
        }
    }
}