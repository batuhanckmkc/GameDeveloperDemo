using System;
using System.Collections.Generic;
using System.Linq;
using GameDeveloperDemo.Model;
using ScriptableObjects;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class ZoneController : MonoBehaviour
    {
        [SerializeField] private ZoneDataSO zoneDataSo;
        private ZoneData _currentZone;
        private int _currentZoneIndex;
        private List<ZoneData> _sortedZones = new ();
        public static event Action<ZoneData> OnZoneChange;

        private void Awake()
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
            ZoneData selectedZone = null;
            foreach (var zone in _sortedZones)
            {
                if (_currentZoneIndex >= zone.activationAmount)
                {
                    selectedZone = zone;
                }
                else
                {
                    break;
                }
            }
            if (selectedZone != null && _currentZone.zoneType != selectedZone.zoneType)
            {
                _currentZone = selectedZone;
                OnZoneChange?.Invoke(_currentZone);
                Debug.Log("Update Zone");
            }
        }
    }
}