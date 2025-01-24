using System;
using GameDeveloperDemo.Controller;
using UnityEngine;

namespace GameDeveloperDemo.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private WheelController wheelController;
        [SerializeField] private RewardItemGenerator rewardItemGenerator;

        private void Awake()
        {
            wheelController.Initialize(rewardItemGenerator);
        }
    }
}
