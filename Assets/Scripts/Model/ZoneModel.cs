namespace GameDeveloperDemo.Model
{
    public class ZoneModel
    {
        public ZoneData ZoneData { get; private set; }
        public int CurrentZoneIndex { get; private set; } = 1;

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
            ZoneData = null;
            CurrentZoneIndex = 0;
        }
    }
}