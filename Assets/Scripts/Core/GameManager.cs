using System.Threading.Tasks;
using Game.Core.Economy;
using Game.Core.Economy.UpgradeSystem;
using Game.Core.Monetization.Ads;
using Game.Core.Monetization.IAP;
using Game.Core.SaveSystem;
using Game.Core.Telemetry;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// Composition root for the idle game. Attach to a bootstrap object in Bootstrap scene.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UpgradeDefinition[] _upgradeDefinitions;
        [SerializeField] private AdConfigSO _adConfig;
        [SerializeField] private IAPConfigSO _iapConfig;
        [SerializeField] private TickService _tickService;

        private CurrencyService _currencyService;
        private UpgradeService _upgradeService;
        private IncomeService _incomeService;
        private PrestigeService _prestigeService;
        private RemoteConfigService _remoteConfigService;
        private AnalyticsService _analyticsService;
        private OfflineEarningsService _offlineEarningsService;
        private AdService _adService;
        private IAPService _iapService;
        private SaveService _saveService;
        private TimeService _timeService;

        public static GameManager Instance { get; private set; }

        private async void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            _analyticsService = new AnalyticsService();
            _remoteConfigService = new RemoteConfigService();
            await _remoteConfigService.InitializeAsync();

            _currencyService = new CurrencyService();
            _prestigeService = new PrestigeService(_currencyService, _analyticsService);
            _upgradeService = new UpgradeService(_upgradeDefinitions, _currencyService);
            _timeService = new TimeService();
            _incomeService = new IncomeService(_upgradeService, _currencyService, _prestigeService, _remoteConfigService, _analyticsService);
            _offlineEarningsService = new OfflineEarningsService(_incomeService, _timeService, _remoteConfigService, _analyticsService);
            _adService = new AdService(_adConfig, _analyticsService, _remoteConfigService);
            _iapService = new IAPService(_iapConfig, _analyticsService);
            _saveService = new SaveService(new PlayerPrefsSaveProvider());
            await _saveService.LoadAsync();

            HookTick();
        }

        private void HookTick()
        {
            if (_tickService != null)
            {
                _tickService.Tick += OnTick;
            }
        }

        private void OnTick(double delta)
        {
            _incomeService.GrantTickIncome(delta);
        }

        private async void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                await _saveService.SaveAsync();
            }
        }

        private async void OnApplicationQuit()
        {
            await _saveService.SaveAsync();
        }
    }

    internal class PlayerPrefsSaveProvider : ISaveProvider
    {
        public Task SaveAsync(string data)
        {
            PlayerPrefs.SetString("save", data);
            PlayerPrefs.Save();
            return Task.CompletedTask;
        }

        public Task<string> LoadAsync()
        {
            var data = PlayerPrefs.GetString("save", string.Empty);
            return Task.FromResult(data);
        }
    }
}
