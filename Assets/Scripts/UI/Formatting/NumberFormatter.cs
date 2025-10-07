using Game.Core.Economy.Balancing;

namespace Game.UI.Formatting
{
    public static class NumberFormatter
    {
        public static string Format(double value, int decimals = 1)
        {
            return BigNumber.Format(value, decimals);
        }

        public static string Format(BigNumber value, int decimals = 1)
        {
            return value.ToAbbreviatedString(decimals);
        }
    }
}
