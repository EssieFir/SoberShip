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
            switch (ConfigOptions.VainShroudPreventionMethod.Value)
            {
                case ConfigOptions.PreventionMethod.DELETION:
                    DestroyNearbyMold(__instance, false);
                    return;
                case ConfigOptions.PreventionMethod.DEFLECTION:
                    return;
                default:
                    DestroyNearbyMold(__instance, true);
                    return;
            }
        }

        private static void DestroyNearbyMold(MoldSpreadManager __instance, bool permanently = false)
        {
            if (ClearedUnwantedMold) return;
            if (__instance.generatedMold.Count <= 0) return;

            StartOfRound roundManager = StartOfRound.Instance;

            if (roundManager == null) return;

            SoberShip.Logger.LogInfo("Checking VainShrouds...");

            for (int i = 0; i < __instance.generatedMold.Count; i++)
            {
                var mold = __instance.generatedMold[i];
                if (Vector3.Distance(mold.transform.position, roundManager.elevatorTransform.position) <= ConfigOptions.VainShroudDistanceFromShip.Value)
                {
                    DestroyMold(permanently, __instance, mold);
                }
            }

            for (int i = 0; i < __instance.moldContainer.childCount; i++)
            {
                var mold = __instance.moldContainer.GetChild(i).gameObject;
                if (Vector3.Distance(mold.transform.position, roundManager.elevatorTransform.position) <= ConfigOptions.VainShroudDistanceFromShip.Value)
                {
                    DestroyMold(permanently, __instance, mold);
                }
            }
            SoberShip.Logger.LogInfo("Scanned all Vain Shrouds!");
        }

        [HarmonyPatch(nameof(MoldSpreadManager.ChooseMoldSpawnPosition))]
        private static void ChooseMoldSpawnPositionPostFix(MoldSpreadManager __instance)
        {
            if (ConfigOptions.VainShroudPreventionMethod.Value == ConfigOptions.PreventionMethod.DEFLECTION)
            {

            }
        }

        private static void DestroyMold(bool permanently, MoldSpreadManager manager, GameObject mold)
        {
            Vector3 moldPos = mold.transform.position;

            if (permanently) manager.DestroyMoldAtPosition(mold.transform.position, false);
            new SprayPaintItem().KillWeedServerRpc(mold.transform.position);
            if (manager.generatedMold.Contains(mold)) manager.generatedMold.Remove(mold);
            Object.Destroy(mold);

            SoberShip.Logger.LogInfo("Destroying Vain Shroud at : " + moldPos);
        }
    }
}