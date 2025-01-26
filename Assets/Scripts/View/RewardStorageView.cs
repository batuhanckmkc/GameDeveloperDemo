using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameDeveloperDemo.View
{
    public class RewardStorageView : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private Transform parentTransform;
        [SerializeField] private Button exitButton;
        public Transform Container => container;
        public Transform ParentTransform => parentTransform;
        private readonly List<StorageRewardItem> _storageRewardItems = new();
        public void Initialize(Action onClickExit)
        {
            UpdateExitButtonState(false);
            exitButton.onClick.AddListener(()=> onClickExit?.Invoke());
        }
        
        public void Deinitialize(Action onClickExit)
        {
            exitButton.onClick.RemoveListener(()=> onClickExit?.Invoke());
        }

        public void AddRewardItem(StorageRewardItem rewardItem)
        {
            rewardItem.transform.SetParent(container);
            rewardItem.transform.localScale = Vector3.one;
            _storageRewardItems.Add(rewardItem);
        }

        public void UpdateExitButtonState(bool activeState)
        {
            exitButton.interactable = activeState;
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