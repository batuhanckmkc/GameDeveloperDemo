using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameDeveloperDemo.View
{
    public class BaseButton : MonoBehaviour
    {
        public Button button;
        private void OnValidate()
        {
            button = GetComponent<Button>();
        }

        public void RegisterListener(Action onButtonClicked)
        {
            button.onClick.AddListener(()=> onButtonClicked?.Invoke());
        }

        public void UnregisterListener(Action onButtonClicked)
        {
            button.onClick.RemoveListener(()=> onButtonClicked?.Invoke());
        }
    }
}