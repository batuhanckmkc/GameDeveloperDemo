using UnityEngine;
using DG.Tweening;

public class WheelOfFortune : MonoBehaviour
{
    public Transform wheel;
    public Transform indicator;
    public int sliceCount = 8;
    public float spinDuration = 3f;
    public Ease spinEase = Ease.OutCubic;

    private float _sliceAngle;
    private int _lastSliceIndex = -1;
    private bool _isSpinning;

    private void Start()
    {
        _sliceAngle = 360f / sliceCount;
    }

    public void SpinWheel()
    {
        if (_isSpinning) return;
        _isSpinning = true;

        int randomSlice = Random.Range(0, sliceCount);
        float targetAngle = randomSlice * _sliceAngle;
        float currentAngle = wheel.rotation.eulerAngles.z % 360;
        float spins = Random.Range(3, 5);
        float finalAngle = spins * 360 + targetAngle - currentAngle;

        wheel.DORotate(new Vector3(0, 0, -finalAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(spinEase)
            .OnUpdate(HandleIndicator)
            .OnComplete(() =>
            {
                _isSpinning = false;
                Debug.Log($"Wheel Stopped! Slice: {randomSlice + 1}");
            });
    }

    private void HandleIndicator()
    {
        float currentAngle = wheel.rotation.eulerAngles.z % 360;
        int currentSliceIndex = Mathf.FloorToInt(currentAngle / _sliceAngle);
        if (currentSliceIndex != _lastSliceIndex)
        {
            _lastSliceIndex = currentSliceIndex;
            float speed = Mathf.Abs(_sliceAngle / (spinDuration * 60f));
            indicator.DORotate(new Vector3(0, 0, -45f), speed)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    indicator.DORotate(Vector3.zero, speed).SetEase(Ease.InCubic);
                });
        }
    }
}
