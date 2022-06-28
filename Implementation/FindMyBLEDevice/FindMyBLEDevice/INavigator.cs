// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>

using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindMyBLEDevice
{
    /// <summary>
    /// Only because of those damn unit tests...
    /// </summary>
    public interface INavigator
    {
        Task GoToAsync(string page);
    }
}
