namespace FanControl.StorageSpacePlugin.Infrastructure
{
    public class Disk
    {
        public Disk(string model, string serialNumber, string uniqueId)
        {
            Model = model;
            SerialNumber = serialNumber;
            UniqueId = uniqueId;
        }

        public string Model { get; }

        public string SerialNumber { get; }

        public string UniqueId { get; }

        public float Temperature { get; set; } = 36;
    }
}