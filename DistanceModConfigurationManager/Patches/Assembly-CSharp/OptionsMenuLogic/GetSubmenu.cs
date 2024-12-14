using DistanceModConfigurationManager.DistanceGUI.Menu;
using HarmonyLib;
using System.Collections.Generic;

namespace DistanceModConfigurationManager.Patches
{
    [HarmonyPatch(typeof(OptionsMenuLogic), "GetSubmenus")]
    internal static class OptionsMenuLogic__GetSubmenus
    {
        [HarmonyPrefix]
        internal static void Prefix(OptionsMenuLogic __instance)
        {
            foreach (var menu in __instance.subMenus_)
            {
                if (menu.GetType().IsSubclassOf(typeof(SuperMenu)))
                {
                    MenuSystem.MenuBlueprint = ((SuperMenu)menu).menuBlueprint_;
                }
            }

            ModdingMenu ModdingMenu = __instance.gameObject.AddComponent<ModdingMenu>();
            ModdingMenu.MenuTree = MenuSystem.MenuTree;

            List<OptionsSubmenu> menus = new List<OptionsSubmenu>(__instance.subMenus_);

            foreach (var menu in __instance.subMenus_)
            {
                if (menu.Name_ == ModdingMenu.Name_)
                {
                    menus.Remove(menu);
                }
            }

            menus.Add(ModdingMenu);

            __instance.subMenus_ = menus.ToArray();
        }
    }
}
