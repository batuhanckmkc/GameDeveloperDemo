using GameDeveloperDemo.Utils;
using UnityEngine;

namespace GameDeveloperDemo.Model
{
    public class WheelModel
    {
        public int SliceCount { get; } = 8;
        private int _minSpinCount = 2;
        private int _maxSpinCount = 5;

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
    }   
}
