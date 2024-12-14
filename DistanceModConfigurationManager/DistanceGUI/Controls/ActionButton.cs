using DistanceModConfigurationManager.DistanceGUI.Data;
using DistanceModConfigurationManager.DistanceGUI.Menu;
using System;

namespace DistanceModConfigurationManager.DistanceGUI.Controls
{
    public class ActionButton : MenuItemBase
    {
        public Action OnClick { get; set; }
        public ActionButton(MenuDisplayMode mode, string id, string name) : base(mode, id, name) { }
        public ActionButton WhenClicked(Action onClick)
        {
            OnClick = onClick;
            return this;
        }
        public override void Tweak(ModdingMenu menu)
        {
            if (OnClick == null)
            {
                throw new InvalidOperationException("OnClick action not initialized. Use WhenClicked() to configure the action.");
            }
            else
            {
                menu.TweakAction(Name, OnClick, Description);
                base.Tweak(menu);
            }
        }
    }
}
