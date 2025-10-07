using Game.Core.Economy;
using Game.UI.Formatting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Widgets
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private Text _label;
        [SerializeField] private CurrencyType _type;

        public void SetValue(BigNumber value)
        {
            _label.text = NumberFormatter.Format(value);
        }

        public void SetValue(int value)
        {
            _label.text = value.ToString();
        }
    }
}
