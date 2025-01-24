using System;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.ScriptableObjects;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class WheelController : MonoBehaviour
    {
        [SerializeField] private WheelView wheelView;
        [SerializeField] private RewardsDataSO rewardsDataSo;
        [SerializeField] private ZoneDataSO zoneDataSo;
        public static event Action OnSpinComplete;
        private WheelModel _wheelModel;
        private RewardItemGenerator _rewardItemGenerator;

        public void Initialize(RewardItemGenerator rewardItemGenerator)
        {
            _wheelModel = new WheelModel();
            _rewardItemGenerator = rewardItemGenerator;
            wheelView.SetRewardItems(_rewardItemGenerator.GenerateRewards(wheelView.transform), zoneDataSo.GetZoneData(ZoneType.NormalZone));
        }

        private void OnEnable()
        {
            ZoneController.OnZoneChange += OnZoneChange;
            wheelView.Initialize(OnSpinButtonClicked, rewardsDataSo);
        }

        private void OnDisable()
        {
            ZoneController.OnZoneChange -= OnZoneChange;
            wheelView.Deinitialize(OnSpinButtonClicked);
        }

        private void OnSpinButtonClicked()
        {
            Debug.Log("Spin button clicked!");
            var sliceAngle = _wheelModel.GetSliceAngle();
            var finalAngle = _wheelModel.GetFinalAngle();
            wheelView.CloseSpinButton();
            wheelView.RotateWheel(sliceAngle, finalAngle, 3f,() =>
            {
                wheelView.OpenSpinButton();
                OnSpinComplete?.Invoke();
                
                var stoppedReward = GetStoppedReward(finalAngle);
                Debug.Log($"Wheel Stopped! ZoneRewardType: {stoppedReward.rewardConfigurationData.rewardType}, ZoneRewardAmount: {stoppedReward.amount}");
            });
        }
        
        private ZoneRewardData GetStoppedReward(float finalAngle)
        {
            int sliceIndex = _wheelModel.GetWheelPointerSliceIndex(finalAngle);
            return wheelView.GetRewardAtIndex(sliceIndex);
        }
        
        private void OnZoneChange(ZoneData zoneData)
        {
            wheelView.SetView(zoneData);
        }
    }
}