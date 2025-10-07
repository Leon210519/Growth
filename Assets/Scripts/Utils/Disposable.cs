using System;

namespace Game.Utils
{
    /// <summary>
    /// Helper to wrap disposable actions.
    /// </summary>
    public sealed class Disposable : IDisposable
    {
        private readonly Action _onDispose;
        private bool _disposed;

        public Disposable(Action onDispose)
        {
            _onDispose = onDispose;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _onDispose?.Invoke();
        }
    }
}
