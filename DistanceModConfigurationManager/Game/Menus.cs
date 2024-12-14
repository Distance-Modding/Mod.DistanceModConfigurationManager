using DistanceModConfigurationManager.DistanceGUI.Controls;
using DistanceModConfigurationManager.DistanceGUI.Data;
using DistanceModConfigurationManager.DistanceGUI.Menu;
using System;

namespace DistanceModConfigurationManager.Game
{
    public static class Menus
    {
        public static void AddNew(MenuDisplayMode displayMode, MenuTree menuTree, string description = null)
        {
            AddNew(displayMode, menuTree, menuTree.Title, description);
        }

        public static void AddNew(MenuDisplayMode displayMode, MenuTree menuTree, string title, string description = null)
        {
            try
            {
                MenuSystem.MenuTree.Add(new SubMenu(displayMode, menuTree.Id, title)
                    .NavigatesTo(menuTree)
                    .WithDescription(description)
                );

                Mod.Log.LogInfo($"Added new menu tree: '{menuTree.Id}', '{menuTree.Title}'...");
            }
            catch (Exception ex)
            {
                Mod.Log.LogError($"Failed to add the menu tree: '{menuTree.Id}', '{menuTree.Title}'.");
                Mod.Log.LogError(ex);
            }
        }
    }
}
