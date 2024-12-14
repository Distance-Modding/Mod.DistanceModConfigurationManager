using DistanceModConfigurationManager.DistanceGUI.Data;
using DistanceModConfigurationManager.DistanceGUI.Menu;

namespace DistanceModConfigurationManager.DistanceGUI.Controls
{
    public class SubMenu : MenuItemBase
    {
        public MenuTree MenuTree { get; private set; }
        public SubMenu(MenuDisplayMode mode, string id, string name) : base(mode, id, name) { }
        public SubMenu NavigatesTo(MenuTree menuTree)
        {
            MenuTree = menuTree;
            return this;
        }
        public override void Tweak(ModdingMenu menu)
        {
            menu.TweakAction(Name, () =>
            {
                MenuSystem.ShowMenu(MenuTree, menu, 0);
                base.Tweak(menu);
            }, Description);
        }
    }
}
