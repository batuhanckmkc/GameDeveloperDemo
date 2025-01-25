using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameDeveloperDemo.Model;
using GameDeveloperDemo.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameDeveloperDemo.View
{
    public class ZoneBarView : MonoBehaviour
    {
        [SerializeField] private RectTransform numbersContainer;
        [SerializeField] private TextMeshProUGUI numberPrefab;
        [SerializeField] private Image zoneRoundImage;
        [SerializeField] private ZoneDataSO zoneDataSo;
    
        private int _visibleNumbersCount = 10;
        private int _numberSpacing = 55;
        private float _containerMoveAnimationDuration = 0.5f;

        private readonly List<TextMeshProUGUI> _numberTexts = new();
        public void Initialize(ZoneModel zoneModel)
        {
            CreateNumberObjects();
            UpdateUI(zoneModel);
        }
        
        public void ShiftNumbers(ZoneModel zoneModel)
        {
            UpdateUI(zoneModel);
            numbersContainer.DOAnchorPos(new Vector3(-_numberSpacing, 0, 0), _containerMoveAnimationDuration).OnComplete(() =>
            {
                ResetContainerPosition();
                RecycleNumbers();
            });
        }
        
        private void CreateNumberObjects()
        {
            for (int i = 0; i < _visibleNumbersCount; i++)
            {
                TextMeshProUGUI numberObject = Instantiate(numberPrefab, numbersContainer.transform);
                numberObject.text = (i + 1).ToString();
                numberObject.rectTransform.anchoredPosition = new Vector3(i * _numberSpacing, 0, 0);
                _numberTexts.Add(numberObject);
            }
        }
        
        private void UpdateUI(ZoneModel zoneModel)
        {
            for (int i = 0; i < _numberTexts.Count; i++)
            {
                var numberText = _numberTexts[i];
                var number = int.Parse(numberText.text);

                foreach (var zone in zoneDataSo.ZoneConfigurations.Where(zone => number % zone.activationAmount == 0))
                {
                    numberText.color = zone.outsidePointerTextColor;
                }
                if (number == zoneModel.CurrentZoneIndex)
                {
                    numberText.color = zoneModel.ZoneData.insidePointerTextColor;
                    zoneRoundImage.color = zoneModel.ZoneData.zoneBackgroundColor;
                }
            }
        }
        
        public void ResetZoneBar(ZoneModel zoneModel)
        {
            numbersContainer.anchoredPosition = Vector2.zero;

            for (int i = 0; i < _visibleNumbersCount; i++)
            {
                _numberTexts[i].transform.localPosition = new Vector3(i * _numberSpacing, 0, 0);
                _numberTexts[i].text = (i + 1).ToString();
            }
            UpdateUI(zoneModel);
        }
        
        private void RecycleNumbers()
        {
            var firstNumber = _numberTexts[0];
            if (firstNumber.rectTransform.anchoredPosition.x < numbersContainer.anchoredPosition.x - numbersContainer.rect.width / 2)
            {
                float newXPosition = _numberTexts[^1].rectTransform.anchoredPosition.x + _numberSpacing;
                firstNumber.rectTransform.anchoredPosition = new Vector3(newXPosition, 0, 0);
                
                int newValue = int.Parse(_numberTexts[^1].text) + 1;
                firstNumber.text = newValue.ToString();

                _numberTexts.RemoveAt(0);
                _numberTexts.Add(firstNumber);
            }
        }

        private void ResetContainerPosition()
        {
            foreach (var number in _numberTexts)
            {
                number.transform.SetParent(transform);
            }

            numbersContainer.anchoredPosition = Vector3.zero;

            foreach (var number in _numberTexts)
            {
                number.transform.SetParent(numbersContainer.transform);
            }
        }
    }
}