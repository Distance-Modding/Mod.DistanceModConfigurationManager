using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace DistanceModConfigurationManager
{
    internal static class Utils
    {
        public static string ToProperClass(this string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            if (str.Length < 2) return str;

            // Start with the first character.
            string result = str.Substring(0, 1).ToUpper();

            // Add the remaining characters.
            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i])) result += " ";
                result += str[i];
            }

            return result;
        }

        public static string AppendZero(this string s)
        {
            return !s.Contains(".") ? s + ".0" : s;
        }

        public static string AppendZeroIfFloat(this string s, Type type)
        {
            return type == typeof(float) || type == typeof(double) || type == typeof(decimal) ? s.AppendZero() : s;
        }

        /*public static string GetWebsite(BaseUnityPlugin bepInPlugin)
        {
            if (bepInPlugin == null) return null;
            try
            {
                var fileName = bepInPlugin.Info.Location; //.GetType().Assembly.Location;

                if (!File.Exists(fileName)) return null;
                var fi = FileVersionInfo.GetVersionInfo(fileName);
                return new[]
                {
                    fi.CompanyName,
                    fi.FileDescription,
                    fi.Comments,
                    fi.LegalCopyright,
                    fi.LegalTrademarks
                }.FirstOrDefault(x => Uri.IsWellFormedUriString(x, UriKind.Absolute));
            }
            catch (Exception e)
            {
                Mod.Log.LogWarning($"Failed to get URI for {bepInPlugin.Info?.Metadata?.Name} - {e.Message}");
                return null;
            }
        }

        public static void OpenWebsite(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) throw new Exception("Empty URL");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Mod.Log.Log(LogLevel.Message | LogLevel.Warning, $"Failed to open URL {url}\nCause: {ex.Message}");
            }
        }*/
    }
}
