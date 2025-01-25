using GameDeveloperDemo.Model;
using GameDeveloperDemo.View;
using UnityEngine;

namespace GameDeveloperDemo.Controller
{
    public interface IRewardFactory
    {
        public RewardItem CreateReward(ZoneRewardData zoneRewardData);
    }
}