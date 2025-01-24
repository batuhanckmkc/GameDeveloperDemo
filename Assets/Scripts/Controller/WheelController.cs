using System;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.View;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameDeveloperDemo.Controller
{
    public class WheelController : MonoBehaviour
    {
        [SerializeField] private WheelView wheelView;
        [SerializeField] private RewardItemGenerator rewardItemGenerator;
        [SerializeField] private RewardsDataSO rewardsDataSo;
        [SerializeField] private ZoneDataSO zoneDataSo;
        public static event Action OnSpinComplete;
        private WheelModel _wheelModel;

        private void Start()
        {
            _wheelModel = new WheelModel();
            wheelView.SetRewardItems(rewardItemGenerator.GenerateRewards(wheelView.transform), zoneDataSo.GetZoneData(ZoneType.NormalZone));
        }

        private void OnEnable()
        {
            ZoneController.OnZoneChange += OnZoneChange;
            wheelView.Initialize(OnSpinButtonClicked, rewardsDataSo);
        }

        private void OnDisable()
        {
            ZoneController.OnZoneChange -= OnZoneChange;
            wheelView.Deinitialize(OnSpinButtonClicked);
        }

        private void OnSpinButtonClicked()
        {
            Debug.Log("Spin button clicked!");

            var sliceCount = _wheelModel.SliceCount;
            int randomSlice = Random.Range(0, sliceCount);
            float targetAngle = randomSlice * (360f / sliceCount);
            float spins = Random.Range(3, 5);
            float finalAngle = spins * 360 + targetAngle;
            float sliceAngle = 360f / sliceCount;

            wheelView.CloseSpinButton();
            wheelView.RotateWheel(sliceAngle, finalAngle, 3f, () =>
            {
                wheelView.OpenSpinButton();
                OnSpinComplete?.Invoke();
            });
        }

        private void OnZoneChange(ZoneData zoneData)
        {
            wheelView.SetView(zoneData);
        }
    }
}