using DistanceModConfigurationManager.DistanceGUI.Data;

namespace DistanceModConfigurationManager.DistanceGUI.Controls
{
    internal class EmptyElement : MenuItemBase
    {
        public EmptyElement() : base(Data.MenuDisplayMode.Both, "empty", string.Empty) { }
        public EmptyElement(MenuDisplayMode mode, string id, string name) : base(mode, id, name) { } 
    }
}
