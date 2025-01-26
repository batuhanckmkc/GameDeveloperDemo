using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class GameViewManager : MonoBehaviour
    {
        [SerializeField] private ZoneBarView zoneBarView;
        [SerializeField] private WheelView wheelView;
        [SerializeField] private RewardStorageView rewardStorageView;
        [SerializeField] private InventoryView inventoryView;
        [SerializeField] private ReviveScreenView reviveScreenViewPrefab;
        public ZoneBarView ZoneBarView => zoneBarView;
        public WheelView WheelView => wheelView;
        public RewardStorageView RewardStorageView => rewardStorageView;
        public InventoryView InventoryView => inventoryView;
        public ReviveScreenView ReviveScreenView => reviveScreenViewPrefab;
    }
}