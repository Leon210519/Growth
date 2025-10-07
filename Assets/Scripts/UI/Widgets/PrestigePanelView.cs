using Game.Core.Economy;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Widgets
{
    public class PrestigePanelView : MonoBehaviour
    {
        [SerializeField] private Text _multiplierLabel;
        [SerializeField] private Text _shardsLabel;
        [SerializeField] private Button _prestigeButton;
        private PrestigeService _prestigeService;

        public void Initialize(PrestigeService service)
        {
            _prestigeService = service;
            Refresh();
            _prestigeButton.onClick.AddListener(OnPrestige);
        }

        private void OnDestroy()
        {
            _prestigeButton.onClick.RemoveListener(OnPrestige);
        }

        private void OnPrestige()
        {
            _prestigeService.ApplyPrestige();
            Refresh();
        }

        private void Refresh()
        {
            _multiplierLabel.text = $"x{_prestigeService.GetPrestigeMultiplier():F1}";
            _shardsLabel.text = $"Gain {_prestigeService.CalculateShardsToGain()}";
        }
    }
}
