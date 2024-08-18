using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace SoberShip.Patches
{
    [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndOfGame))]
    public class EndOfGamePatch
    {
        [HarmonyPostfix]
        private static void EndOfGamePostfix(StartOfRound __instance)
        {
            if (GameNetworkManager.Instance == null || !GameNetworkManager.Instance.isHostingGame) return;

            MoldSpreadManagerPatches.ClearedNearbyMold = false;
            MoldSpreadManagerPatches.ClearedExcessiveMold = false;
        }
    }
}