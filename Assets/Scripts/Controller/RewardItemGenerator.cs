using System.Collections.Generic;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.View;
using ScriptableObjects;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class RewardItemGenerator : MonoBehaviour
    {
        [SerializeField] private ZoneRewardsDataSO zoneRewardsDataSo;
        [SerializeField] private RewardItem rewardItemPrefab;
        [SerializeField] private float radius = 250f;
        private const float CircleAngle = 360f;
        
        public List<RewardItem> GenerateRewards(Transform parent)
        {
            List<RewardItem> rewardItems = new List<RewardItem>();
            List<ZoneRewardData> rewards = zoneRewardsDataSo.RewardModels;
            float angleStep = CircleAngle / rewards.Count;
            for (int i = 0; i < rewards.Count; i++)
            {
                float angle = 90f - i * angleStep;

                RewardItem rewardItem = Instantiate(rewardItemPrefab, parent);
                rewardItem.transform.localPosition = GetRewardPosition(angle);
                rewardItem.transform.localRotation = GetRewardRotation(angle);
                rewardItems.Add(rewardItem);
            }
            return rewardItems;
        }

        private Vector3 GetRewardPosition(float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians) * radius;
            float y = Mathf.Sin(radians) * radius;

            return new Vector3(x, y, 0);
        }

        private Quaternion GetRewardRotation(float angle)
        {
            return Quaternion.Euler(0, 0, angle - 90);
        }
    }
}