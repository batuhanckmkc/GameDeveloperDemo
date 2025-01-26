using DG.Tweening;
using UnityEngine;

namespace GameDeveloperDemo.View
{
    public class StorageRewardItem : RewardItem
    {
        public void PlaySpawnAnimation(float duration = 0.5f, float overshoot = 1.2f)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack, overshoot);
        }
    }
}