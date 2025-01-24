using System;
using System.Collections.Generic;
using System.Linq;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.ScriptableObjects;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class ZoneController : MonoBehaviour
    {
        [SerializeField] private ZoneDataSO zoneDataSo;
        private ZoneData _currentZone;
        public ZoneData CurrentZone => _currentZone;
        private int _currentZoneIndex = 1;
        private List<ZoneData> _sortedZones = new ();
        public static event Action<ZoneData> OnZoneChange;

        public void Initialize()
        {
            SortZones();
            _currentZone = zoneDataSo.GetZoneData(ZoneType.NormalZone);
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
            _currentZoneIndex++;
            CheckZoneState();
        }
        
        private void CheckZoneState()
        {
            for (int i = _sortedZones.Count - 1; i >= 0; i--)
            {
                ZoneData zoneData = _sortedZones[i];
                if (_currentZoneIndex % zoneData.activationAmount == 0)
                {
                    _currentZone = zoneData;
                    OnZoneChange?.Invoke(_currentZone);
                    Debug.Log("Update Zone" + _currentZone.zoneType);
                    break;
                }
            }
        }
    }
}