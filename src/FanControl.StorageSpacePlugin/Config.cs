using System.Reflection;
using System.IO;

namespace FanControl.StorageSpacePlugin
{
    internal static class Config
    {
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
            var configText = File.ReadAllText(ConfigPath());

            var configsArray = configText.Split('=');

            var result = int.TryParse(configsArray[1], out var refreshRate);

            return result ? refreshRate : 30;
        }
    }
}
