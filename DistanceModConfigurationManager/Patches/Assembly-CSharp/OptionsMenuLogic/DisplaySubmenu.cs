using DistanceModConfigurationManager.DistanceGUI.Menu;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace DistanceModConfigurationManager.Patches
{
    [HarmonyPatch(typeof(OptionsMenuLogic), "DisplaySubmenu")]
    internal static class OptionsMenuLogic__DisplaySubmenu
    {
        [HarmonyPrefix]
        internal static bool Prefix(OptionsMenuLogic __instance, string submenuName)
        {
            List<OptionsSubmenu> menus = __instance.subMenus_.ToList();

            OptionsSubmenu subMenu = menus.Find(x => x.Name_ == submenuName);

            if (subMenu && subMenu is ModdingMenu)
            {
                ModdingMenu moddingMenu = subMenu as ModdingMenu;

                if (!moddingMenu.MenuTree.Any())
                {
                    MenuSystem.ShowUnavailableMessage();
                    return false;
                }
            }

            return true;
        }
    }
}
