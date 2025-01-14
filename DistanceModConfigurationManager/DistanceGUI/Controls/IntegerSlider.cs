using DistanceModConfigurationManager.DistanceGUI.Data;
using DistanceModConfigurationManager.DistanceGUI.Menu;
using System;

namespace DistanceModConfigurationManager.DistanceGUI.Controls
{
    public class IntegerSlider : MenuItemBase
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public Func<int> Get { get; set; }
        public Action<int> Set { get; set; }
        public int DefaultValue { get; set; }
        public IntegerSlider(MenuDisplayMode mode, string id, string name) : base(mode, id, name)
        {
            Get = () => DefaultValue;
            Set = (_) => { };
        }
        public IntegerSlider WithGetter(Func<int> getter)
        {
            Get = getter ?? throw new ArgumentNullException("Getter cannot be null.");
            return this;
        }
        public IntegerSlider WithSetter(Action<int> setter)
        {
            Set = setter ?? throw new ArgumentNullException("Setter cannot be null.");
            return this;
        }
        public IntegerSlider WithDefaultValue(int defaultValue)
        {
            if (defaultValue > Maximum || defaultValue < Minimum)
            {
                throw new ArgumentOutOfRangeException($"Default value ({defaultValue}) must be between minimum ({Minimum}) and maximum ({Maximum}) values.");
            }

            DefaultValue = defaultValue;
            return this;
        }
        public IntegerSlider LimitedByRange(int minimum, int maximum)
        {
            if (Minimum > Maximum)
            {
                throw new ArgumentOutOfRangeException("Minimum must be lower than maximum.");
            }

            Minimum = minimum;
            Maximum = maximum;

            return this;
        }
        public override void Tweak(ModdingMenu menu)
        {
            if (Get == null || Set == null)
            {
                throw new InvalidOperationException("Cannot invoke tweak with Get or Set being null.");
            }

            menu.TweakInt(Name, Get(), Minimum, Maximum, DefaultValue, Set, Description);
            base.Tweak(menu);
        }
    }
}
