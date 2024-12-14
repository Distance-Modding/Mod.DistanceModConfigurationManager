using DistanceModConfigurationManager.DistanceGUI.Data;
using DistanceModConfigurationManager.DistanceGUI.Menu;
using System.Linq;
using UnityEngine;

namespace DistanceModConfigurationManager.DistanceGUI.Controls
{
    public abstract class MenuItemBase
    {
        public MenuDisplayMode Mode { get; }
        public string Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }

        protected MenuItemBase(MenuDisplayMode mode, string id, string name)
        {
            Mode = mode;
            Id = id;
            Name = name;
        }

        public MenuItemBase WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public virtual void Tweak(ModdingMenu menu)
        {
            GameObject[] items = (from x in menu.OptionsTable.GetChildren() where string.Equals(x.name, Name) select x).ToArray();

            GameObject item = null;

            if (items.Length > 0)
            {
                item = items[0];
            }

            if (item != null)
            {
                MenuItemInfo info = item.AddComponent<MenuItemInfo>();
                info.Item = this;
            }
        }
    }
}
