// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

using Xamarin.Essentials;

namespace FindMyBLEDevice.XamarinAccess
{
    public class DeviceDisplayAccess : IDeviceDisplayAccess
    {
        private DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;

        public double Width { get => displayInfo.Width; }
        public double Density { get => displayInfo.Density; }
    }
}
