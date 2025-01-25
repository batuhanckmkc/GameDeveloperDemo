using GameDeveloperDemo.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameDeveloperDemo.View
{
    public abstract class RewardItem : MonoBehaviour
    {
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardText;
        public ZoneRewardData ZoneRewardData { get; private set; }

        public void InjectData(ZoneRewardData zoneRewardData)
        {
            ZoneRewardData = zoneRewardData;
        }
        public void SetRewardUI(Sprite sprite)
        {
            rewardImage.sprite = sprite;
            rewardText.text = $"x{ZoneRewardData.amount}";
        }
    }
}