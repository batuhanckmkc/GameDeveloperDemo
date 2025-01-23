using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ZoneRewardsData", menuName = "Create Zone Rewards Data")]
    public class ZoneRewardsDataSO : ScriptableObject
    {
        [SerializeField] private List<ZoneRewardData> rewardModels;
        public List<ZoneRewardData> RewardModels => rewardModels;
    }
    
    [System.Serializable]
    public class ZoneRewardData
    {
        public RewardConfigurationData RewardConfigurationData;
        public int amount;
    }
}