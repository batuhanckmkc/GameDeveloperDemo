using GameDeveloperDemo.Controller;
using GameDeveloperDemo.Factories;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.ScriptableObjects;
using UnityEngine;

namespace GameDeveloperDemo.Core
{
    public class GameManager : MonoBehaviour
    {
        #region Managers

        [Header("Controllers")]
        [SerializeField] private GameViewManager gameViewManager;
        [SerializeField] private WheelController wheelController;
        [SerializeField] private ZoneController zoneController;
        [SerializeField] private ReviveScreenScreenController reviveScreenScreenController;
        [SerializeField] private RewardController rewardController;
        [SerializeField] private RewardStorageController rewardStorageController;
        [SerializeField] private CurrencyController currencyController;

        #endregion

        #region Scriptables

        [Header("Scriptable Objects")]
        [SerializeField] private ZoneDataSO zoneDataSo;
        [SerializeField] private RewardsDataSO rewardsDataSo;

        #endregion
        
        [Header("Prefabs")]
        [SerializeField] private GameViewManager gameViewManagerPrefab;
        
        private WheelRewardFactory _wheelRewardFactory;
        private FlyingRewardFactory _flyingRewardFactory;
        private StorageRewardFactory _storageRewardFactory;
        private InventoryModel _inventoryModel;
        private void Awake()
        {
            _inventoryModel = new InventoryModel();
            _wheelRewardFactory = new WheelRewardFactory(rewardController.WheelRewardItemPrefab);
            _flyingRewardFactory = new FlyingRewardFactory(rewardController.FlyingRewardItemPrefab);
            _storageRewardFactory = new StorageRewardFactory(rewardController.StorageRewardItem);
            
            gameViewManager = Instantiate(gameViewManagerPrefab);
            InitializeControllers();
        }

        private void InitializeControllers()
        {
            var startingZone = zoneDataSo.GetZoneData(ZoneType.NormalZone);
            var initialZoneModel = new ZoneModel(startingZone);
            
            zoneController.Initialize(gameViewManager.ZoneBarView, zoneDataSo, startingZone, initialZoneModel);
            wheelController.Initialize(_wheelRewardFactory, gameViewManager.WheelView, rewardsDataSo, initialZoneModel);
            reviveScreenScreenController.Initialize(gameViewManager.transform);
            rewardStorageController.Initialize(_flyingRewardFactory, _storageRewardFactory, gameViewManager.RewardStorageView, rewardsDataSo, gameViewManager.WheelView.RewardItemSpawnTransform);
            currencyController.Initialize(gameViewManager.CurrencyView, _storageRewardFactory, rewardsDataSo, _inventoryModel);
        }

        private void OnDestroy()
        {
            _inventoryModel.Dispose();
        }
    }
}
