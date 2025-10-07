#if DEV
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.DebugTools
{
    public class DebugPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _grantCoinsButton;
        private int _tapCount;
        private float _lastTapTime;

        private void Awake()
        {
            _panel.SetActive(false);
            _grantCoinsButton.onClick.AddListener(() => Debug.Log("Grant coins"));
        }

        public void OnVersionLabelTapped()
        {
            if (Time.unscaledTime - _lastTapTime < 0.5f)
            {
                _tapCount++;
                if (_tapCount >= 3)
                {
                    _panel.SetActive(!_panel.activeSelf);
                    _tapCount = 0;
                }
            }
            else
            {
                _tapCount = 1;
            }

            _lastTapTime = Time.unscaledTime;
        }
    }
}
#endif
