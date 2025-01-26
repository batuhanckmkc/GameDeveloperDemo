
using DG.Tweening;
using UnityEngine;

namespace GameDeveloperDemo.View
{
    public class FlyingWheelRewardItem : RewardItem
    {
        public void SetAmount(bool isFirstFlyingItem)
        {
            rewardText.text = isFirstFlyingItem ? $"x{ZoneRewardData.amount}" : "";
        }

        public void SetSprite(Sprite sprite)
        {
            rewardImage.sprite = sprite;
        }
        
        public Tween ScaleUp()
        {
            var targetScale = new Vector3(0.75f, 0.75f, 0.75f);
            var animationDuration = 0.35f;
            transform.localScale = Vector3.zero;
            var tween = transform.DOScale(targetScale, animationDuration).SetEase(Ease.OutBack);
            return tween;
        }
        
        public Tween Fly(Vector3 targetPosition)
        {
            var animationDuration = 0.7f; 
            var tween = transform.DOMove(targetPosition, animationDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    Debug.Log("Reward animation completed.");
                    Destroy(gameObject);
                });

            return tween;
        }
    }
}