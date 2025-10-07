using Game.Core.Economy;
using Game.Core.Economy.UpgradeSystem;
using NUnit.Framework;

namespace Tests.PlayMode
{
    public class UpgradeServiceTests
    {
        [Test]
        public void Purchase_DeductsCoins()
        {
            var currency = new CurrencyService();
            currency.AddSoft(CurrencyType.Coins, 1000);
            var def = new UpgradeDefinition { Id = "test", CostCurve = new Game.Core.Economy.Balancing.CostCurve { StartCost = 100, Growth = 1 } };
            var upgrades = new UpgradeService(new[] { def }, currency);
            Assert.IsTrue(upgrades.TryBuy("test"));
            Assert.Less(currency.GetCoins().RawValue, 1000);
        }
    }
}
