using System;
using System.Reflection;
using System.IO;

namespace FanControl.StorageSpacePlugin
{
    internal static class Config
    {
        private static int _defaultRefreshRate = 30;

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

        public static int RefreshRate()
        {
            string configText;

            try
            {
                configText = File.ReadAllText(ConfigPath());
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
                        return _defaultRefreshRate;
                    default:
                        throw; // Let FanControl handle and log the Exception
                }
            }

            var configsArray = configText.Split('=');

            var result = int.TryParse(configsArray[1], out var refreshRate);

            return result ? refreshRate : _defaultRefreshRate;
        }

        public static class Defaults
        {
            public static float FallbackTemperature => 36; // Default reported temperature in case of wrong PowerShell value
        }
    }
}
