using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameDeveloperDemo.View
{
    public class RewardItem : MonoBehaviour
    {
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardText;
        public ZoneRewardData ZoneRewardData { get; private set; }
        public void SetRewardUI(ZoneRewardData zoneRewardData, Sprite sprite)
        {
            rewardImage.sprite = sprite;
            rewardText.text = $"x{zoneRewardData.amount}";
            ZoneRewardData = zoneRewardData;
        }
    }
}