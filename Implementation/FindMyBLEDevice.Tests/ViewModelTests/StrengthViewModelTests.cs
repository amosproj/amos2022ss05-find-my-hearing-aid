// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2022 Nicolas Stellwag <nicolas.stellwag@fau.de>
// SPDX-FileCopyrightText: 2022 Dominik Pysch <dominik.pysch@fau.de>

using FindMyBLEDevice.Models;
using FindMyBLEDevice.Services.Bluetooth;
using FindMyBLEDevice.Services.Database;
using FindMyBLEDevice.Services.Settings;
using FindMyBLEDevice.ViewModels;
using FindMyBLEDevice.XamarinAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FindMyBLEDevice.Tests.ViewModelTests
{
    [TestClass]
    public class StrengthViewModelTests
    {
        [TestMethod]
        public void CircleSizesEquidistant()
        {
            // arrange
            var disAcc = new Mock<IDeviceDisplayAccess>();
            disAcc.SetupGet(mock => mock.Width).Returns(1000);
            disAcc.SetupGet(mock => mock.Density).Returns(10);
            var vm = new StrengthViewModel(disAcc.Object, null, null, null, null, null);

            // act
            var distances = new List<int>();
            for (int i = 0; i < vm.CircleSizes.Count - 1; i++)
            {
                distances.Add(Math.Abs(vm.CircleSizes[i] - vm.CircleSizes[i + 1]));
            }

            // assert
            int reference = distances[0];
            foreach (int dist in distances)
            {
                Assert.AreEqual(reference, dist, 2);
            }
        }

        [TestMethod]
        public void MaxRadiusSmallerThanScreenWidth()
        {
            // arrange
            var disAcc = new Mock<IDeviceDisplayAccess>();
            disAcc.SetupGet(mock => mock.Width).Returns(1000);
            disAcc.SetupGet(mock => mock.Density).Returns(10);
            
            // act
            var vm = new StrengthViewModel(disAcc.Object, null, null, null, null, null);

            // assert
            Assert.IsTrue(vm.CircleSizes.TrueForAll(s => s < (1000 / 10)));

        }


        [DataTestMethod]
        [DataRow(0, 10, 6, 36, 0)] // 36 = 60 * (6/10)
        [DataRow(20, 200, 100, 27, 1)] // 27 = 60 * (80/180)
        public void otherScaleToRadius_Test(double scaleMin, double scaleMax, double value, int expected, int acceptableDelta)
        {
            // arrange
            const int radiusMin = 30;
            var disAcc = new Mock<IDeviceDisplayAccess>();
            disAcc.SetupGet(mock => mock.Width).Returns(1000);
            disAcc.SetupGet(mock => mock.Density).Returns(10);
            // therefore MaxRadiusSize = 90
            var vm = new StrengthViewModel(disAcc.Object, null, null, null, null, null);


            // act
            MethodInfo methodInfo = typeof(StrengthViewModel).GetMethod("OtherScaleToRadius", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] arguments = { scaleMin, scaleMax, value };
            int res = (int) methodInfo.Invoke(vm, arguments);

            // assert
            Assert.AreEqual(radiusMin + expected, res, acceptableDelta);
        }

        [DataTestMethod]
        [DataRow(-30, 0, 10)]
        [DataRow(-80, 0, 100)]
        public void rssiToMeter_SensibleResults(int rssi, int minMeterExcl, int maxMeterExcl)
        {
            // arrange
            var disAcc = new Mock<IDeviceDisplayAccess>();
            disAcc.SetupGet(mock => mock.Width).Returns(1000);
            disAcc.SetupGet(mock => mock.Density).Returns(10);
            var vm = new StrengthViewModel(disAcc.Object, null, null, null, null, null);

            // act
            vm.CurrentRssi = rssi;
            double res = vm.Meter;

            // assert
            Assert.IsTrue(res > minMeterExcl && res < maxMeterExcl);            
        }

    }
}
