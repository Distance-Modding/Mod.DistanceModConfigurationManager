using DistanceModConfigurationManager.Internal;
using HarmonyLib;
using System;

namespace DistanceModConfigurationManager.Patches
{
    [HarmonyPatch(typeof(SpeedrunTimerLogic), "OnEnable")]
    internal static class SpeedrunTimerLogic__OnEnable
    {
        [HarmonyPostfix]
        internal static void Postfix(SpeedrunTimerLogic __instance)
        {
            try
            {
                VersionNumber.Create(__instance.gameObject);
            }
            catch (Exception e)
            {
                Mod.Log.LogError(e);
            }
        }
    }
}
