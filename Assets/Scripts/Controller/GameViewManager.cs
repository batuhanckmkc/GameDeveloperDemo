using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class GameViewManager : MonoBehaviour
    {
        [SerializeField] private ZoneBarView zoneBarView;
        [SerializeField] private WheelView wheelView;
        [SerializeField] private RewardStorageView rewardStorageView;
        [SerializeField] private CurrencyView currencyView;
        public ZoneBarView ZoneBarView => zoneBarView;
        public WheelView WheelView => wheelView;
        public RewardStorageView RewardStorageView => rewardStorageView;
        public CurrencyView CurrencyView => currencyView;
    }
}