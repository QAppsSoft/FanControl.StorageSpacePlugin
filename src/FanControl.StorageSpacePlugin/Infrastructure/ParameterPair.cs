namespace FanControl.StorageSpacePlugin.Infrastructure
{
    /// <summary>
    /// Class defining the PowerShell parameter in the form Name/Value.
    /// it can be replaced with any convenient dictionary class
    /// </summary>
    public class ParameterPair
    {
        public string Name { get; set; } = string.Empty;

        public object? Value { get; set; } = null;
    }
}