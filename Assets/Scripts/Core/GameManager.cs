using GameDeveloperDemo.Controller;
using GameDeveloperDemo.Controller.Factory;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.Model.Enum;
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
        [SerializeField] private ReviveScreenController reviveScreenController;
        [SerializeField] private RewardStorageController rewardStorageController;
        [SerializeField] private InventoryController inventoryController;

        #endregion

        #region Scriptables

        [Header("Scriptable Objects")]
        [SerializeField] private ZoneDataSO zoneDataSo;
        [SerializeField] private RewardsDataSO rewardsDataSo;
        [SerializeField] private RewardItemPrefabDataSO rewardItemPrefabDataSo;

        #endregion
        
        [Header("Prefabs")]
        [SerializeField] private GameViewManager gameViewManagerPrefab;
        
        private InventoryModel _inventoryModel;
        private RewardFactory _rewardFactory;
        private void Awake()
        {
            _rewardFactory = new RewardFactory(rewardItemPrefabDataSo);
            _inventoryModel = new InventoryModel();
            
            gameViewManager = Instantiate(gameViewManagerPrefab);
            InitializeControllers();
        }

        private void InitializeControllers()
        {
            var startingZone = zoneDataSo.GetZoneData(ZoneType.NormalZone);
            var initialZoneModel = new ZoneModel(startingZone);
            
            zoneController.Initialize(gameViewManager.ZoneBarView, zoneDataSo, startingZone, initialZoneModel);
            wheelController.Initialize(_rewardFactory, gameViewManager.WheelView, rewardsDataSo, initialZoneModel);
            reviveScreenController.Initialize(gameViewManager.ReviveScreenView, gameViewManager.transform);
            rewardStorageController.Initialize(_rewardFactory, gameViewManager.RewardStorageView, rewardsDataSo, gameViewManager.WheelView.RewardItemSpawnTransform);
            inventoryController.Initialize(gameViewManager.InventoryView, _rewardFactory, rewardsDataSo, _inventoryModel);
        }

        private void OnDestroy()
        {
            _inventoryModel.Dispose();
        }
    }
}
