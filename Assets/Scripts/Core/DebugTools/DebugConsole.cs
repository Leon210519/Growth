#if DEV
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.DebugTools
{
    public class DebugConsole : MonoBehaviour
    {
        private readonly Queue<string> _logs = new();
        [SerializeField] private UnityEngine.UI.Text _output;

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string condition, string stackTrace, LogType type)
        {
            _logs.Enqueue(condition);
            while (_logs.Count > 20)
            {
                _logs.Dequeue();
            }

            _output.text = string.Join("\n", _logs);
        }
    }
}
#endif
