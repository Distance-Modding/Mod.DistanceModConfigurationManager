using DistanceModConfigurationManager.DistanceGUI.Data;
using DistanceModConfigurationManager.Game;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DistanceModConfigurationManager.DistanceGUI.Menu
{
    public static class MenuSystem
    {
        internal static GameObject MenuBlueprint { get; set; }
        internal static MenuTree MenuTree { get; set; }

        static MenuSystem()
        {
            MenuTree = new MenuTree("menu.distancemodding.main", "Configure Distance Mods");
        }

        internal static void ShowMenu(MenuTree menuTree, ModdingMenu parentMenu, int pageIndex)
        {
            if (menuTree.GetItems().Count is 0)
            {
                ShowUnavailableMessage();
                return;
            }

            foreach (var component in parentMenu.PanelObject_.GetComponents<ModdingMenu>())
            {
                component.Destroy();
            }

            var menu = Mod.Instance.GetOrAddComponent<ModdingMenu>();
            menu.MenuTree = menuTree;

            menu.CurrentPageIndex = pageIndex;
            menu.MenuPanel = MenuPanel.Create(menu.PanelObject_, true, true, false, true, true, true);
            menu.MenuPanel.backgroundOpacity_ = 0.75f;

            menu.MenuPanel.onIsTopChanged_ += (isTop) =>
            {
                if (isTop)
                {
                    menu.ResetAnimations();
                }
                else
                {
                    if (G.Sys.MenuPanelManager_.panelStack_.Contains(menu.MenuPanel))
                    {
                        menu.SwitchPage(menu.CurrentPageIndex, false, true);
                    }
                    else
                    {
                        menu.SwitchPage(0, false, true);
                    }
                }
            };

            menu.MenuPanel.onPanelPop_ += () =>
            {
                if (!G.Sys.MenuPanelManager_.panelStack_.Contains(menu.MenuPanel))
                {
                    menu.SwitchPage(0, false, true);
                    parentMenu.PanelObject_.SetActive(true);

                    if (menu.MenuTree != MenuTree)
                    {
                        menu.PanelObject_.Destroy();
                    }

                    menu.Destroy();
                }
            };

            parentMenu.PanelObject_.SetActive(false);

            menu.MenuPanel.Push();
        }

        public static void ShowUnavailableMessage()
        {
            MessageBox.Create("This menu is currently unavailable.\nNo menu entries found.", "MOD SETTINGS MANAGER")
            .SetButtons(MessagePanelLogic.ButtonType.Ok)
            .Show();
        }

        public static MenuDisplayMode GetCurrentDisplayMode()
        {
            if (string.Equals(SceneManager.GetActiveScene().name, "mainmenu", StringComparison.OrdinalIgnoreCase))
            {
                return MenuDisplayMode.MainMenu;
            }

            return MenuDisplayMode.PauseMenu;
        }
    }
}
