using UnityEngine;

namespace GameDeveloperDemo.View
{
    public class RewardStorageView : MonoBehaviour
    {
        [SerializeField] private Transform container;
        public Transform Container => container;
    }
}