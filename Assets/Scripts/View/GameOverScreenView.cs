using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameDeveloperDemo.View
{
    public class GameOverScreenView : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Button giveUpButton;
        [SerializeField] private Button reviveButton;
        
        public void Show() => root.gameObject.SetActive(true);
        public void Hide() => root.gameObject.SetActive(false);

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
            giveUpButton.onClick.AddListener(Hide);
            reviveButton.onClick.AddListener(Hide);
        }
        
        private void UnsubscribeButtonClicks()
        {
            giveUpButton.onClick.RemoveListener(Hide);
            reviveButton.onClick.RemoveListener(Hide);
        }
    }
}