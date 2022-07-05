// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using System.Threading.Tasks;

namespace FindMyBLEDevice
{
    /// <summary>
    /// Only because of those damn unit tests...
    /// </summary>
    public interface INavigator
    {
        string AboutPage { get; }
        string DevicesPage { get; }
        string NewDevicePage { get; }
        string DeviceDetailPage { get; }
        string StrengthPage { get; }
        string MapPage { get; }
        string SettingsPage { get; }

        Task GoToAsync(string page);
    }
}
