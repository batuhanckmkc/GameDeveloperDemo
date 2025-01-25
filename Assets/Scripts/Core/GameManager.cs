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
        [SerializeField] private WheelController wheelController;
        [SerializeField] private ZoneController zoneController;
        [SerializeField] private ReviveScreenScreenController reviveScreenScreenController;
        [SerializeField] private GameCanvasManager gameCanvasManager;
        [SerializeField] private RewardController rewardController;
        [SerializeField] private RewardStorageController rewardStorageController;
        
        #endregion

        #region Scriptables

        [Header("Scriptable Objects")]
        [SerializeField] private ZoneDataSO zoneDataSo;
        [SerializeField] private RewardsDataSO rewardsDataSo;

        #endregion
        
        [Header("Prefabs")]
        [SerializeField] private GameCanvasManager gameCanvasManagerPrefab;
        
        private InventoryModel _inventoryModel;
        private WheelRewardFactory _wheelRewardFactory;
        private FlyingRewardFactory _flyingRewardFactory;
        private void Awake()
        {
            _inventoryModel = new InventoryModel();
            _wheelRewardFactory = new WheelRewardFactory(rewardController.WheelRewardItemPrefab);
            _flyingRewardFactory = new FlyingRewardFactory(rewardController.FlyingRewardItemPrefab);
            gameCanvasManager = Instantiate(gameCanvasManagerPrefab);
            InitializeControllers();
        }

        private void InitializeControllers()
        {
            var startingZone = zoneDataSo.GetZoneData(ZoneType.NormalZone);
            wheelController.Initialize(_wheelRewardFactory, gameCanvasManager.WheelView, rewardsDataSo, startingZone);
            zoneController.Initialize(gameCanvasManager.ZoneBarView, zoneDataSo, startingZone);
            reviveScreenScreenController.Initialize(gameCanvasManager.transform);
            rewardStorageController.Initialize(_flyingRewardFactory, gameCanvasManager.RewardStorageView, rewardsDataSo, gameCanvasManager.transform, gameCanvasManager.WheelView.RewardItemSpawnTransform);
        }
    }
}
