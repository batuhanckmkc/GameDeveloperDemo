using GameDeveloperDemo.Controller;
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
        [SerializeField] private RewardItemGenerator rewardItemGenerator;
        [SerializeField] private ZoneController zoneController;
        [SerializeField] private GameCanvasManager gameCanvasManager;

        #endregion

        #region Scriptables

        [Header("Scriptable Objects")]
        [SerializeField] private ZoneDataSO zoneDataSo;
        [SerializeField] private RewardsDataSO rewardsDataSo;

        #endregion
        
        [Header("Prefabs")]
        [SerializeField] private GameCanvasManager gameCanvasManagerPrefab;
        private void Awake()
        {
            gameCanvasManager = Instantiate(gameCanvasManagerPrefab);
            InitializeControllers();
        }

        private void InitializeControllers()
        {
            var startingZone = zoneDataSo.GetZoneData(ZoneType.NormalZone);
            rewardItemGenerator.Initialize(startingZone);
            wheelController.Initialize(rewardItemGenerator, gameCanvasManager.WheelView, rewardsDataSo, startingZone);
            zoneController.Initialize(gameCanvasManager.ZoneBarView, zoneDataSo, startingZone);
        }
    }
}
