using System.Collections.Generic;
using GameDeveloperDemo.View;
using ScriptableObjects;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class RewardItemGenerator : MonoBehaviour
    {
        [SerializeField] private RewardsDataSO rewardsDataSo;
        [SerializeField] private ZoneRewardsDataSO zoneRewardsDataSo;
        [SerializeField] private Transform wheel;
        [SerializeField] private RewardItem rewardItemPrefab;
        [SerializeField] private float radius = 250f;
        private const float CircleAngle = 360f;
        public void GenerateRewards()
        {
            List<ZoneRewardData> rewards = zoneRewardsDataSo.RewardModels;
            float angleStep = CircleAngle / rewards.Count;

            for (int i = 0; i < rewards.Count; i++)
            {
                RewardItem rewardItem = Instantiate(rewardItemPrefab, wheel);
                float angle = 90f - i * angleStep;

                rewardItem.transform.localPosition = GetRewardPosition(angle);
                rewardItem.transform.localRotation = Quaternion.Euler(0, 0, angle - 90);
                rewardItem.SetRewardUI(rewards[i], rewardsDataSo.GetSprite(rewards[i].RewardConfigurationData.RewardType)
                );
            }
        }

        private Vector3 GetRewardPosition(float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians) * radius;
            float y = Mathf.Sin(radians) * radius;

            return new Vector3(x, y, 0);
        }
    }
}