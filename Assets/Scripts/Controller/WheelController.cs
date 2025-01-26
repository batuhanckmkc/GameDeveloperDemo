using System;
using System.Collections.Generic;
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
        private RewardsDataSO _rewardsDataSo;

        public void Initialize(IRewardFactory rewardFactory, WheelView wheelView, RewardsDataSO rewardsDataSo, ZoneModel initialZoneModel)
        {
            _wheelModel = new WheelModel();
            _rewardsDataSo = rewardsDataSo;
            _wheelView = wheelView;
            _wheelView.Initialize(OnSpinButtonClicked, _rewardsDataSo);
            SetInitialView(initialZoneModel, rewardFactory);
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

        private void SetInitialView(ZoneModel initialZoneModel, IRewardFactory rewardFactory)
        {
            var totalRewardCount = initialZoneModel.ZoneData.zoneRewardsDataSo.RewardModels.Count;
            List<RewardItem> wheelRewardItems = new List<RewardItem>();
            for (int i = 0; i < totalRewardCount; i++)
            {
                var transformData = _wheelModel.GetPositionAndRotation(totalRewardCount, i);
                var rewardData = initialZoneModel.ZoneData.zoneRewardsDataSo.RewardModels[i];
                var rewardItem = rewardFactory.CreateReward(rewardData);
                rewardItem.transform.SetParent(_wheelView.RewardItemParent);
                rewardItem.transform.localPosition = transformData.position;
                rewardItem.transform.localRotation = transformData.rotation;
                wheelRewardItems.Add(rewardItem);
            }
            _wheelView.SetRewardItems(wheelRewardItems);
            _wheelView.SetView(initialZoneModel);
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
        
        private void OnZoneChange(ZoneModel zoneModel)
        {
            _wheelView.SetView(zoneModel);
        }
    }
}