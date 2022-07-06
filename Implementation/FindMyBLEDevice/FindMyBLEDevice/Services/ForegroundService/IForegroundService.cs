// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Leo Köberlein <leo@wolfgang-koeberlein.de>
// SPDX-FileCopyrightText: 2022 Adrian Wandinger<adrian.wandinger@fau.de>
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nico.stellwag@gmail.com>

using System;

namespace FindMyBLEDevice.Services.ForegroundService
{
    public interface IForegroundService
    {
        event EventHandler<ForegroundServiceEventArgs> ServiceIteration;

        void Start();
    }
}