using BepInEx.Configuration;

namespace SoberShip.Config
{
    internal class ConfigOptions
    {
        public enum RemovalMethod
        {
            DELETION,
            DELETION_PERMANENT
        }

        //Misc Settings
        public static ConfigEntry<bool> BringBackVainShrouds;

        //Disable Vain Shrouds Settings
        public static ConfigEntry<bool> DisableVainShroudsCompletely;

        //Maximum Vain Shrouds Settings
        public static ConfigEntry<bool> RemoveExcessiveVainShrouds;
        public static ConfigEntry<int> MaximumVainShrouds;

        //Vain Shroud Removal Settings
        public static ConfigEntry<bool> RemoveNearbyVainShrouds;
        public static ConfigEntry<float> MinimumVainShroudDistanceFromShip;
        public static ConfigEntry<RemovalMethod> VainShroudRemovalMethod;

        //Vain Shroud Spawning Relocation Settings
        public static ConfigEntry<bool> RelocateVainShroudSpawnPosition;
        public static ConfigEntry<float> MinimumVainShroudStartDistanceFromShip;

        public static void Init(ConfigFile config)
        {
            string category = "v60 Settings";
            BringBackVainShrouds = config.Bind<bool>(category, "BringBackVainShrouds", true, "Bring back Vain Shrouds (including the fox) again in v60. Disable this if you're having issues with other mods.");

            category = "Relocation";
            RelocateVainShroudSpawnPosition = config.Bind<bool>(category, "AllowStartRelocation", true, "Relocate the starting position of Vain Shrouds when they're too close to the ship.");
            MinimumVainShroudStartDistanceFromShip = config.Bind<float>(category, "MinStartDistance", 35f, "The minimum distance the Vain Shroud starting position is required to be from the ship.");

            category = "Removal";
            RemoveNearbyVainShrouds = config.Bind<bool>(category, "AllowRemoval", true, "Remove existing Vain Shrouds that are too close to the ship. (This setting also prevents Vain Shrouds from spreading to the ship)");
            MinimumVainShroudDistanceFromShip = config.Bind<float>(category, "MinDistance", 35f, "The minimum distance Vain Shrouds are required to be from the ship, otherise they're deleted.");
            VainShroudRemovalMethod = config.Bind<RemovalMethod>(category, "RemovalMethod", RemovalMethod.DELETION, "The method used to remove Vain Shrouds that are near the ship.");

            category = "Disable Vain Shrouds";
            DisableVainShroudsCompletely = config.Bind<bool>(category, "DisableCompletely", false, "Makes the mod just disable the spawning of Vain Shrouds entirely.");

            category = "Vain Shroud Limiter";
            RemoveExcessiveVainShrouds = config.Bind<bool>(category, "CapVainShrouds", false, "Makes the mod cap the amount of Vain Shrouds that are allowed to be generated.");
            MaximumVainShrouds = config.Bind<int>(category, "MaximumVainShrouds", 200, "The maximum amount of Vain Shrouds that are allowed to be generated. (The game already has a maximum implemented, though I believe the maximum that could spawn is around 600 to 625)");
        }
    }
}
