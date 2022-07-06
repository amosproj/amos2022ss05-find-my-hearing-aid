// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nico.stellwag@gmail.com>

using FindMyBLEDevice.Models;
using System;
using System.Collections.Generic;

namespace FindMyBLEDevice.Services.ForegroundService
{
    public class ForegroundServiceEventArgs : EventArgs
    {
        public List<BTDevice> Devices { get; set; }
    }
}
