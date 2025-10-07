using System;
using Game.Core.Economy.Balancing;
using Game.Core.Economy.UpgradeSystem;
using Game.Core.Telemetry;

namespace Game.Core.Economy
{
    public class IncomeService
    {
        private readonly UpgradeService _upgradeService;
        private readonly CurrencyService _currencyService;
        private readonly PrestigeService _prestigeService;
        private readonly RemoteConfigService _remoteConfigService;
        private readonly AnalyticsService _analyticsService;
        private readonly BigNumber _baseTap = new(1d);

        public IncomeService(
            UpgradeService upgradeService,
            CurrencyService currencyService,
            PrestigeService prestigeService,
            RemoteConfigService remoteConfigService,
            AnalyticsService analyticsService)
        {
            _upgradeService = upgradeService;
            _currencyService = currencyService;
            _prestigeService = prestigeService;
            _remoteConfigService = remoteConfigService;
            _analyticsService = analyticsService;
        }

        public BigNumber CalculateIncomePerSecond()
        {
            var incomeUpgrade = _upgradeService.GetEffect(UpgradeIds.Income);
            var tapUpgrade = _upgradeService.GetEffect(UpgradeIds.Tap);
            var prestigeMult = _prestigeService.GetPrestigeMultiplier();
            var globalMult = _remoteConfigService.GetDouble(RemoteConfigKeys.GlobalIncomeMultiplier, 1d);

            var baseIncome = _remoteConfigService.GetDouble(RemoteConfigKeys.IncomeBase, 1d);
            var effectA = _upgradeService.GetEntry(UpgradeIds.Income)?.Definition.EffectExponentA ?? 1d;
            var effectB = _upgradeService.GetEntry(UpgradeIds.Tap)?.Definition.EffectExponentA ?? 1d;
            var total = baseIncome * Math.Pow(incomeUpgrade, effectA) * Math.Pow(tapUpgrade, effectB) * prestigeMult * globalMult;
            return new BigNumber(total);
        }

        public BigNumber CalculateTapIncome()
        {
            var tapEffect = _upgradeService.GetEffect(UpgradeIds.Tap);
            var prestigeMult = _prestigeService.GetPrestigeMultiplier();
            return _baseTap * tapEffect * prestigeMult;
        }

        public void GrantTickIncome(double deltaTime)
        {
            var income = CalculateIncomePerSecond().RawValue * deltaTime;
            _currencyService.AddSoft(CurrencyType.Coins, income);
            _prestigeService.TrackLifetimeEarnings(income);
            _analyticsService.LogIncomeTick(income);
        }
    }

    public static class UpgradeIds
    {
        public const string Income = "income";
        public const string Tap = "tap";
        public const string Offline = "offline";
    }
}
