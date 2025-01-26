using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameDeveloperDemo.View
{
    public class ReviveScreenView : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Button giveUpButton;
        [SerializeField] private Button reviveButton;
        [SerializeField] private TextMeshProUGUI reviveCostText;
        [SerializeField] private TextMeshProUGUI notEnoughGoldText;

        public static event Action<int> OnRevive;
        public static event Action OnGiveUp;
        private int _reviveCost;
        public void Initialize(int reviveCost)
        {
            _reviveCost = reviveCost;
        }
        
        public void Show()
        {
            reviveCostText.SetText(_reviveCost.ToString());
            root.gameObject.SetActive(true);
        }

        private void Hide()
        {
            reviveButton.interactable = true;
            notEnoughGoldText.gameObject.SetActive(false);
            root.gameObject.SetActive(false);
        }

        public void UpdateViewAccordingEnoughGold(bool isEnoughGold)
        {
            reviveButton.interactable = !isEnoughGold;
            notEnoughGoldText.gameObject.SetActive(isEnoughGold);
        }

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
            OnRevive?.Invoke(_reviveCost);
        }
    }
}