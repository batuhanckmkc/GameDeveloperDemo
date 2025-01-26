namespace GameDeveloperDemo.Model
{
    public class ZoneModel
    {
        public ZoneData ZoneData { get; private set; }
        public int CurrentZoneIndex { get; private set; } = ZoneStartIndex;
        private const int ZoneStartIndex = 1;
        public ZoneModel(ZoneData zoneData)
        {
            ZoneData = zoneData;
        }

        public void SetZoneData(ZoneData zoneData)
        {
            ZoneData = zoneData;
        }
        
        public void IncreaseZone()
        {
            CurrentZoneIndex++;
        }
        
        public void ResetZone()
        {
            CurrentZoneIndex = ZoneStartIndex;
        }
    }
}