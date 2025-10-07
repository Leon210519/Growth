using Game.Core;
using Game.Core.Economy;
using Game.Core.Economy.UpgradeSystem;
using Game.Core.Monetization.Ads;
using Game.UI.Widgets;
using UnityEngine;

namespace Game.UI.Screens
{
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private CurrencyView _coinsView;
        [SerializeField] private CurrencyView _gemsView;
        [SerializeField] private PrestigePanelView _prestigePanel;
        [SerializeField] private UpgradeCardView[] _upgradeCards;

        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            Bind();
        }

        private void Bind()
        {
            var currencyService = typeof(GameManager).GetField("_currencyService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_gameManager) as CurrencyService;
            var upgradeService = typeof(GameManager).GetField("_upgradeService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_gameManager) as UpgradeService;
            var prestigeService = typeof(GameManager).GetField("_prestigeService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_gameManager) as PrestigeService;

            if (currencyService == null || upgradeService == null || prestigeService == null)
            {
                Debug.LogError("Services not ready");
                return;
            }

            _prestigePanel.Initialize(prestigeService);
            foreach (var entry in upgradeService.GetAll())
            {
                foreach (var card in _upgradeCards)
                {
                    if (card.name.ToLowerInvariant().Contains(entry.Definition.Id))
                    {
                        card.Initialize(entry, upgradeService);
                    }
                }
            }

            currencyService.SoftBalanceChanged += OnSoftBalanceChanged;
            currencyService.IntBalanceChanged += OnIntBalanceChanged;
            OnSoftBalanceChanged(CurrencyType.Coins, currencyService.GetCoins());
            OnIntBalanceChanged(CurrencyType.Gems, currencyService.GetGems());
        }

        private void OnDestroy()
        {
            var currencyService = typeof(GameManager).GetField("_currencyService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_gameManager) as CurrencyService;
            if (currencyService != null)
            {
                currencyService.SoftBalanceChanged -= OnSoftBalanceChanged;
                currencyService.IntBalanceChanged -= OnIntBalanceChanged;
            }
        }

        private void OnSoftBalanceChanged(CurrencyType type, BigNumber value)
        {
            if (type == CurrencyType.Coins)
            {
                _coinsView.SetValue(value);
            }
        }

        private void OnIntBalanceChanged(CurrencyType type, int value)
        {
            if (type == CurrencyType.Gems)
            {
                _gemsView.SetValue(value);
            }
        }
    }
}
