using BepInEx;
using System.Collections.Generic;

namespace DistanceModConfigurationManager
{
    public sealed class PluginSettingsData
    {
        public BepInPlugin Info;
        public List<SettingEntryBase> Settings;
    }
}
