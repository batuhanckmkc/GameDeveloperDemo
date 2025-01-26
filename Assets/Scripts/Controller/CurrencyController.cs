using System.Collections.Generic;
using GameDeveloperDemo.Factories;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.ScriptableObjects;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class CurrencyController : MonoBehaviour
    {
        private RewardsDataSO _rewardsDataSo;
        private StorageRewardFactory _storageRewardFactory;
        private CurrencyView _currencyView;
        private InventoryModel _inventoryModel;

        private readonly Dictionary<RewardType, StorageRewardItem> _rewardItemViewDictionary = new();
        public void Initialize(CurrencyView currencyView, StorageRewardFactory storageRewardFactory, RewardsDataSO rewardsDataSo, InventoryModel inventoryModel)
        {
            _currencyView = currencyView;
            _storageRewardFactory = storageRewardFactory;
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
        }

        private void UnsubscribeEvents()
        {
            RewardStorageController.OnExit -= UpdateInventory;
            ReviveScreenView.OnGiveUp -= ClearItems;
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
                    var newRewardItem = _storageRewardFactory.CreateReward(rewardData) as StorageRewardItem;
                    newRewardItem.InjectData(rewardData);
                    newRewardItem.SetRewardUI(_rewardsDataSo.GetSprite(rewardData.rewardConfigurationData.rewardType));
                    _currencyView.AddRewardItem(newRewardItem);

                    _rewardItemViewDictionary[rewardData.rewardConfigurationData.rewardType] = newRewardItem;
                }
            }
        }

        private void ClearItems()
        {
            _currencyView.ClearItems();
            _rewardItemViewDictionary.Clear();
        }
    }
}