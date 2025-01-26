using System.Collections.Generic;
using GameDeveloperDemo.Controller.Factory;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.Model.Data;
using GameDeveloperDemo.Model.Enum;
using GameDeveloperDemo.ScriptableObjects;
using GameDeveloperDemo.View;
using GameDeveloperDemo.View.RewardItem;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class InventoryController : MonoBehaviour
    {
        private RewardsDataSO _rewardsDataSo;
        private RewardFactory _rewardFactory;
        private InventoryView _inventoryView;
        private InventoryModel _inventoryModel;

        private readonly Dictionary<RewardType, StorageRewardItem> _rewardItemViewDictionary = new();
        public void Initialize(InventoryView inventoryView, RewardFactory rewardFactory, RewardsDataSO rewardsDataSo, InventoryModel inventoryModel)
        {
            _inventoryView = inventoryView;
            _rewardFactory = rewardFactory;
            _rewardsDataSo = rewardsDataSo;
            _inventoryModel = inventoryModel;
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
            RewardStorageController.OnExit += UpdateInventory;
            ReviveScreenView.OnGiveUp += ClearItems;
            ReviveScreenView.OnRevive += OnRevive;
        }

        private void UnsubscribeEvents()
        {
            RewardStorageController.OnExit -= UpdateInventory;
            ReviveScreenView.OnGiveUp -= ClearItems;
            ReviveScreenView.OnRevive -= OnRevive;
        }

        private void UpdateInventory()
        {
            Dictionary<RewardType, ZoneRewardData> rewardDataCollection = _inventoryModel.GetInventory();
            foreach (var rewardData in rewardDataCollection.Values)
            {
                if (_rewardItemViewDictionary.TryGetValue(rewardData.rewardConfigurationData.rewardType, out var existingRewardItem))
                {
                    existingRewardItem.ZoneRewardData.amount = rewardData.amount;
                    existingRewardItem.SetRewardUI(_rewardsDataSo.GetSprite(rewardData.rewardConfigurationData.rewardType));
                }
                else
                {
                    var newRewardItem = _rewardFactory.CreateReward<StorageRewardItem>(rewardData, RewardItemType.Storage);
                    newRewardItem.InjectData(rewardData);
                    newRewardItem.SetRewardUI(_rewardsDataSo.GetSprite(rewardData.rewardConfigurationData.rewardType));
                    _inventoryView.AddRewardItem(newRewardItem);

                    _rewardItemViewDictionary[rewardData.rewardConfigurationData.rewardType] = newRewardItem;
                }
            }
        }

        private void OnRevive(int reviveCost)
        {
            if (_rewardItemViewDictionary.TryGetValue(RewardType.Gold, out var goldRewardItem))
            {
                if (goldRewardItem.ZoneRewardData.amount >= reviveCost)
                {
                    goldRewardItem.ZoneRewardData.amount -= reviveCost;
                    goldRewardItem.SetRewardUI(_rewardsDataSo.GetSprite(goldRewardItem.ZoneRewardData.rewardConfigurationData.rewardType));
                }
            }
        }
        
        private void ClearItems()
        {
            _inventoryView.ClearItems();
            _rewardItemViewDictionary.Clear();
        }
    }
}