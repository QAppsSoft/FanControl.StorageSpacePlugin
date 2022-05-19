/*
 * PowerShell is very Memory and CPU hungry and trying to get the Disk Temperature every
 * second is a really expensive task. This class update all drives info in one pass instead
 * of per disk. The update is only run every 30 seconds to lower CPU and memory usage.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reactive.Linq;
using FanControl.StorageSpacePlugin.Infrastructure;

namespace FanControl.StorageSpacePlugin
{
    internal class DisksInfoProvider : IDisposable
    {
        private readonly IDisposable _cleanup;
        private readonly PowerShellRunner _powerShellRunner;
        private Action<bool> _autoUpdaterObserver = _ => { };
        private Dictionary<string, Disk> _disks = new Dictionary<string, Disk>();

        public DisksInfoProvider()
        {
            _powerShellRunner = new PowerShellRunner();

            var refreshRate = Config.RefreshRate();

            var autoUpdater = Observable.FromEvent<bool>(
                    eh => _autoUpdaterObserver += eh,
#pragma warning disable CS8601
                    eh => _autoUpdaterObserver -= eh)
#pragma warning restore CS8601
                .StartWith(false)
                .Select(value => value ? Observable.Timer(TimeSpan.MinValue, TimeSpan.FromSeconds(refreshRate)) : Observable.Empty<long>())
                .Switch()
                .Select(_ => _disks)
                .Select(GetTemperatures)
                .Subscribe(tuples =>
                {
                    foreach (var (serial, temperature) in tuples)
                    {
                        if (_disks.TryGetValue(serial, out var disk))
                        {
                            disk.Temperature = temperature == 0 ? Config.Defaults.FallbackTemperature : temperature;
                        }
                    }
                });

            _cleanup = autoUpdater;
        }

        /// <summary>
        /// Update the list of disks included in storage space
        /// </summary>
        public void Initialize()
        {
            using var processRunspace = RunspaceFactory.CreateOutOfProcessRunspace(null);
            processRunspace.Open();

            using var powerShell = PowerShell.Create();
            powerShell.Runspace = processRunspace;

            var executed = _powerShellRunner.RunPS(powerShell,
                "Get-PhysicalDisk | where {($_.CannotPoolReason -match 'In a Pool')}",
                out var psObjects);

            if (executed)
            {
                _disks = psObjects.Select(i =>
                        new Disk((string)i.Properties["Model"].Value,
                            (string)i.Properties["SerialNumber"].Value,
                            (string)i.Properties["UniqueId"].Value))
                    .ToDictionary(disk => disk.SerialNumber);
            }
            else
            {
                _disks = new Dictionary<string, Disk>();
            }
        }

        /// <summary>
        /// Get the list of disks included in storage space
        /// </summary>
        public Disk[] GetDisks()
        {
            var disks = _disks.Values;

            return disks.ToArray();
        }

        /// <summary>
        /// Control the disks info auto update process
        /// </summary>
        /// <param name="enabled">If set to true update disks info every few seconds</param>
        public void AutoUpdate(bool enabled)
        {
            _autoUpdaterObserver(enabled);
        }

        public void Dispose()
        {
            _cleanup.Dispose();
        }

        private (string Serial, float Temperature)[] GetTemperatures(Dictionary<string, Disk> dictionary)
        {
            var disks = dictionary.Values;

            using var processRunspace = RunspaceFactory.CreateOutOfProcessRunspace(null);
            processRunspace.Open();

            using var powerShell = PowerShell.Create();
            powerShell.Runspace = processRunspace;

            var results = new List<(string Serial, float Temperature)>();

            foreach (var disk in disks)
            {
                var temperature = GetTemperature(powerShell, disk);

                results.Add((disk.SerialNumber, temperature));
            }

            return results.ToArray();
        }

        private float GetTemperature(PowerShell ps, Disk disk)
        {
            var executed = _powerShellRunner.RunPS(ps,
                $"Get-PhysicalDisk | Where SerialNumber -EQ '{disk.SerialNumber}' | Get-StorageReliabilityCounter",
                out var psObjects);

            if (executed)
            {
                var tempString = psObjects[0].Properties["Temperature"].Value.ToString();

                if (float.TryParse(tempString, out var temp))
                {
                    return temp;
                }
            }

            return 36;
        }
    }
}