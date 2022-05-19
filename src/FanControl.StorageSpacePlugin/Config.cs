using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace FanControl.StorageSpacePlugin
{
    internal static class Config
    {
        private static readonly Dictionary<string, Action<string>> ConfigActions = new Dictionary<string, Action<string>>();

        static Config()
        {
            ConfigActions.Add(
                ConfigValues.RefreshRateKey,
                value =>
                {
                    var result = int.TryParse(value, out var refreshRate);
                    _refreshRate = result ? refreshRate : Defaults.RefreshRateValue;
                });

            ConfigActions.Add(
                ConfigValues.FallbackTemperatureKey,
                value =>
                {
                    var result = float.TryParse(value, out var fallbackTemperature);
                    _fallbackTemperature = result ? fallbackTemperature : Defaults.FallbackTemperatureValue;
                });
        }

        public static string ApplicationPath()
        {
            var path = Assembly.GetEntryAssembly().Location;
            return Path.GetDirectoryName(path);
        }

        public static string PluginsPath()
        {
            return Path.Combine(ApplicationPath(), "Plugins");
        }

        public static string ConfigPath()
        {
            return Path.Combine(PluginsPath(), "FanControl.StorageSpacePlugin.ini");
        }

        public static void ReadConfig()
        {
            string[] lines;

            try
            {
                lines = File.ReadAllLines(ConfigPath());
            }
            catch (Exception exception)
            {
                switch (exception)
                {
                    case FileNotFoundException _:
                    case DirectoryNotFoundException _:
                    case PathTooLongException _:
                    case IOException _:
                    case UnauthorizedAccessException _:
                        return;
                    default:
                        throw; // Let FanControl handle and log the Exception
                }
            }

            foreach (var line in lines)
            {
                var configsArray = line.Split('=');

                if (configsArray.Length != 2)
                {
                    continue;
                }
                
                if (ConfigActions.TryGetValue(configsArray[0], out var configUpdate))
                {
                    configUpdate.Invoke(configsArray[1]);
                }
            }
        }

        private static int _refreshRate = Defaults.RefreshRateValue;
        public static int RefreshRate => _refreshRate;

        private static float _fallbackTemperature = Defaults.FallbackTemperatureValue;
        public static float FallbackTemperature => _fallbackTemperature;

        public static class Defaults
        {
            public static int RefreshRateValue => 30;
            public static float FallbackTemperatureValue => 36; // Default reported temperature in case of wrong PowerShell value
        }

        public static class ConfigValues
        {
            public static string RefreshRateKey => "RefreshRate";
            public static string FallbackTemperatureKey => "FallbackTemperature";
        }
    }
}