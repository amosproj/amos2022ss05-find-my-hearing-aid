using System;
using System.Collections.Generic;
using System.Text;

namespace FindMyBLEDevice.Services.Permission
{
    public interface IPermission
    {
        bool checkLocationPermission();
        bool checkBluetoothPermission();
    }
}
