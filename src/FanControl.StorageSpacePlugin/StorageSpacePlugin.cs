using System.Linq;
using FanControl.Plugins;

namespace FanControl.StorageSpacePlugin
{
    public class StorageSpacePlugin : IPlugin
    {
        private readonly DisksInfoProvider _disksInfoProvider;

        public StorageSpacePlugin()
        {
            Config.ReadConfig();
            _disksInfoProvider = new DisksInfoProvider();
            _disksInfoProvider.Initialize();
        }

        public void Initialize()
        {
            _disksInfoProvider.Initialize();
            _disksInfoProvider.AutoUpdate(true);
        }

        public void Load(IPluginSensorsContainer container)
        {
            var sensors = _disksInfoProvider.GetDisks().Select(disk => new PluginSensor(disk));

            container.TempSensors.AddRange(sensors);
        }

        public void Close()
        {
            _disksInfoProvider.AutoUpdate(false);
        }

        public string Name => "Storage Space Plugin";
    }
}
