using UnityEngine;

namespace GameDeveloperDemo.View
{
    public class RewardStorageView : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private Transform parentTransform;

        public Transform Container => container;
        public Transform ParentTransform => parentTransform;

        public void AddRewardItem(StorageRewardItem rewardItem)
        {
            rewardItem.transform.SetParent(container);
            rewardItem.transform.localScale = Vector3.one;
        }
    }
}