using System;
using System.Collections.Generic;
using Game.Core.Telemetry;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Game.Core.Monetization.IAP
{
    /// <summary>
    /// Handles Unity IAP initialization and purchase processing.
    /// </summary>
    public class IAPService : IStoreListener
    {
        private IStoreController _controller;
        private IExtensionProvider _extensions;
        private readonly IAPConfigSO _config;
        private readonly AnalyticsService _analyticsService;
        private readonly Dictionary<string, Action> _productRewards = new();

        public bool IsInitialized => _controller != null;

        public IAPService(IAPConfigSO config, AnalyticsService analyticsService)
        {
            _config = config;
            _analyticsService = analyticsService;
        }

        public void RegisterReward(string productId, Action reward)
        {
            _productRewards[productId] = reward;
        }

        public void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            if (_config?.Products != null)
            {
                foreach (var product in _config.Products)
                {
                    builder.AddProduct(product.ProductId, ProductType.NonConsumable);
                }
            }

            UnityPurchasing.Initialize(this, builder);
        }

        public void Purchase(string productId)
        {
            if (!IsInitialized)
            {
                Initialize();
                return;
            }

            _controller?.InitiatePurchase(productId);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            UnityEngine.Debug.LogError($"IAP init failed {error}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            if (_productRewards.TryGetValue(e.purchasedProduct.definition.id, out var reward))
            {
                reward?.Invoke();
            }

            _analyticsService.LogIAPPurchase(e.purchasedProduct.definition.id, e.purchasedProduct.metadata.localizedPriceString, e.purchasedProduct.metadata.isoCurrencyCode);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            UnityEngine.Debug.LogWarning($"Purchase failed {failureReason}");
        }
    }
}
