using HarmonyLib;
using SoberShip.Config;
using UnityEngine;

namespace SoberShip.Patches
{
    [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.GenerateNewLevelClientRpc))]
    public class RoundManagerPatches
    {

        public static int moldIterations = 0;
        public static int moldStartPosition = -1;

        [HarmonyPrefix]
        private static void GenerateNewLevelClientRpcPrefix(RoundManager __instance, ref int moldIterations, ref int moldStartPosition)
        {
            SoberShip.Logger.LogDebug(string.Format("GenerateNewClientLevelRpcPreFix() moldIterations : {0}; moldStartPosition : {1}", moldIterations, moldStartPosition));

            if (GameNetworkManager.Instance == null || !GameNetworkManager.Instance.isHostingGame)
            {
                SoberShip.Logger.LogDebug("GenerateNewClientLevelRpcPreFix() called on a non-host client, skipping...");
                return;
            }

            if (ConfigOptions.DisableVainShroudsCompletely.Value || (ConfigOptions.RemoveExcessiveVainShrouds.Value && ConfigOptions.MaximumVainShrouds.Value <= 0))
            {
                moldIterations = 0;
                moldStartPosition = -1;
                SoberShip.Logger.LogDebug("Vain Shrouds disabled.");
                return;
            }

            if (SoberShip.forceSpawnShrouds)
            {
                moldIterations = 17;
                moldStartPosition = 10;
            }

            if (ConfigOptions.FixFalseVainShroudRemoval.Value)
            {
                if (moldIterations > 0 && moldStartPosition > 0)
                {
                    SoberShip.Logger.LogDebug(string.Format("Saving Vain Shroud state... : iterations = {0}; startPosition = {1}", moldIterations, moldStartPosition));
                    RoundManagerPatches.moldIterations = moldIterations;
                    RoundManagerPatches.moldStartPosition = moldStartPosition;
                }
            }

            if (!ConfigOptions.RelocateVainShroudSpawnPosition.Value) return;
            if (moldIterations <= 0) return;

            if (ConfigOptions.RemoveExcessiveVainShrouds.Value && moldIterations > ConfigOptions.MaximumVainShrouds.Value)
            {
                SoberShip.Logger.LogInfo(string.Format("Capping moldIterations to the specified maximum of {0}", ConfigOptions.MaximumVainShrouds.Value));
                moldIterations = ConfigOptions.MaximumVainShrouds.Value;
            }

            float minDistance = ConfigOptions.MinimumVainShroudStartDistanceFromShip.Value;
            Vector3 shipPos = StartOfRound.Instance.elevatorTransform.position;

            if (moldStartPosition <= 0 || (__instance.outsideAINodes.Length > moldStartPosition && Vector3.Distance(__instance.outsideAINodes[moldStartPosition].transform.position, shipPos) < minDistance))
            {
                SoberShip.Logger.LogInfo(string.Format("Vain Shroud starting position is {0}, which is too close to the ship, looking for a new location...", moldStartPosition));

                System.Random random = new System.Random(StartOfRound.Instance.randomMapSeed + 2017);
                int i = 0;
                int newPosition = random.Next(i, __instance.outsideAINodes.Length);
                while (Vector3.Distance(__instance.outsideAINodes[newPosition].transform.position, shipPos) < minDistance)
                {
                    newPosition = random.Next(++i, __instance.outsideAINodes.Length);
                    if (i >= __instance.outsideAINodes.Length) break;
                }

                if (Vector3.Distance(__instance.outsideAINodes[newPosition].transform.position, shipPos) >= minDistance)
                {
                    moldStartPosition = newPosition;
                    SoberShip.Logger.LogInfo(string.Format("Found a new Vain Shroud starting position ({0}) using the specified minimum distance : {1}", newPosition, minDistance));
                }
                else
                {
                    moldStartPosition = random.Next(20, __instance.outsideAINodes.Length);
                    SoberShip.Logger.LogWarning(string.Format("Randomly chose a new Vain Shroud starting point ({0}).", moldStartPosition));
                    SoberShip.Logger.LogError(string.Format("Couldn't find a new position that's further than the specified minimum ({0}), is it too high?", minDistance));
                }
            }
        }
    }
}