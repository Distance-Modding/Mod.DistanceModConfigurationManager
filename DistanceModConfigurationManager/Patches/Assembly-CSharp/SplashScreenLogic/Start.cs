using HarmonyLib;

namespace DistanceModConfigurationManager.Patches
{
    [HarmonyPatch(typeof(SplashScreenLogic), "Start")]
    internal static class SplashScreenLogic__Start
    {
        [HarmonyPostfix]
        internal static void CreateSettingList()
        {
            Mod.Instance.BuildSettingList();
        }
    }
}
