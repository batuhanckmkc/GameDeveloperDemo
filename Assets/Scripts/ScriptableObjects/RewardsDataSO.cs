using System.Collections.Generic;
using System.Linq;
using GameDeveloperDemo.Model.Data;
using GameDeveloperDemo.Model.Enum;
using UnityEngine;

namespace GameDeveloperDemo.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RewardsData", menuName = "Create Rewards Data")]
    public class RewardsDataSO : ScriptableObject
    {
        [SerializeField] private List<RewardData> rewardModels;
        public Sprite GetSprite(RewardType rewardType)
        {
            foreach (var rewardData in rewardModels.Where(setItem => setItem.rewardConfigurationData.rewardType == rewardType))
            {
                return rewardData.sprite;
            }
            Debug.LogWarning("The targeted sprite could not be found.");
            return null;
        }
    }
}

