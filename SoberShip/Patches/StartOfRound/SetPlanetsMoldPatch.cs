using HarmonyLib;
using SoberShip.Config;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace SoberShip.Patches
{
    [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.SetPlanetsMold))]
    public class SetPlanetsMoldPatch
    {
        public static bool patchSetPlanetsMoldTranspilerSuccess = false;

        [HarmonyPrefix]
        private static bool SetPlanetsMoldPrefix(StartOfRound __instance)
        {
            if (patchSetPlanetsMoldTranspilerSuccess)
            {
                SoberShip.Logger.LogDebug("SetPlanetsMoldPrefix() called.");
                if (!ConfigOptions.BringBackVainShrouds.Value) return false;
                if (GameNetworkManager.Instance == null || !GameNetworkManager.Instance.isHostingGame)
                {
                    SoberShip.Logger.LogDebug("SetPlanetsMoldPrefix() called on a non-host client, skipping...");
                    return false;
                }
            }
            return true;
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> SetPlanetsMoldTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!ConfigOptions.BringBackVainShrouds.Value) return instructions;

            bool foundIsServerCheck = false;
            int start = -1;
            int end = -1;

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ret)
                {
                    start = i + 1;
                    SoberShip.Logger.LogDebug(string.Format("SetPlanetsMoldTranspiler() : Start = {0}", start));

                    for (int j = start; j < codes.Count; j++)
                    {
                        if (codes[j].opcode == OpCodes.Ret)
                        {
                            end = j;
                            foundIsServerCheck = true;
                            SoberShip.Logger.LogDebug(string.Format("SetPlanetsMoldTranspiler() : End = {0}", end));
                            break;
                        }
                    }
                    break;
                }
            }

            int range = end - start;
            if (foundIsServerCheck && range < 4 && range > 0)
            {
                codes[start].opcode = OpCodes.Br_S;
                codes[start].operand = codes[end + 1].labels[0];
                codes.RemoveRange(start + 1, range - 1);
                patchSetPlanetsMoldTranspilerSuccess = true;
            }

            return codes.AsEnumerable();
        }
    }
}