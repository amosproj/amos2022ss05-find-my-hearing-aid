// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nico.stellwag@gmail.com>

namespace FindMyBLEDevice.XamarinAccess
{
    public interface IDeviceDisplayAccess
    {
        double Density { get; }
        double Width { get; }
    }
}