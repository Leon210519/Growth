using System;
using System.Collections.Generic;
using Game.Core.Economy.Balancing;

namespace Game.Core.Economy.UpgradeSystem
{
    public class UpgradeService
    {
        private readonly Dictionary<string, UpgradeEntry> _entries = new();
        private readonly CurrencyService _currencyService;

        public UpgradeService(IEnumerable<UpgradeDefinition> definitions, CurrencyService currencyService)
        {
            _currencyService = currencyService;
            foreach (var def in definitions)
            {
                _entries[def.Id] = new UpgradeEntry(def, 0);
            }
        }

        public event Action<UpgradeEntry> UpgradePurchased;

        public IEnumerable<UpgradeEntry> GetAll() => _entries.Values;

        public UpgradeEntry GetEntry(string id) => _entries.TryGetValue(id, out var entry) ? entry : null;

        public BigNumber GetCost(string id)
        {
            var entry = GetEntry(id);
            if (entry == null)
            {
                return new BigNumber(0);
            }

            return entry.Definition.CostCurve.Evaluate(entry.Level);
        }

        public double GetEffect(string id)
        {
            var entry = GetEntry(id);
            if (entry == null)
            {
                return 1d;
            }

            var def = entry.Definition;
            var level = entry.Level;
            var baseEffect = def.BaseEffect;
            return baseEffect * Math.Pow(1 + level, def.EffectExponentA);
        }

        public double GetMilestoneMultiplier(string id)
        {
            var entry = GetEntry(id);
            if (entry == null)
            {
                return 1d;
            }

            return entry.Definition.MilestoneBonus.EvaluateMultiplier(entry.Level);
        }

        public bool CanBuy(string id)
        {
            var cost = GetCost(id);
            return _currencyService.GetCoins() >= cost;
        }

        public bool TryBuy(string id)
        {
            var entry = GetEntry(id);
            if (entry == null)
            {
                return false;
            }

            var cost = GetCost(id);
            if (!_currencyService.SpendSoft(CurrencyType.Coins, cost))
            {
                return false;
            }

            entry.SetLevel(entry.Level + 1);
            UpgradePurchased?.Invoke(entry);
            return true;
        }

        public void SetLevel(string id, int level)
        {
            var entry = GetEntry(id);
            if (entry != null)
            {
                entry.SetLevel(level);
            }
        }
    }
}
