using System;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.ScriptableObjects;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class ZoneController : MonoBehaviour
    {
        public static event Action<ZoneData> OnZoneChange;
        private ZoneBarView _zoneBarView;
        private ZoneModel _zoneModel;
        private ZoneDataSO _zoneDataSo;
        private ZoneData _startingZone;
        public void Initialize(ZoneBarView zoneBarView, ZoneDataSO zoneDataSo, ZoneData startingZone)
        {
            _startingZone = startingZone;
            _zoneBarView = zoneBarView;
            _zoneDataSo = zoneDataSo;
            _zoneModel = new ZoneModel();
            _zoneModel.SetZoneData(startingZone);
            _zoneBarView.Initialize(_zoneModel, zoneDataSo);
        }

        private void OnEnable()
        {
            WheelController.OnSpinComplete += IncreaseZone;
            ReviveScreenView.OnGiveUp += OnGiveUp;
        }

        private void OnDisable()
        {
            WheelController.OnSpinComplete -= IncreaseZone;
            ReviveScreenView.OnGiveUp -= OnGiveUp;
        }
        
        private void IncreaseZone(ZoneRewardData zoneRewardData)
        {
            if (zoneRewardData.rewardConfigurationData.rewardType == RewardType.Bomb)
            {
                return;
            }
            _zoneModel.IncreaseZone();;
            CheckZoneState();
        }
        
        private void CheckZoneState()
        {
            for (int i = _zoneDataSo.ZoneConfigurations.Count - 1; i >= 0; i--)
            {
                ZoneData zoneData = _zoneDataSo.ZoneConfigurations[i];
                if (_zoneModel.CurrentZoneIndex % zoneData.activationAmount == 0)
                {
                    _zoneModel.SetZoneData(zoneData);
                    _zoneBarView.ShiftNumbers(_zoneModel);
                    OnZoneChange?.Invoke(zoneData);
                    Debug.Log("Update Zone" + zoneData.zoneType);
                    break;
                }
            }
        }

        private void OnGiveUp()
        {
            _zoneModel.ResetZone();
            _zoneModel.SetZoneData(_startingZone);
            _zoneBarView.ResetZoneBar(_zoneModel);
            OnZoneChange?.Invoke(_startingZone);
        }
    }
}