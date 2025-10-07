using System;
using System.Collections.Generic;
using Game.Core.Economy.Balancing;

namespace Game.Core.Economy
{
    /// <summary>
    /// Tracks currency balances and raises events when they change.
    /// </summary>
    public class CurrencyService
    {
        private readonly Dictionary<CurrencyType, BigNumber> _softBalances = new();
        private readonly Dictionary<CurrencyType, int> _intBalances = new();

        public event Action<CurrencyType, BigNumber> SoftBalanceChanged;
        public event Action<CurrencyType, int> IntBalanceChanged;

        public CurrencyService()
        {
            _softBalances[CurrencyType.Coins] = new BigNumber(0);
            _softBalances[CurrencyType.Shards] = new BigNumber(0);
            _intBalances[CurrencyType.Gems] = 0;
        }

        public BigNumber GetCoins() => GetSoft(CurrencyType.Coins);
        public BigNumber GetShards() => GetSoft(CurrencyType.Shards);
        public int GetGems() => _intBalances[CurrencyType.Gems];

        public void SetSoft(CurrencyType type, BigNumber value)
        {
            _softBalances[type] = value;
            SoftBalanceChanged?.Invoke(type, value);
        }

        public void AddSoft(CurrencyType type, BigNumber delta)
        {
            var value = GetSoft(type) + delta;
            SetSoft(type, value);
        }

        public bool SpendSoft(CurrencyType type, BigNumber amount)
        {
            if (GetSoft(type) < amount)
            {
                return false;
            }

            SetSoft(type, GetSoft(type) - amount);
            return true;
        }

        public void SetInt(CurrencyType type, int value)
        {
            _intBalances[type] = value;
            IntBalanceChanged?.Invoke(type, value);
        }

        public void AddInt(CurrencyType type, int delta)
        {
            SetInt(type, GetInt(type) + delta);
        }

        public bool SpendInt(CurrencyType type, int amount)
        {
            if (GetInt(type) < amount)
            {
                return false;
            }

            SetInt(type, GetInt(type) - amount);
            return true;
        }

        public BigNumber GetSoft(CurrencyType type)
        {
            return _softBalances.TryGetValue(type, out var value) ? value : new BigNumber(0);
        }

        public int GetInt(CurrencyType type)
        {
            return _intBalances.TryGetValue(type, out var value) ? value : 0;
        }
    }
}
