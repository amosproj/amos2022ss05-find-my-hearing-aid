// X-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

namespace FindMyBLEDevice.Services.Location
{
    public interface IPlatformSpecificLocation
    {
        bool IsLocationServiceEnabled();
    }
}
