// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindMyBLEDevice
{
    /// <summary>
    /// Only because of those damn unit tests...
    /// </summary>
    public class Navigator : INavigator
    {
        public string AboutPage { get; }

        public string DevicesPage { get; }

        public string NewDevicePage { get; }

        public string DeviceDetailPage { get; }

        public string StrengthPage { get; }

        public string MapPage { get; }

        public string SettingsPage { get; }

        public Navigator(
            string aboutPage,
            string devicesPage,
            string newDevicePage,
            string deviceDetailPage,
            string strengthPage, 
            string mapPage,
            string settingsPage)
        {
            AboutPage = aboutPage;
            DevicesPage = devicesPage;
            NewDevicePage = newDevicePage;
            DeviceDetailPage = deviceDetailPage;
            StrengthPage = strengthPage;
            MapPage = mapPage;
            SettingsPage = settingsPage;
        }

        public Task GoToAsync(string page, bool newStack = false)
        {
            if (newStack)
            {
                page = "//" + page;
            }
            return Shell.Current.GoToAsync(page);
        }
    }
}
