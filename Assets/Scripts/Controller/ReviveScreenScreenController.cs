using GameDeveloperDemo.Model;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public class ReviveScreenScreenController : MonoBehaviour
    {
        [SerializeField] private ReviveScreenView reviveScreenViewPrefab;
        private ReviveScreenView _reviveScreenView;
        private Transform _reviveScreenSpawnTransform;
        public void Initialize(Transform reviveScreenSpawnTransform)
        {
            _reviveScreenSpawnTransform = reviveScreenSpawnTransform;
        }
        
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            WheelController.OnSpinComplete += CheckGameOver;
        }
        
        private void UnsubscribeEvents()
        {
            WheelController.OnSpinComplete -= CheckGameOver;
        }

        private void CheckGameOver(ZoneRewardData zoneRewardData)
        {
            if (!IsItemBomb(zoneRewardData))
                return;

            CreateGameOverView();
        }

        private bool IsItemBomb(ZoneRewardData zoneRewardData)
        {
            return zoneRewardData.rewardConfigurationData.rewardType == RewardType.Bomb;
        }
        
        private void CreateGameOverView()
        {
            _reviveScreenView = Instantiate(reviveScreenViewPrefab, _reviveScreenSpawnTransform);
            _reviveScreenView.Show();
        }
    }
}