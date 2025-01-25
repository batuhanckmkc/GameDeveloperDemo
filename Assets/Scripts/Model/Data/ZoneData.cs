using GameDeveloperDemo.ScriptableObjects;
using UnityEngine;

namespace GameDeveloperDemo.Model
{
    [System.Serializable]
    public class ZoneData
    {
        public ZoneType zoneType;
        public Color outsidePointerTextColor;
        public Color insidePointerTextColor;
        public Color zoneBackgroundColor;
        public Sprite wheelSprite;
        public Sprite pointerSprite;
        public ZoneRewardsDataSO zoneRewardsDataSo;
        public int activationAmount;
    }
}