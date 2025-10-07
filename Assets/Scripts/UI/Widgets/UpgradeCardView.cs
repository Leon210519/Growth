using Game.Core.Economy;
using Game.Core.Economy.UpgradeSystem;
using Game.UI.Formatting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Widgets
{
    public class UpgradeCardView : MonoBehaviour
    {
        [SerializeField] private Text _title;
        [SerializeField] private Text _level;
        [SerializeField] private Text _description;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Text _costLabel;
        private UpgradeEntry _entry;
        private UpgradeService _service;

        public void Initialize(UpgradeEntry entry, UpgradeService service)
        {
            _entry = entry;
            _service = service;
            _title.text = entry.Definition.DisplayName;
            Refresh();
            _buyButton.onClick.AddListener(OnBuy);
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(OnBuy);
        }

        private void OnBuy()
        {
            if (_service.TryBuy(_entry.Definition.Id))
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            _level.text = $"Lv {_entry.Level}";
            _costLabel.text = NumberFormatter.Format(_service.GetCost(_entry.Definition.Id));
            _description.text = _entry.Definition.Description;
        }
    }
}
