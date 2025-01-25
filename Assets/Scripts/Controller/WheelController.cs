using System;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.ScriptableObjects;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class WheelController : MonoBehaviour
    {
        public static event Action<ZoneRewardData> OnSpinComplete;
        private WheelModel _wheelModel;
        private WheelView _wheelView;
        private RewardItemGenerator _rewardItemGenerator;
        private RewardsDataSO _rewardsDataSo;

        public void Initialize(RewardItemGenerator rewardItemGenerator, WheelView wheelView, RewardsDataSO rewardsDataSo, ZoneData startingZone)
        {
            _wheelModel = new WheelModel();
            _rewardItemGenerator = rewardItemGenerator;
            _rewardsDataSo = rewardsDataSo;
            _wheelView = wheelView;
            _wheelView.Initialize(OnSpinButtonClicked, _rewardsDataSo);
            _wheelView.SetRewardItems(_rewardItemGenerator.GenerateRewards(_wheelView.RewardItemSpawnPosition), startingZone);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
            _wheelView.Deinitialize(OnSpinButtonClicked);
        }

        private void SubscribeEvents()
        {
            ZoneController.OnZoneChange += OnZoneChange;
            ReviveScreenView.OnGiveUp += OnGiveUp;
        }
        
        private void UnsubscribeEvents()
        {
            ZoneController.OnZoneChange -= OnZoneChange;
            ReviveScreenView.OnGiveUp -= OnGiveUp;
        }
        
        private void OnSpinButtonClicked()
        {
            Debug.Log("Spin button clicked!");
            var sliceAngle = _wheelModel.GetSliceAngle();
            var finalAngle = _wheelModel.GetFinalAngle();
            _wheelView.CloseSpinButton();
            _wheelView.RotateWheel(sliceAngle, finalAngle, 3f,() =>
            {
                _wheelView.OpenSpinButton();
                var stoppedItem = GetStoppedZoneItem(finalAngle);
                OnSpinComplete?.Invoke(stoppedItem);
                Debug.Log($"Wheel Stopped! ZoneRewardType: {stoppedItem.rewardConfigurationData.rewardType}, ZoneRewardAmount: {stoppedItem.amount}");
            });
        }
        
        private ZoneRewardData GetStoppedZoneItem(float finalAngle)
        {
            int sliceIndex = _wheelModel.GetWheelPointerSliceIndex(finalAngle);
            return _wheelView.GetRewardAtIndex(sliceIndex);
        }

        private void OnGiveUp()
        {
            _wheelView.ResetView();
        }
        
        private void OnZoneChange(ZoneData zoneData)
        {
            _wheelView.SetView(zoneData);
        }
    }
}