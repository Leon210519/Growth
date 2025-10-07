namespace Game.Core.Economy.UpgradeSystem
{
    /// <summary>
    /// Runtime state for an upgrade.
    /// </summary>
    public class UpgradeEntry
    {
        public UpgradeDefinition Definition { get; }
        public int Level { get; private set; }

        public UpgradeEntry(UpgradeDefinition definition, int level)
        {
            Definition = definition;
            Level = level;
        }

        public void SetLevel(int level)
        {
            Level = level;
        }
    }
}
