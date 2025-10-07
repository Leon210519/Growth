using UnityEngine;

namespace Game.Core.Monetization.IAP
{
    [CreateAssetMenu(menuName = "Game/IAP Config", fileName = "IAPConfig")]
    public class IAPConfigSO : ScriptableObject
    {
        [System.Serializable]
        public class ProductInfo
        {
            public string ProductId;
            public string Title;
            public string Description;
            public string PriceString;
        }

        public ProductInfo[] Products;
    }
}
