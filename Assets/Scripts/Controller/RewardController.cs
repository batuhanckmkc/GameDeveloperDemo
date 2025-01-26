using GameDeveloperDemo.Factories;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class RewardController : MonoBehaviour
    {
        [SerializeField] private WheelRewardItem wheelRewardItemPrefab;
        [SerializeField] private FlyingWheelRewardItem flyingWheelRewardItemPrefab;
        [SerializeField] private StorageRewardItem storageRewardItem;

        public WheelRewardItem WheelRewardItemPrefab => wheelRewardItemPrefab;
        public FlyingWheelRewardItem FlyingRewardItemPrefab => flyingWheelRewardItemPrefab;
        public StorageRewardItem StorageRewardItem => storageRewardItem;
    }
}