using System;

namespace Game.Core
{
    /// <summary>
    /// Provides access to wall clock and monotonic time. Includes simple heuristics for time tampering detection.
    /// </summary>
    public class TimeService
    {
        private DateTime _lastRecordedTime;
        private TimeSpan _lastRecordedUptime;
        private bool _timeTampered;

        public TimeService()
        {
            _lastRecordedTime = DateTime.UtcNow;
            _lastRecordedUptime = GetAppUptime();
        }

        public DateTime UtcNow => DateTime.UtcNow;

        public TimeSpan GetAppUptime()
        {
            return TimeSpan.FromSeconds(UnityEngine.Time.realtimeSinceStartupAsDouble);
        }

        public bool CheckForTampering(DateTime previousTime, TimeSpan previousUptime, out TimeSpan delta)
        {
            var now = UtcNow;
            var uptime = GetAppUptime();
            delta = now - previousTime;
            var monotonicDelta = uptime - previousUptime;

            if (delta.TotalSeconds < 0 || delta.TotalHours > 24 || monotonicDelta.TotalSeconds < 0)
            {
                _timeTampered = true;
                delta = TimeSpan.Zero;
                return true;
            }

            _lastRecordedTime = now;
            _lastRecordedUptime = uptime;
            return false;
        }

        public bool WasTimeTampered() => _timeTampered;

        public void MarkSaved(DateTime savedTime, TimeSpan savedUptime)
        {
            _lastRecordedTime = savedTime;
            _lastRecordedUptime = savedUptime;
        }

        public (DateTime time, TimeSpan uptime) GetLastRecorded() => (_lastRecordedTime, _lastRecordedUptime);
    }
}
