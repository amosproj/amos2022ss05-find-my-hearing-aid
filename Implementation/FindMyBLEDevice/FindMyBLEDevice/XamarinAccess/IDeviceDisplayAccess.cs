// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>

namespace FindMyBLEDevice.XamarinAccess
{
    public interface IDeviceDisplayAccess
    {
        double Density { get; }
        double Width { get; }
    }
}