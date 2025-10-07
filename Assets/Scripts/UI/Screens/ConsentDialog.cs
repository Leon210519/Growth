using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Screens
{
    /// <summary>
    /// Simple consent dialog with placeholder hooks for Google UMP integration.
    /// </summary>
    public class ConsentDialog : MonoBehaviour
    {
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _declineButton;

        private void Awake()
        {
            _acceptButton.onClick.AddListener(() => HandleConsent(true));
            _declineButton.onClick.AddListener(() => HandleConsent(false));
        }

        private void OnDestroy()
        {
            _acceptButton.onClick.RemoveAllListeners();
            _declineButton.onClick.RemoveAllListeners();
        }

        private void HandleConsent(bool accepted)
        {
            // TODO: Integrate Google UMP
            gameObject.SetActive(false);
        }
    }
}
