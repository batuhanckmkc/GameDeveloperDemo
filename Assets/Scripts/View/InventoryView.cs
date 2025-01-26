using System.Collections.Generic;
using UnityEngine;

namespace GameDeveloperDemo.View
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private Transform container;
        private readonly List<StorageRewardItem> _storageRewardItems = new();
        public void AddRewardItem(StorageRewardItem rewardItem)
        {
            rewardItem.transform.SetParent(container);
            rewardItem.transform.localScale = Vector3.one;
            _storageRewardItems.Add(rewardItem);
        }
        
        public void ClearItems()
        {
            foreach (var storageRewardItem in _storageRewardItems)
            {
                Destroy(storageRewardItem.gameObject);
            }
            _storageRewardItems.Clear();
        }
    }
}