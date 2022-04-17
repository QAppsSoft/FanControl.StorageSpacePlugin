# FanControl.StorageSpacePlugin

This pluggin attempt to fix an [issue](https://github.com/Rem0o/FanControl.Releases/issues/109) with ussage of [LibreHardwareMonitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor) in [FanControl](https://github.com/Rem0o/FanControl.Releases).
`LibreHardwareMonitor` don´t currently support access to drives wich are part of a `Microsoft Storage Pool`, this plugin try to solve this issue using plugin system from `FanControl` and `PowerShell` to get drives temperature.

### What this plugin do:
- Get a list a of drives included in any Storage Pool.
- Get drive actual temperature.
- To avoid the high memory an cpu usage from PowerShell, the update rate is limited to 30s (Maybe in the future can be adjusted from `FanControl` app)

### Todo:
- [X] Support temperature reading for HDD
- [ ] Check if this pluging work with any SSD used by an Storage Space.
- [X] Setting to control the refresh rate instead of use a hardcoded 30s.

### See also
- [FanControl](https://github.com/Rem0o/FanControl.Releases)
- [LibreHardwareMonitor](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor)
- [No sensing HDD temp when connected to SAS controller](https://github.com/Rem0o/FanControl.Releases/issues/109)