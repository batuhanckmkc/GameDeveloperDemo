using System.Collections.Generic;
using System.Linq;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.Model.Enum;
using UnityEngine;

namespace GameDeveloperDemo.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ZoneData", menuName = "Create Zone Data")]
    public class ZoneDataSO : ScriptableObject
    {
        [SerializeField] private List<ZoneData> zoneConfigurations;
        public List<ZoneData> ZoneConfigurations => zoneConfigurations;

        private void OnEnable()
        {
            SortZones();
        }

        private void SortZones()
        {
            zoneConfigurations.Sort((x, y) => x.activationAmount.CompareTo(y.activationAmount));
        }
        
        public ZoneData GetZoneData(ZoneType zoneType)
        {
            foreach (var zoneData in zoneConfigurations.Where(setItem => setItem.zoneType == zoneType))
            {
                return zoneData;
            }
            Debug.LogWarning("The targeted zone could not be found.");
            return null;
        }
    }
}