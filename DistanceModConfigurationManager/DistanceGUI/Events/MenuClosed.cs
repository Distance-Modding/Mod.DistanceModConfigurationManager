﻿using DistanceModConfigurationManager.DistanceGUI.Menu;
using Events;

namespace DistanceModConfigurationManager.DistanceGUI.Events
{
    public class MenuClosed : StaticEvent<MenuOpened.Data>
    {
        public struct Data
        {
            public ModdingMenu menu;

            public Data(ModdingMenu m)
            {
                menu = m;
            }
        }
    }
}
