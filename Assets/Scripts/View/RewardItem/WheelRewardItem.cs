
using GameDeveloperDemo.Model;
using UnityEngine;

namespace GameDeveloperDemo.View
{
    public class WheelRewardItem : RewardItem
    {
        public override void SetRewardUI(Sprite sprite)
        {
            rewardImage.sprite = sprite;
            rewardText.text = ZoneRewardData.rewardConfigurationData.rewardType == RewardType.Bomb ? "Bomb" : $"x{ZoneRewardData.amount}";
        }
    }
}