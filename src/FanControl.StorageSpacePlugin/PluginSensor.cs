﻿using System;
using FanControl.Plugins;
using FanControl.StorageSpacePlugin.Infrastructure;

namespace FanControl.StorageSpacePlugin
{
    public class PluginSensor : IPluginSensor
    {
        private readonly Disk _disk;
        
        public PluginSensor(Disk disk)
        {
            _disk = disk ?? throw new ArgumentNullException(nameof(disk));

            Name = $"{_disk.Model} | {_disk.SerialNumber}";
        }

        public string Name { get; }

        public float? Value
        {
            get
            {
                var temperature = _disk.Temperature;
                return temperature == 0 ? Config.FallbackTemperature : temperature;
            }
        }

        public void Update() { }

        public string Id => $"{_disk.Model} | {_disk.SerialNumber}";
    }
}