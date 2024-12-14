using BepInEx;
using System;
using System.Reflection;

namespace DistanceModConfigurationManager
{
    internal class PropertySettingEntry : SettingEntryBase
    {
        private Type _settingType;

        public object Instance { get; internal set; }
        public PropertyInfo Property { get; internal set; }

        public PropertySettingEntry(object instance, PropertyInfo settingProp, BaseUnityPlugin pluginInstance)
        {
            SetFromAttributes(settingProp.GetCustomAttributes(false), pluginInstance);
            if (Browsable == null) Browsable = settingProp.CanRead && settingProp.CanWrite;
            ReadOnly = settingProp.CanWrite;
            Property = settingProp;
            Instance = instance;
        }

        public override string DispName 
        { 
            get => string.IsNullOrEmpty(base.DispName) ? Property.Name : base.DispName; 
            protected internal set => base.DispName = value; 
        }

        public override Type SettingType => _settingType ?? (_settingType = Property.PropertyType);
        public override object Get() => Property.GetValue(Instance, null);
        protected override void SetValue(object newVal) => Property.SetValue(Instance, newVal, null);
    }
}
