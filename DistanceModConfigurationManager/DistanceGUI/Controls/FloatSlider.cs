﻿using DistanceModConfigurationManager.DistanceGUI.Data;
using DistanceModConfigurationManager.DistanceGUI.Menu;
using System;

namespace DistanceModConfigurationManager.DistanceGUI.Controls
{
    public class FloatSlider : MenuItemBase
    {
        public float Minimum { get; private set; } = 0.0f;
        public float Maximum { get; private set; } = 1.0f;
        public Func<float> Get { get; set; }
        public Action<float> Set { get; set; }
        public float DefaultValue { get; set; }
        public FloatSlider(MenuDisplayMode mode, string id, string name) : base(mode, id, name)
        {
            Get = () => DefaultValue;
            Set = (_) => { };
        }
        public FloatSlider WithDefaultValue(float defaultValue)
        {
            if (defaultValue > Maximum || defaultValue < Minimum)
            {
                throw new ArgumentOutOfRangeException($"Default value ({defaultValue}) must be between minimum ({Minimum}) and maximum ({Maximum}) values.");
            }

            DefaultValue = defaultValue;
            return this;
        }
        public FloatSlider LimitedByRange(float minimum, float maximum)
        {
            if (Minimum > Maximum)
            {
                throw new ArgumentException("Minimum must be lower than maximum.");
            }

            Minimum = minimum;
            Maximum = maximum;

            return this;
        }
        public FloatSlider WithGetter(Func<float> getter)
        {
            Get = getter ?? throw new ArgumentNullException("Getter cannot be null.");
            return this;
        }
        public FloatSlider WithSetter(Action<float> setter)
        {
            Set = setter ?? throw new ArgumentNullException("Setter cannot be null.");
            return this;
        }
        public override void Tweak(ModdingMenu menu)
        {
            if (Get == null || Set == null)
            {
                throw new InvalidOperationException("Cannot call Tweak with Get or Set method being null.");
            }

            menu.TweakFloat(Name, Get(), Minimum, Maximum, DefaultValue, Set, Description);
            base.Tweak(menu);
        }
    }
}
