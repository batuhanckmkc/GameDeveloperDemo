using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class GameCanvasManager : MonoBehaviour
    {
        [SerializeField] private ZoneBarView zoneBarView;
        [SerializeField] private WheelView wheelView;

        public ZoneBarView ZoneBarView => zoneBarView;
        public WheelView WheelView => wheelView;
    }
}