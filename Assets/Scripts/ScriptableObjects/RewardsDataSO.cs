using System.Collections.Generic;
using System.Linq;
using GameDeveloperDemo.Model;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RewardsData", menuName = "Create Rewards Data")]
    public class RewardsDataSO : ScriptableObject
    {
        [SerializeField] private List<RewardData> rewardModels;
        public Sprite GetSprite(RewardType rewardType)
        {
            foreach (var setItem in rewardModels.Where(setItem => setItem.RewardConfigurationData.RewardType == rewardType))
            {
                return setItem.sprite;
            }
            Debug.LogWarning("The targeted sprite could not be found.");
            return null;
        }
    }
    
    [System.Serializable]
    public class RewardData
    {
        public RewardConfigurationData RewardConfigurationData;
        public Sprite sprite;
    }

    [System.Serializable]
    public struct RewardConfigurationData
    {
        public RewardType RewardType;
        public TierType TierType;
    }
}

