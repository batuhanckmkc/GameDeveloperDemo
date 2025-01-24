using System.Collections.Generic;
using GameDeveloperDemo.Model;
using UnityEngine;

namespace GameDeveloperDemo.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ZoneRewardsData", menuName = "Create Zone Rewards Data")]
    public class ZoneRewardsDataSO : ScriptableObject
    {
        [SerializeField] private List<ZoneRewardData> rewardModels;
        public List<ZoneRewardData> RewardModels => rewardModels;
    }
}