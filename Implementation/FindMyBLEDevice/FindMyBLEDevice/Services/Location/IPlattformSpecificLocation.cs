// X-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Adrian Wandinger <adrian.wandinger@fau.de>

using System;
using System.Collections.Generic;
using System.Text;

namespace FindMyBLEDevice.Services.Location
{
    public interface IPlatformSpecificLocation
    {
        bool IsLocationServiceEnabled();
    }
}
