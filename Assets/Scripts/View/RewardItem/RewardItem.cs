using GameDeveloperDemo.Model.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameDeveloperDemo.View.RewardItem
{
    public abstract class RewardItem : MonoBehaviour
    {
        [SerializeField] protected Image rewardImage;
        [SerializeField] protected TextMeshProUGUI rewardText;
        public ZoneRewardData ZoneRewardData { get; private set; }

        public void InjectData(ZoneRewardData zoneRewardData)
        {
            ZoneRewardData = zoneRewardData;
        }
        public virtual void SetRewardUI(Sprite sprite)
        {
            rewardImage.sprite = sprite;
            rewardText.text = $"x{ZoneRewardData.amount}";
        }
    }
}