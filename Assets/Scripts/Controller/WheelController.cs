using GameDeveloperDemo.Model;
using GameDeveloperDemo.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameDeveloperDemo.Controller
{
    public class WheelController : MonoBehaviour
    {
        [SerializeField] private WheelView wheelView;
        [SerializeField] private RewardItemGenerator rewardItemGenerator;
        
        private WheelModel _wheelModel;
        
        private void Awake()
        {
            _wheelModel = new WheelModel();
            wheelView.Initialize(OnSpinButtonClicked);
            rewardItemGenerator.GenerateRewards();
        }

        private void OnDestroy()
        {
            wheelView.DeInitialize(OnSpinButtonClicked);
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

            wheelView.RotateWheel(sliceAngle, finalAngle, 3f, null);
        }
    }
}