using UnityEngine;

namespace Game.Core.Monetization.Ads
{
    [CreateAssetMenu(menuName = "Game/Ad Config", fileName = "AdConfig")]
    public class AdConfigSO : ScriptableObject
    {
        public string AndroidRewardedId = "ca-app-pub-3940256099942544/5224354917";
        public string IOSRewardedId = "";
    }
}
