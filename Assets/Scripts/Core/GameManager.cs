using GameDeveloperDemo.Controller;
using UnityEngine;

namespace GameDeveloperDemo.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private WheelController wheelController;
        [SerializeField] private RewardItemGenerator rewardItemGenerator;
        [SerializeField] private ZoneBarController zoneBarController;
        [SerializeField] private ZoneController zoneController;

        private void Awake()
        {
            wheelController.Initialize(rewardItemGenerator);
            zoneController.Initialize();
            zoneBarController.Initialize(zoneController.CurrentZone);
        }
    }
}
