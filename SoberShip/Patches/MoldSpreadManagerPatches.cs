using HarmonyLib;
using SoberShip.Config;
using UnityEngine;

namespace SoberShip.Patches
{
    [HarmonyPatch(typeof(MoldSpreadManager), nameof(MoldSpreadManager.GenerateMold))]
    public class MoldSpreadManagerPatches
    {
        public static bool ClearedUnwantedMold = false;

        [HarmonyPostfix]
        private static void GenerateMoldPostFix(MoldSpreadManager __instance)
        {
            if (GameNetworkManager.Instance == null || !GameNetworkManager.Instance.isHostingGame)
            {
                SoberShip.Logger.LogDebug("GenerateMoldPostFix() called on a non-host client, skipping...");
                return;
            }

            if (!ConfigOptions.RemoveNearbyVainShrouds.Value || ConfigOptions.DisableVainShroudsCompletely.Value) return;

            switch (ConfigOptions.VainShroudRemovalMethod.Value)
            {
                case ConfigOptions.RemovalMethod.DELETION:
                    DestroyNearbyMold(__instance, false);
                    return;
                default:
                    DestroyNearbyMold(__instance, true);
                    return;
            }
        }

        private static void DestroyNearbyMold(MoldSpreadManager __instance, bool permanently = false)
        {
            if (ClearedUnwantedMold) return;
            if (__instance.moldContainer.childCount <= 0) return;

            StartOfRound roundManager = StartOfRound.Instance;

            if (roundManager == null) return;

            SoberShip.Logger.LogInfo("Scanning Vain Shrouds...");

            int rem = 0;
            for (int i = 0; i < __instance.moldContainer.childCount; i++)
            {
                var mold = __instance.moldContainer.GetChild(i).gameObject;
                if (Vector3.Distance(mold.transform.position, roundManager.elevatorTransform.position) <= ConfigOptions.MinimumVainShroudDistanceFromShip.Value)
                {
                    DestroyMold(permanently, __instance, mold);
                    ++rem;
                }
                SoberShip.Logger.LogDebug(string.Format("Removed : {0} of {1} Shrouds", rem, __instance.moldContainer.childCount));
            }
            SoberShip.Logger.LogInfo("Scanned all Vain Shrouds!");
        }

        private static void DestroyMold(bool permanently, MoldSpreadManager manager, GameObject mold)
        {
            Vector3 moldPos = mold.transform.position;

            if (permanently) manager.DestroyMoldAtPosition(mold.transform.position, false);
            new SprayPaintItem().KillWeedServerRpc(mold.transform.position); // This is to remove the shrouds for clients as well, though I don't think this is the best way to do it.
            if (manager.generatedMold.Contains(mold)) manager.generatedMold.Remove(mold);
            Object.Destroy(mold);

            SoberShip.Logger.LogDebug("Destroying Vain Shroud at : " + moldPos);
        }
    }
}