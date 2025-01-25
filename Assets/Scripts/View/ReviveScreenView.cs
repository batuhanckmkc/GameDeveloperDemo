using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameDeveloperDemo.View
{
    public class ReviveScreenView : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Button giveUpButton;
        [SerializeField] private Button reviveButton;
        public static event Action OnRevive;
        public static event Action OnGiveUp;
        public void Show() => root.gameObject.SetActive(true);
        private void Hide() => root.gameObject.SetActive(false);

        private void OnEnable()
        {
            SubscribeButtonClicks();
        }

        private void OnDisable()
        {
            UnsubscribeButtonClicks();
        }

        private void SubscribeButtonClicks()
        {
            giveUpButton.onClick.AddListener(GiveUp);
            reviveButton.onClick.AddListener(Revive);
        }
        
        private void UnsubscribeButtonClicks()
        {
            giveUpButton.onClick.RemoveListener(GiveUp);
            reviveButton.onClick.RemoveListener(Revive);
        }

        private void GiveUp()
        {
            Hide();
            OnGiveUp?.Invoke();
        }
        
        private void Revive()
        {
            Hide();
            OnRevive?.Invoke();
        }
    }
}