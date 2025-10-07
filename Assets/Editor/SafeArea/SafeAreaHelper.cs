using UnityEditor;
using UnityEngine;

namespace Game.EditorTools.SafeArea
{
    [ExecuteAlways]
    public class SafeAreaHelper : MonoBehaviour
    {
        private Rect _lastSafeArea;

        private void Update()
        {
            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            var safe = Screen.safeArea;
            if (safe == _lastSafeArea)
            {
                return;
            }

            _lastSafeArea = safe;
            var panel = GetComponent<RectTransform>();
            if (panel == null)
            {
                return;
            }

            Vector2 anchorMin = safe.position;
            Vector2 anchorMax = safe.position + safe.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            panel.anchorMin = anchorMin;
            panel.anchorMax = anchorMax;
        }
    }
}
