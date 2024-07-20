using HarmonyLib;

namespace SoberShip.Patches
{
    [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.EndOfGame))]
    public class StartOfRoundPatches
    {
        [HarmonyPostfix]
        private static void EndOfGamePostFix(StartOfRound __instance)
        {
            MoldSpreadManagerPatches.ClearedUnwantedMold = false;
        }
    }
}