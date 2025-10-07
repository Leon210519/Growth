using System;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// Drives fixed tick updates decoupled from frame rate.
    /// </summary>
    public class TickService : MonoBehaviour
    {
        public event Action<double> Tick;
        [SerializeField] private float _tickRate = 10f;
        private float _accumulator;

        private void Update()
        {
            var delta = Time.unscaledDeltaTime;
            _accumulator += delta;
            var step = 1f / Mathf.Max(_tickRate, 0.01f);
            while (_accumulator >= step)
            {
                Tick?.Invoke(step);
                _accumulator -= step;
            }
        }
    }
}
