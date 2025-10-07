using Game.Core.Economy.Balancing;
using UnityEngine;

namespace Game.Core.Economy.UpgradeSystem
{
    [CreateAssetMenu(menuName = "Game/Upgrade Definition", fileName = "UpgradeDefinition")]
    public class UpgradeDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        public string Description;
        public CostCurve CostCurve = new();
        public double BaseEffect = 1d;
        public double EffectExponentA = 1d;
        public double EffectExponentB = 1d;
        public MilestoneBonus MilestoneBonus = new();
    }
}
