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
        public static ConfigEntry<bool> FixFalseVainShroudRemoval;

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
            BringBackVainShrouds = config.Bind<bool>(category, "BringBackVainShrouds", true, "Bring back Vain Shrouds (including the fox) again in v60.\nDisable this if you're having issues with other mods.");
            FixFalseVainShroudRemoval = config.Bind<bool>(category, "FixFalseVainShroudRemoval", true, "In v60, if the Vain Shrouds fail to spawn they are reset completely,\nthis setting makes sure they aren't reset if they fail to spawn.");

            category = "Relocation";
            RelocateVainShroudSpawnPosition = config.Bind<bool>(category, "AllowStartRelocation", true, "Relocate the starting position of the Vain Shrouds when they're too close to the ship.\nIn v60 this setting is only useful if you're loading a save that already has Vain Shrouds on the ship.");
            MinimumVainShroudStartDistanceFromShip = config.Bind<float>(category, "MinStartDistance", 35f, "The minimum distance the Vain Shroud starting position is required to be from the ship.");

            category = "Removal";
            RemoveNearbyVainShrouds = config.Bind<bool>(category, "AllowRemoval", true, "Remove existing Vain Shrouds that are too close to the ship.\nEnable this to prevent Vain Shrouds from spreading to the ship.");
            MinimumVainShroudDistanceFromShip = config.Bind<float>(category, "MinDistance", 35f, "The minimum distance Vain Shrouds are required to be from the ship, otherwise they're deleted.");
            VainShroudRemovalMethod = config.Bind<RemovalMethod>(category, "RemovalMethod", RemovalMethod.DELETION, "The method used to remove Vain Shrouds that are near the ship.");

            category = "Disable Vain Shrouds";
            DisableVainShroudsCompletely = config.Bind<bool>(category, "DisableCompletely", false, "Makes the mod just disable the spawning of Vain Shrouds entirely.\nIn v60, you can turn this on to remove any Vain Shrouds that spawned before the update.");

            category = "Vain Shroud Limiter";
            RemoveExcessiveVainShrouds = config.Bind<bool>(category, "CapVainShrouds", false, "Makes the mod cap the amount of Vain Shrouds that are allowed to be generated.\nThis setting does not currently work correctly.");
            MaximumVainShrouds = config.Bind<int>(category, "MaximumVainShrouds", 200, "The maximum amount of Vain Shrouds that are allowed to be generated.");
        }
    }
}
