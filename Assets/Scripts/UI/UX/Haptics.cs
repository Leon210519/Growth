using UnityEngine;

namespace Game.UI.UX
{
    public static class Haptics
    {
        public static void LightImpact()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }
    }
}
