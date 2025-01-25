using System;
using System.Collections.Generic;
using System.Linq;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.ScriptableObjects;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class ZoneController : MonoBehaviour
    {
        [SerializeField] private ZoneBarView zoneBarView;
        [SerializeField] private ZoneDataSO zoneDataSo;
        private List<ZoneData> _sortedZones = new ();
        public static event Action<ZoneData> OnZoneChange;
        private ZoneModel _zoneModel;
        public void Initialize()
        {
            SortZones();
            _zoneModel = new ZoneModel();
            _zoneModel.SetZoneData(zoneDataSo.GetZoneData(ZoneType.NormalZone));
            zoneBarView.Initialize(_zoneModel);
        }

        private void OnEnable()
        {
            WheelController.OnSpinComplete += IncreaseZone;
        }

        private void OnDisable()
        {
            WheelController.OnSpinComplete -= IncreaseZone;
        }

        private void SortZones()
        {
            _sortedZones = zoneDataSo.ZoneConfigurations.OrderBy(z => z.activationAmount).ToList();
        }
        
        private void IncreaseZone()
        {
            _zoneModel.IncreaseZone();;
            CheckZoneState();
        }
        
        private void CheckZoneState()
        {
            for (int i = _sortedZones.Count - 1; i >= 0; i--)
            {
                ZoneData zoneData = _sortedZones[i];
                if (_zoneModel.CurrentZoneIndex % zoneData.activationAmount == 0)
                {
                    _zoneModel.SetZoneData(zoneData);
                    zoneBarView.ShiftNumbers(_zoneModel);
                    OnZoneChange?.Invoke(zoneData);
                    Debug.Log("Update Zone" + zoneData.zoneType);
                    break;
                }
            }
        }
    }
}