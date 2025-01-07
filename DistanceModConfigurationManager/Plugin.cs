using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DistanceModConfigurationManager.DistanceGUI.Controls;
using DistanceModConfigurationManager.DistanceGUI.Data;
//using DistanceModConfigurationManager.DistanceGUI.Menu;
using DistanceModConfigurationManager.Game;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DistanceModConfigurationManager
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public sealed class Mod : BaseUnityPlugin
    {
        //Mod Details
        private const string modGUID = "Distance.DistanceModConfigurationManager";
        private const string modName = "Distance Mod Configuration Manager";
        public const string modVersion = "1.0.1";

        //Config Entry Settings
        public static string ShowVersionKey = "Show Version Info";

        //Config Entries
        public static ConfigEntry<bool> ShowVersionInfo { get; set; }

        //Public Variables
        public int modsLoaded { get; set; }

        //Private Variables

        private string _modsWithoutSettings;

        private List<SettingEntryBase> _allSettings;
        private List<PluginSettingsData> _filteredSettings = new List<PluginSettingsData>();

        //Other
        private static readonly Harmony harmony = new Harmony(modGUID);
        public static ManualLogSource Log = new ManualLogSource(modName);
        public static Mod Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Log = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            Logger.LogInfo("Thanks for using the Distance Mod Configuration Manager!");

            CreateSettingsMenu();

            //Config Setup
            ShowVersionInfo = Config.Bind("General",
                ShowVersionKey,
                true,
                new ConfigDescription("Display the Config Manager Version in the main menu"));

            //Apply Patches
            Logger.LogInfo("Loading...");
            harmony.PatchAll();
            Logger.LogInfo("Loaded!");
        }

        private void OnConfigChanged(object sender, EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            if (settingChangedEventArgs == null) return;
        }

        private void CreateSettingsMenu()
        {
            MenuTree settingsMenu = new MenuTree("menu.distance.modding.settings", "Mod Settings");

            //settingsMenu.CheckBox(MenuDisplayMode.Both, "setting:show_version_info", modVersion, () => ShowVersionInfo.Value, (value) => ShowVersionInfo.Value = value, "Display the Config Manager Version in the main menu");

            //MenuSystem.MenuTree.SubmenuButton(MenuDisplayMode.Both, "navigate:menu.distance.modding.settings", "MOD CONFIGURATION MANAGER SETTINGS", settingsMenu, "Settings related to installed mods");
        }

        private static bool IsKeyboardShortcut(SettingEntryBase x)
        {
            return x.SettingType == typeof(KeyboardShortcut) || x.SettingType == typeof(KeyCode);
        }

        /// <summary>
        /// Rebuild the setting list. Use to update the config manager window if config settings were removed or added while it was open.
        /// </summary>
        public void BuildSettingList()
        {
            SettingSearcher.CollectSettings(out var results, out var modsWithoutSettings);

            _modsWithoutSettings = string.Join(", ", modsWithoutSettings.Select(x => x.TrimStart('!')).OrderBy(x => x).ToArray());
            _allSettings = results.ToList();
            Log.LogInfo("Settings found: " + results.Count());
            foreach(var jeff in results)
            {
                Log.LogInfo(jeff.PluginInfo.Name);
            }

            BuildFilteredSettingList();
        }

        private void BuildFilteredSettingList()
        {
            IEnumerable<SettingEntryBase> results = _allSettings;

            results = results.Where(x => x.IsAdvanced != true);
            foreach (var jeff in results)
            {
                Log.LogInfo("Filtered List: " + jeff.DispName);
            }

            List<PluginSettingsData> filteredSettings = new List<PluginSettingsData>();
            string pluginName = string.Empty;

            foreach (var setting in results)
            {
                if (pluginName != setting.PluginInfo.Name)
                {
                    pluginName = setting.PluginInfo.Name;

                    filteredSettings.Add(new PluginSettingsData { Info = setting.PluginInfo, Settings = new List<SettingEntryBase>() { setting } });
                }
                else
                {
                    PluginSettingsData pluginSettings = filteredSettings.Find(plugin => plugin.Info == setting.PluginInfo);
                    pluginSettings.Settings.Add(setting);
                }
            }

            _filteredSettings = filteredSettings;
            modsLoaded = _filteredSettings.Count;

            AddFilteredSettingsToMenu();
        }

        private void AddFilteredSettingsToMenu()
        {
            foreach(var plugin in _filteredSettings)
            {
                MenuTree settingsMenu = new MenuTree($"menu.distance.mod.{Regex.Replace(plugin.Info.Name, @"\s+", "").ToLower()}", $"{plugin.Info.Name.ToUpper()} SETTINGS");

                foreach(var setting in plugin.Settings)
                {
                    //For now, just skipping keyboard shortcuts entirely
                    if (typeof(KeyboardShortcut) != setting.SettingType)
                    {
                        settingsMenu.Add(CreateUIForSetting(setting));
                    }
                }

                Menus.AddNew(MenuDisplayMode.Both, settingsMenu, plugin.Info.Name.ToUpper(), $"Settings for the {plugin.Info.Name} mod");

            }
        }

        private MenuItemBase CreateUIForSetting(SettingEntryBase setting)
        {
            if (typeof(bool) == setting.SettingType)
            {
                return new CheckBox(MenuDisplayMode.Both, $"settings:{Regex.Replace(setting.DispName, @"\s+", "_").ToLower()}", setting.DispName.ToUpper())
                .WithGetter(() => (bool)setting.Get())
                .WithSetter((x) => setting.Set(x))
                .WithDescription($"{setting.Description}");
            }

            if (typeof(float) == setting.SettingType)
            {
                if (setting.AcceptableValueRange.Key == null)
                {
                    return new FloatSlider(MenuDisplayMode.Both, $"settings:{Regex.Replace(setting.DispName, @"\s+", "_").ToLower()}", setting.DispName.ToUpper())
                        .WithDefaultValue((float)setting.DefaultValue)
                        .LimitedByRange((float)setting.DefaultValue - 180, (float)setting.DefaultValue + 180)
                        .WithGetter(() => (float)setting.Get())
                        .WithSetter((x) => setting.Set(x))
                        .WithDescription($"{setting.Description}");
                }
                else
                {
                    return new FloatSlider(MenuDisplayMode.Both, $"settings:{Regex.Replace(setting.DispName, @"\s+", "_").ToLower()}", setting.DispName.ToUpper())
                        .WithDefaultValue((float)setting.DefaultValue)
                        .LimitedByRange((float)setting.AcceptableValueRange.Key, (float)setting.AcceptableValueRange.Value)
                        .WithGetter(() => (float)setting.Get())
                        .WithSetter((x) => setting.Set(x))
                        .WithDescription($"{setting.Description}");
                }
            }

            if (typeof(int) == setting.SettingType)
            {
                if (setting.AcceptableValueRange.Key == null)
                {
                    return new IntegerSlider(MenuDisplayMode.Both, $"settings:{Regex.Replace(setting.DispName, @"\s+", "_").ToLower()}", setting.DispName.ToUpper())
                    .WithDefaultValue((int)setting.DefaultValue)
                    .LimitedByRange((int)setting.DefaultValue, (int)setting.DefaultValue + 60)
                    .WithGetter(() => (int)setting.Get())
                    .WithSetter((x) => setting.Set(x))
                    .WithDescription($"{setting.Description}");
                }
                else
                {
                    return new IntegerSlider(MenuDisplayMode.Both, $"settings:{Regex.Replace(setting.DispName, @"\s+", "_").ToLower()}", setting.DispName.ToUpper())
                        .WithDefaultValue((int)setting.DefaultValue)
                        .LimitedByRange((int)setting.AcceptableValueRange.Key, (int)setting.AcceptableValueRange.Value)
                        .WithGetter(() => (int)setting.Get())
                        .WithSetter((x) => setting.Set(x))
                        .WithDescription($"{setting.Description}");
                }
            }

            if (setting.SettingType.IsEnum)
            {
                Dictionary<string, int> settingDict = Enum.GetNames(setting.SettingType).ToDictionary(name => name, name => (int)Enum.Parse(setting.SettingType, name));

                return new ListBox<int>(MenuDisplayMode.Both, $"settings:{Regex.Replace(setting.DispName, @"\s+", "_").ToLower()}", setting.DispName.ToUpper())
                    .WithEntries(settingDict)
                    .WithGetter(() => (int)setting.Get())
                    .WithSetter((x) => setting.Set(x))
                    .WithDescription($"{setting.Description}");
            }

            Logger.LogInfo($"Could not properly display {setting.DispName} in the menu");
            //This doesn't even display lmao
            return new EmptyElement(MenuDisplayMode.Both, $"settings:{Regex.Replace(setting.DispName, @"\s+", "_").ToLower()}", setting.DispName.ToUpper())
                .WithDescription("This setting is not properly displayed! Report this problem to the #modding channel on the Distance discord!");
        }
    }
}
