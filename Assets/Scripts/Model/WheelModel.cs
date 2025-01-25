using GameDeveloperDemo.Utils;
using UnityEngine;

namespace GameDeveloperDemo.Model
{
    public class WheelModel
    {
        public int SliceCount { get; } = 8;
        private int _minSpinCount = 2;
        private int _maxSpinCount = 5;
        private int _radius = 150;
        public float GetFinalAngle()
        {
            int randomSlice = Random.Range(0, SliceCount);
            float targetAngle = randomSlice * GetSliceAngle();
            float spinCount = Random.Range(_minSpinCount, _maxSpinCount);
            return spinCount * Constants.CircleAngle + targetAngle;
        }
        
        public float GetSliceAngle()
        {
            return Constants.CircleAngle / SliceCount;
        }
        
        public int GetWheelPointerSliceIndex(float finalAngle)
        {
            float normalizedRotation = finalAngle % Constants.CircleAngle;
            if (normalizedRotation < 0)
                normalizedRotation += Constants.CircleAngle;

            float reversedRotation = Constants.CircleAngle - normalizedRotation;
            int sliceIndex = Mathf.FloorToInt(reversedRotation / GetSliceAngle()) % SliceCount;
            return sliceIndex;
        }
        
        public (Vector3 position, Quaternion rotation) GetPositionAndRotation(int totalRewardCount, int rewardIndex)
        {
            float rewardDistance = 90f;
            float angleStep = Constants.CircleAngle / totalRewardCount;
            float angle = rewardDistance - rewardIndex * angleStep;
            return (GetRewardPosition(angle), GetRewardRotation(angle));
        }

        private Vector3 GetRewardPosition(float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians) * _radius;
            float y = Mathf.Sin(radians) * _radius;

            return new Vector3(x, y, 0);
        }

        private Quaternion GetRewardRotation(float angle)
        {
            return Quaternion.Euler(0, 0, angle - 90);
        }
    }   
}
