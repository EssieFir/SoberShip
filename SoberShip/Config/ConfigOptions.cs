﻿using BepInEx.Configuration;

namespace SoberShip.Config
{
    internal class ConfigOptions
    {
        public enum RemovalMethod
        {
            DELETION,
            DELETION_PERMANENT
        }

        //Disable Vain Shrouds Settings
        public static ConfigEntry<bool> DisableVainShroudsCompletely;
        public static ConfigEntry<bool> RemoveExistingVainShrouds;

        //Vain Shroud Removal Settings
        public static ConfigEntry<bool> RemoveNearbyVainShrouds;
        public static ConfigEntry<float> MinimumVainShroudDistanceFromShip;
        public static ConfigEntry<RemovalMethod> VainShroudRemovalMethod;

        //Vain Shroud Spawning Relocation Settings
        public static ConfigEntry<bool> RelocateVainShroudSpawnPosition;
        public static ConfigEntry<float> MinimumVainShroudStartDistanceFromShip;

        public static void Init(ConfigFile config)
        {
            string category = "Relocation";
            RelocateVainShroudSpawnPosition = config.Bind<bool>(category, "AllowStartRelocation", true, "Relocate the starting position of Vain Shrouds when they're too close to the ship.");
            MinimumVainShroudStartDistanceFromShip = config.Bind<float>(category, "MinStartDistance", 35f, "The minimum distance the Vain Shroud starting position is required to be from the ship.");

            category = "Removal";
            RemoveNearbyVainShrouds = config.Bind<bool>(category, "AllowRemoval", true, "Remove existing Vain Shrouds that are too close to the ship. (This setting also prevents Vain Shrouds from spreading to the ship)");
            MinimumVainShroudDistanceFromShip = config.Bind<float>(category, "MinDistance", 35f, "The minimum distance Vain Shrouds are required to be from the ship, otherise they're deleted.");
            VainShroudRemovalMethod = config.Bind<RemovalMethod>(category, "RemovalMethod", RemovalMethod.DELETION, "The method used to remove Vain Shrouds that are near the ship.");

            category = "Disable Vain Shrouds";
            DisableVainShroudsCompletely = config.Bind<bool>(category, "DisableCompletely", false, "Whether to just disable the spawning of Vain Shrouds entirely. (Enabling this disables the \"Relocation\" & \"Removal\" feature)");
            RemoveExistingVainShrouds = config.Bind<bool>(category, "RemoveRemaining", true, "Removes ALL existing Vain Shrouds if Vain Shrouds are disabled completely. (This setting does nothing if Vain Shrouds aren't disabled.)");
        }
    }
}
