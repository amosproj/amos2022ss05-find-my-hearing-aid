using System;
using System.Collections.Generic;
using System.Text;

namespace FindMyBLEDevice.Models
{
    public class AvailableBTDevice
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public int Rssi { get; set; }
    }
}
