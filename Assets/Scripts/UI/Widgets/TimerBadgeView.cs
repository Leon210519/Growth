using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Widgets
{
    public class TimerBadgeView : MonoBehaviour
    {
        [SerializeField] private Text _label;
        private DateTime _expireTime;
        private bool _active;

        public void StartTimer(TimeSpan duration)
        {
            _expireTime = DateTime.UtcNow + duration;
            _active = true;
        }

        public void StopTimer()
        {
            _active = false;
            _label.text = string.Empty;
        }

        private void Update()
        {
            if (!_active)
            {
                return;
            }

            var remaining = _expireTime - DateTime.UtcNow;
            if (remaining <= TimeSpan.Zero)
            {
                _active = false;
                _label.text = "Ready";
                return;
            }

            _label.text = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";
        }
    }
}
