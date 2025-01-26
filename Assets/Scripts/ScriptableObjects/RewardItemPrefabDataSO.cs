using System;
using System.Collections.Generic;
using GameDeveloperDemo.Model.Enum;
using GameDeveloperDemo.View.RewardItem;
using UnityEngine;

namespace GameDeveloperDemo.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RewardItemPrefabsData", menuName = "Create Prefab Data")]
    public class RewardItemPrefabDataSO : ScriptableObject
    {
        [SerializeField] private List<RewardItemPrefabData> rewardItems;
        public List<RewardItemPrefabData> RewardItems => rewardItems;
    }

    [Serializable]
    public class RewardItemPrefabData
    {
        public RewardItem rewardItemPrefab;
        public RewardItemType rewardItemType;
    }
}