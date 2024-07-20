using BepInEx.Configuration;

namespace SoberShip.Config
{
    internal class ConfigOptions
    {
        public enum PreventionMethod
        {
            DELETION,
            DELETION_PERMANENT,
            DEFLECTION
        }

        public static ConfigEntry<float> VainShroudDistanceFromShip;
        public static ConfigEntry<PreventionMethod> VainShroudPreventionMethod;

        public static void Init(ConfigFile config)
        {
            string category = "Main";
            VainShroudDistanceFromShip = config.Bind<float>(category, "Distance", 35f, "The distance Vain Shrouds are required to be from the ship.");
            VainShroudPreventionMethod = config.Bind<PreventionMethod>(category, "PreventionMethod", PreventionMethod.DELETION_PERMANENT, "The method used to prevent Vain Shrouds appearing near the ship.");
        }
    }
}
